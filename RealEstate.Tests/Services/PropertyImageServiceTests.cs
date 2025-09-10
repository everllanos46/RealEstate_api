using Moq;
using NUnit.Framework.Legacy;
using NUnit.Framework;
using RealEstate.Application.DTOs;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Net;

namespace RealEstate.Tests.Services
{
    [TestFixture]
    public class PropertyImageServiceTests
    {
        private Mock<IPropertyImageRepository> _mockImageRepo;
        private Mock<IFileStorageRepository> _mockStorageRepo;
        private PropertyImageService _service;

        [SetUp]
        public void Setup()
        {
            _mockImageRepo = new Mock<IPropertyImageRepository>();
            _mockStorageRepo = new Mock<IFileStorageRepository>();
            _service = new PropertyImageService(_mockImageRepo.Object, _mockStorageRepo.Object);
        }

        [Test]
        public async Task UploadAsync_Success_ReturnsPropertyImage()
        {
            string propertyId = "prop-1";
            var fileDto = new UploadFileDto
            {
                FileName = "test.jpg",
                Content = new MemoryStream(new byte[] { 1, 2, 3 }),
                ContentType = "image/jpeg"
            };

            string expectedUrl = "https://storage.test/prop-1/test.jpg";

            _mockStorageRepo
                .Setup(s => s.UploadAsync(It.IsAny<string>(), It.IsAny<Stream>(), fileDto.ContentType))
                .ReturnsAsync(expectedUrl);

            _mockImageRepo
                .Setup(r => r.AddAsync(It.IsAny<PropertyImage>()))
                .Returns(Task.CompletedTask);

            var result = await _service.UploadAsync(propertyId, fileDto);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(propertyId, result.Data.IdProperty);
            ClassicAssert.AreEqual(expectedUrl, result.Data.File);
            ClassicAssert.IsTrue(result.Data.Enabled);
        }

        [Test]
        public async Task UploadAsync_WhenFileIsNull_ReturnsErrorResponse()
        {
            string propertyId = "prop-1";

            var result = await _service.UploadAsync(propertyId, null!);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(HttpStatusCode.InternalServerError, result.HttpStatusCode);
            ClassicAssert.AreEqual("El archivo es obligatorio", result.Message);
        }
    }
}
