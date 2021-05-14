using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Moq;
using PhoneStore.Controllers;
using PhoneStore.Core;
using PhoneStore.Core.Domain;
using PhoneStore.Core.Infrastructure.Data;
using PhoneStore.Data;
using PhoneStore.Services.Pictures;
using PhoneStore.Tests.Factory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace PhoneStore.Tests.Services
{
    public class PictureServiceTests : IClassFixture<ApplicationFactory<Startup>>
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IPictureService _pictureService;
        private readonly IRepository<Picture> _pictureRepository;
        private readonly Mock<WebHelper> _fakeWebHelper;
        private readonly IConfiguration _configuration;

        public PictureServiceTests(ApplicationFactory<Startup> fixture)
        {
            var client = fixture.CreateClient();
            _hostingEnvironment = fixture.Server.Host.Services.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;
            _configuration = fixture.Server.Host.Services.GetService(typeof(IConfiguration)) as IConfiguration;

            var db = fixture.Server.Host.Services.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            _pictureRepository = new EfRepository<Picture>(db);

            _fakeWebHelper = new Mock<WebHelper>(new Mock<HttpContextAccessor>().Object);
            _fakeWebHelper.Setup(w => w.GetStoreLocation(It.IsAny<bool?>())).Returns(_hostingEnvironment.WebRootPath);

            _pictureService = new PictureService(_pictureRepository, _hostingEnvironment, _fakeWebHelper.Object);
        }

        [Fact]
        public void Insert_Picture_And_Check_In_Local_File_System()
        {
            //Arrange
            var fileName = "test.jpg";
            GetPictureBinary(out var fileBytes);

            var pictureUrl = string.Empty;

            //Act
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var picture = _pictureService.InsertPicture(fileBytes, MimeTypes.ImageJpeg, fileName, null);
            pictureUrl = _pictureService.GetPictureUrl(picture);

            //Assert
            Assert.NotNull(picture);
            Assert.Equal(Path.Combine(_hostingEnvironment.WebRootPath, "images", fileName), pictureUrl);
        }

        [Fact]
        public void Insert_Picture_And_Check_In_Local_File_Systemm()
        {
            //Arrange
            var fakePictureRepository = new Mock<IRepository<Picture>>();
            var fakeHostingEnviroment = new Mock<IHostingEnvironment>();
            var azurePictureService = new AzurePictureService(fakePictureRepository.Object, fakeHostingEnviroment.Object, _configuration, _fakeWebHelper.Object);
            
            var pictureUrl = string.Empty;
            var fileName = "test.jpg";
            GetPictureBinary(out var fileBytes);

            //Act
            var picture = azurePictureService.InsertPicture(fileBytes, MimeTypes.ImageJpeg, fileName, null);
            pictureUrl = azurePictureService.GetPictureUrl(picture);

            //Assert
            Assert.NotNull(picture);
            Assert.NotEmpty(pictureUrl);
        }

        private void GetPictureBinary(out byte[] fileBytes)
        {
            using (var fileStream = _hostingEnvironment.WebRootFileProvider.GetFileInfo("test.jpg").CreateReadStream())
            using (var ms = new MemoryStream())
            {
                fileStream.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
        }
    }
}
