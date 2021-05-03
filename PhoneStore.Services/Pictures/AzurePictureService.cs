using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using PhoneStore.Core;
using PhoneStore.Core.Domain;
using PhoneStore.Core.Infrastructure.Data;
using PhoneStore.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStore.Services.Pictures
{
    public class AzurePictureService : PictureService
    {
        private static CloudBlobContainer _container;
        private readonly IConfiguration _configuration;


        public AzurePictureService(IRepository<Picture> pictureRepository, ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IConfiguration configuration, WebHelper webHelper)
            : base(pictureRepository, context, hostingEnvironment, webHelper)
        {
            _configuration = configuration;
            CreateCloudBlobContainer();
        }

        protected virtual async void CreateCloudBlobContainer()
        {
            var storageAccount = CloudStorageAccount.Parse(_configuration["AzureBlob:AzureBlobStorageConnectionString"]);
            if (storageAccount == null)
                throw new Exception("Azure connection string for BLOB is not wrong");

            //should we do it for each HTTP request?
            var blobClient = storageAccount.CreateCloudBlobClient();

            //GetContainerReference doesn't need to be async since it doesn't contact the server yet
            _container = blobClient.GetContainerReference(_configuration["AzureBlob:AzureBlobStorageContainerName"]);

            await _container.CreateIfNotExistsAsync();
            await _container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
        }

        protected override async void DeletePictureOnFileSystem(Picture picture)
        {
            await DeletePictureThumbsAsync(picture);
        }

        protected override string GetPictureLocalPath(string thumbFileName)
        {
            return $"{_configuration["AzureBlob:AzureBlobStorageEndPoint"]}{_configuration["AzureBlob:AzureBlobStorageContainerName"]}/{thumbFileName}";
        }

        protected override async void SavePictureInFile(byte[] pictureBinary, string mimeType, string fileName)
        {
            await SaveThumbAsync(fileName, mimeType, pictureBinary);
        }

        //protected override bool GeneratedThumbExists(string thumbFilePath, string thumbFileName)
        //{
        //    return GeneratedThumbExistsAsync(thumbFilePath, thumbFileName).Result;
        //}

        //protected override async void SaveThumb(string thumbFilePath, string thumbFileName, string mimeType, byte[] binary)
        //{
        //    await SaveThumbAsync(thumbFilePath, thumbFileName, mimeType, binary);
        //}

        protected virtual async Task DeletePictureThumbsAsync(Picture picture)
        {
            //create a string containing the blob name prefix
            var prefix = $"{picture.SeoFilename:0000000}";

            BlobContinuationToken continuationToken = null;
            do
            {
                //get result segment
                //listing snapshots is only supported in flat mode, so set the useFlatBlobListing parameter to true.
                var resultSegment = await _container.ListBlobsSegmentedAsync(prefix, true, BlobListingDetails.All, null, continuationToken, null, null);

                //delete files in result segment
                await Task.WhenAll(resultSegment.Results.Select(blobItem => ((CloudBlockBlob)blobItem).DeleteAsync()));

                //get the continuation token.
                continuationToken = resultSegment.ContinuationToken;
            }
            while (continuationToken != null);
        }

        //protected virtual async Task<bool> GeneratedThumbExistsAsync(string thumbFilePath, string thumbFileName)
        //{
        //    try
        //    {
        //        //GetBlockBlobReference doesn't need to be async since it doesn't contact the server yet
        //        var blockBlob = _container.GetBlockBlobReference(thumbFileName);

        //        return await blockBlob.ExistsAsync();
        //    }
        //    catch { return false; }
        //}

        protected virtual async Task SaveThumbAsync(string thumbFileName, string mimeType, byte[] binary)
        {
            //GetBlockBlobReference doesn't need to be async since it doesn't contact the server yet
            var blockBlob = _container.GetBlockBlobReference(thumbFileName);

            //set mime type
            if (!string.IsNullOrEmpty(mimeType))
                blockBlob.Properties.ContentType = mimeType;

            //set cache control
            //if (!string.IsNullOrEmpty(_mediaSettings.AzureCacheControlHeader))
            //    blockBlob.Properties.CacheControl = _mediaSettings.AzureCacheControlHeader;

            await blockBlob.UploadFromByteArrayAsync(binary, 0, binary.Length);
            
        }
    }
}
