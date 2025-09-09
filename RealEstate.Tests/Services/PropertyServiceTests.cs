using Moq;
using NUnit.Framework;
using RealEstate.Application.DTOs;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework.Legacy;

namespace RealEstate.Tests.Services
{
    [TestFixture]
    public class PropertyServiceTests
    {
        private Mock<IPropertyRepository> _mockPropertyRepo;
        private Mock<IPropertyImageRepository> _mockImageRepo;
        private PropertyService _service;

        [SetUp]
        public void Setup()
        {
            _mockPropertyRepo = new Mock<IPropertyRepository>();
            _mockImageRepo = new Mock<IPropertyImageRepository>();
            _service = new PropertyService(_mockPropertyRepo.Object, _mockImageRepo.Object);
        }

        [Test]
        public async Task GetAllAsync_ReturnsPropertiesWithImages()
        {
            var properties = new List<Property>
            {
                new Property { IdProperty = "1", Name = "Casa A", Address = "Calle 1", Price = 100000 },
                new Property { IdProperty = "2", Name = "Casa B", Address = "Calle 2", Price = 200000 }
            };

            var images = new List<PropertyImage>
            {
                new PropertyImage { IdProperty = "1", File = "image1.jpg", Enabled = true }
            };

            _mockPropertyRepo
                .Setup(repo => repo.GetAllAsync(null, null, null, null, 1, 10))
                .ReturnsAsync((properties, 2L));

            _mockImageRepo
                .Setup(repo => repo.GetAllAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(images);

            var (resultProperties, totalCount) = await _service.GetAllAsync(null, null, null, null);

            ClassicAssert.AreEqual(2, totalCount);
            ClassicAssert.AreEqual(2, resultProperties.Count());

            ClassicAssert.IsTrue(resultProperties.Any(p => p.IdProperty == "1" && p.ImageUrl == "image1.jpg"));

            ClassicAssert.IsTrue(resultProperties.Any(p => p.IdProperty == "2" && string.IsNullOrEmpty(p.ImageUrl)));
        }

        [Test]
        public async Task GetAllAsync_ReturnsEmpty_WhenNoProperties()
        {
            _mockPropertyRepo
                .Setup(repo => repo.GetAllAsync(null, null, null, null, 1, 10))
                .ReturnsAsync((new List<Property>(), 0L));

            var (resultProperties, totalCount) = await _service.GetAllAsync(null, null, null, null);

            ClassicAssert.AreEqual(0, totalCount);
            ClassicAssert.IsEmpty(resultProperties);
        }

        [Test]
        public async Task CreateAsync_ReturnsPropertyDto()
        {
            var request = new CreatePropertyDto
            {
                Name = "Nueva Casa",
                Address = "Calle 123",
                Price = 150000,
                IdOwner = "owner-1"
            };

            _mockPropertyRepo
                .Setup(repo => repo.AddAsync(It.IsAny<Property>()))
                .Returns(Task.CompletedTask);

            _mockImageRepo
                .Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((PropertyImage?)null);

            var result = await _service.CreateAsync(request);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual("Nueva Casa", result.Name);
            ClassicAssert.AreEqual("Calle 123", result.Address);
            ClassicAssert.AreEqual(150000, result.Price);
            ClassicAssert.IsTrue(string.IsNullOrEmpty(result.ImageUrl));
        }
    }
}
