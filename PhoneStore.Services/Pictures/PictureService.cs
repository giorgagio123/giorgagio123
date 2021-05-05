using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using PhoneStore.Core;
using PhoneStore.Core.Domain;
using PhoneStore.Core.Infrastructure.Data;
using PhoneStore.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace PhoneStore.Services.Pictures
{
    public class PictureService : IPictureService
    {
        private readonly IRepository<Picture> _pictureRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        protected readonly WebHelper _webHelper;

        public PictureService(IRepository<Picture> pictureRepository, ApplicationDbContext context, IHostingEnvironment hostingEnvironment,
            WebHelper webHelper)
        {
            _pictureRepository = pictureRepository;
            _hostingEnvironment = hostingEnvironment;
            _webHelper = webHelper;
        }

        protected string GetImageFolder()
        {
            return "images";
        }

        protected virtual void SavePictureInFile(byte[] pictureBinary, string mimeType, string fileName)
        {
            var path = GetPictureWebRootPath(fileName);
            File.WriteAllBytes(path, pictureBinary);
        }

        protected virtual void DeletePictureOnFileSystem(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));
            
            var fileName = picture.SeoFilename;
            var filePath = GetPictureWebRootPath(fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private string GetPictureWebRootPath(string fileName)
        {
            return Path.Combine(_hostingEnvironment.WebRootPath, GetImageFolder(), fileName);
        }

        protected virtual string GetPictureLocalPath(string fileName)
        {
            fileName = fileName ?? string.Empty;
            return Path.Combine(_webHelper.GetStoreLocation(), GetImageFolder(), fileName);
        }

        public virtual Picture GetPictureById(int pictureId)
        {
            if (pictureId == 0)
                return null;

            return _pictureRepository.GetById(pictureId);
        }

        public virtual List<Picture> GetPicturesByIds(List<int> pictureIds)
        {
            List<Picture> pictures = new List<Picture>();
            if (pictureIds.Any())
            {
                foreach (var id in pictureIds)
                {
                    pictures.Add(_pictureRepository.GetById(id));
                }
            }

            return pictures;
        }

        public virtual void DeletePicture(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));
            
            var pictures = _pictureRepository.Table.Where(p => p.SeoFilename == picture.SeoFilename);

            if (pictures.Count() == 1)
            {
                //delete from file system
                DeletePictureOnFileSystem(picture);
            }

            //delete from database
            _pictureRepository.Delete(picture);
            
        }

        public virtual IList<Picture> GetPicturesByProductId(int productId, int recordsToReturn = 0)
        {
            if (productId == 0)
                return new List<Picture>();


            var query = _pictureRepository.Table.Where(p => p.ProductId == productId);

            if (recordsToReturn > 0)
                query = query.Take(recordsToReturn);

            var pics = query.ToList();
            return pics;
        }

        public virtual Picture InsertPicture(byte[] pictureBinary, string mimeType, string seoFilename, int? productId,
            string altAttribute = null, string titleAttribute = null,
            bool isNew = true)
        {
            var picture = new Picture
            {
                PictureBinary = pictureBinary,
                MimeType = mimeType,
                SeoFilename = seoFilename,
                IsNew = isNew,
                ProductId = productId
            };

            _pictureRepository.Insert(picture);
            
            SavePictureInFile(pictureBinary, mimeType, picture.SeoFilename);
            
            return picture;
        }

        public virtual string GetDefaultPictureUrl(string storeLocation = null)
        {
            string defaultImageFileName = "default.jpg";
            var filePath = GetPictureWebRootPath(defaultImageFileName);

            if (!File.Exists(filePath))
            {
                return "";
            }

            return GetPictureLocalPath(defaultImageFileName);
        }

        public virtual string GetPictureUrl(Picture picture,bool showDefaultPicture = true)
        {
            var url = string.Empty;

            if (picture == null)
            {
                if (showDefaultPicture)
                {
                    url = GetDefaultPictureUrl("");
                }

                return url;
            }

            url = GetPictureLocalPath(picture.SeoFilename);
            return url;
        }
    }
}
