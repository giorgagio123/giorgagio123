using PhoneStore.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Services.Pictures
{
    public interface IPictureService
    {
        //byte[] LoadPictureBinary(Picture picture);
        
        //string GetPictureSeName(string name);
        List<Picture> GetPicturesByIds(List<int> pictureIds);

        //string GetDefaultPictureUrl(int targetSize = 0,
        //    string storeLocation = null);
        
        //string GetPictureUrl(int pictureId,
        //    int targetSize = 0,
        //    bool showDefaultPicture = true,
        //    string storeLocation = null);

        //string GetPictureUrl(Picture picture,
        //    int targetSize = 0,
        //    bool showDefaultPicture = true,
        //    string storeLocation = null);

        //string GetThumbLocalPath(Picture picture, int targetSize = 0, bool showDefaultPicture = true);

        void DeletePicture(Picture picture);

        IList<Picture> GetPicturesByProductId(int productId, int recordsToReturn = 0);

        Picture InsertPicture(byte[] pictureBinary, string mimeType, string seoFilename, int? productId,
            string altAttribute = null, string titleAttribute = null,
            bool isNew = true);

        //void UpdatePicture(List<int> pictureIds, byte[] pictureBinary, string mimeType,
        //    string seoFilename, bool isNew = true);

        //Picture SetSeoFilename(int pictureId, string seoFilename);
        
        Picture GetPictureById(int pictureId);

        string GetPictureUrl(Picture picture, bool showDefaultPicture = true);

        //void GeneratePictureThumb(Picture picture, int targetSize = 0, bool forceDefault = false);

        //void DeletePictureThumbs(Picture picture);
    }
}
