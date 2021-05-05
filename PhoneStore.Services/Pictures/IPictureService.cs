using PhoneStore.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Services.Pictures
{
    public interface IPictureService
    {
        List<Picture> GetPicturesByIds(List<int> pictureIds);

        void DeletePicture(Picture picture);

        IList<Picture> GetPicturesByProductId(int productId, int recordsToReturn = 0);

        Picture InsertPicture(byte[] pictureBinary, string mimeType, string seoFilename, int? productId,
            string altAttribute = null, string titleAttribute = null,
            bool isNew = true);

        Picture GetPictureById(int pictureId);

        string GetPictureUrl(Picture picture, bool showDefaultPicture = true);
    }
}
