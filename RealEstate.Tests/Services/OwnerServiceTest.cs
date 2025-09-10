using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using RealEstate.Application.DTOs;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using System.Net;

namespace RealEstate.Tests.Services
{
    [TestFixture]
    public class OwnerServiceTests
    {
        private Mock<IOwnerRepository> _mockOwnerRepo;
        private Mock<IFileStorageRepository> _mockStorageRepo;
        private Mock<AutoMapper.IMapper> _mockMapper;
        private OwnerService _service;

        [SetUp]
        public void SetUp()
        {
            _mockOwnerRepo = new Mock<IOwnerRepository>();
            _mockStorageRepo = new Mock<IFileStorageRepository>();
            _mockMapper = new Mock<AutoMapper.IMapper>();
            _service = new OwnerService(_mockOwnerRepo.Object, _mockStorageRepo.Object, _mockMapper.Object);
        }

        [Test]
        public async Task UploadAsync_ReturnsUpdatedOwnerDto_WhenFileIsValid()
        {
            var ownerId = "owner-1";
            var file = new UploadFileDto
            {
                FileName = "photo.jpg",
                Content = new MemoryStream(new byte[] { 1, 2, 3 }),
                ContentType = "image/jpeg"
            };

            var url = "https://storage.com/owner-1/photo.jpg";
            var owner = new Owner { IdOwner = ownerId, Name = "Carlos", Photo = url };
            var ownerDto = new OwnerDto { IdOwner = ownerId, Name = "Carlos", Photo = url };

            _mockStorageRepo.Setup(r => r.UploadAsync(It.IsAny<string>(), file.Content, file.ContentType))
                            .ReturnsAsync(url);
            _mockOwnerRepo.Setup(r => r.UpdatePhotoAsync(ownerId, url))
                          .ReturnsAsync(owner);
            _mockMapper.Setup(m => m.Map<OwnerDto>(owner))
                       .Returns(ownerDto);

            var response = await _service.UploadAsync(ownerId, file);
            ClassicAssert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);
            ClassicAssert.IsNotNull(response.Data);
            ClassicAssert.AreEqual("Carlos", response.Data.Name);
            ClassicAssert.AreEqual(url, response.Data.Photo);
        }

        [Test]
        public async Task UploadAsync_ReturnsError_WhenFileIsNull()
        {
            var response = await _service.UploadAsync("owner-1", null);

            ClassicAssert.AreEqual(HttpStatusCode.InternalServerError, response.HttpStatusCode);
            ClassicAssert.IsNull(response.Data);
        }

        [Test]
        public async Task CreateAsync_ReturnsOwnerDto_WhenValidRequest()
        {
            var request = new CreateOwnerDto { Name = "Ana", Address = "Calle 123", Birthday = DateTime.UtcNow };
            var owner = new Owner { IdOwner = "1", Name = request.Name, Address = request.Address, Birthday = request.Birthday };
            var ownerDto = new OwnerDto { IdOwner = "1", Name = request.Name, Address = request.Address };

            _mockMapper.Setup(m => m.Map<Owner>(request)).Returns(owner);
            _mockOwnerRepo.Setup(r => r.AddAsync(owner)).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<OwnerDto>(owner)).Returns(ownerDto);

            var response = await _service.CreateAsync(request);

            ClassicAssert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);
            ClassicAssert.IsNotNull(response.Data);
            ClassicAssert.AreEqual("Ana", response.Data.Name);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsOwnerDto_WhenOwnerExists()
        {
            var ownerId = "owner-1";
            var owner = new Owner { IdOwner = ownerId, Name = "Luis" };
            var ownerDto = new OwnerDto { IdOwner = ownerId, Name = "Luis" };

            _mockOwnerRepo.Setup(r => r.GetByIdAsync(ownerId)).ReturnsAsync(owner);
            _mockMapper.Setup(m => m.Map<OwnerDto>(owner)).Returns(ownerDto);

            var response = await _service.GetByIdAsync(ownerId);


            ClassicAssert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);
            ClassicAssert.IsNotNull(response.Data);
            ClassicAssert.AreEqual("Luis", response.Data.Name);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNotFound_WhenOwnerDoesNotExist()
        {
            var ownerId = "owner-404";
            _mockOwnerRepo.Setup(r => r.GetByIdAsync(ownerId)).ReturnsAsync((Owner)null);

            var response = await _service.GetByIdAsync(ownerId);

            ClassicAssert.AreEqual(HttpStatusCode.NotFound, response.HttpStatusCode);
            ClassicAssert.IsNull(response.Data);
        }
    }
}
