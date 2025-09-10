using AutoMapper;
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
using System.Net;
using NUnit.Framework.Legacy;
using System.Runtime.Serialization;

namespace RealEstate.Tests.Services
{
    [TestFixture]
    public class PropertyServiceTests
    {
        private Mock<IPropertyRepository> _mockPropertyRepo;
        private Mock<IPropertyImageRepository> _mockImageRepo;
        private Mock<IMapper> _mockMapper;
        private Mock<OwnerService> _mockOwnerService;
        private Mock<PropertyTraceService> _mockTraceService;
        private PropertyService _service;
        private OwnerService _fakeOwnerService;
        private PropertyTraceService _fakeTraceService;

        [SetUp]
        public void Setup()
        {
            _mockPropertyRepo = new Mock<IPropertyRepository>();
            _mockImageRepo = new Mock<IPropertyImageRepository>();
            _mockMapper = new Mock<IMapper>();

            _fakeOwnerService = (OwnerService)FormatterServices.GetUninitializedObject(typeof(OwnerService));
            _fakeTraceService = (PropertyTraceService)FormatterServices.GetUninitializedObject(typeof(PropertyTraceService));

            _service = new PropertyService(
                _mockPropertyRepo.Object,
                _mockImageRepo.Object,
                _mockMapper.Object,
                _fakeOwnerService,
                _fakeTraceService
            );
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

            _mockMapper
    .Setup(m => m.Map<PropertyDto>(
        It.IsAny<object>(),
        It.IsAny<Action<IMappingOperationOptions<object, PropertyDto>>>()))
    .Returns((object src, Action<IMappingOperationOptions<object, PropertyDto>> opts) =>
    {
        var prop = (Property)src;

        var dto = new PropertyDto
        {
            IdProperty = prop.IdProperty,
            Name = prop.Name,
            Address = prop.Address,
            Price = prop.Price,
            ImageUrl = string.Empty
        };

        if (opts != null)
        {
            var fakeOptions = new Mock<IMappingOperationOptions<object, PropertyDto>>();
            var items = new Dictionary<string, object>();
            fakeOptions.Setup(o => o.Items).Returns(items);

            opts(fakeOptions.Object);

            if (items.TryGetValue("image", out var imgObj) && imgObj is PropertyImage img)
            {
                dto.ImageUrl = img.File;
            }
        }

        return dto;
    });


            var response = await _service.GetAllAsync(null, null, null, null);

            ClassicAssert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);
            ClassicAssert.AreEqual(2, response.Data.TotalCount);

            ClassicAssert.IsFalse(response.Data.Properties.Any(p => p == null),
                "El servicio devolvió un PropertyDto nulo en la lista");

            ClassicAssert.IsTrue(response.Data.Properties.Any(p =>
                p.IdProperty == "1" && p.ImageUrl == "image1.jpg"));

            ClassicAssert.IsTrue(response.Data.Properties.Any(p =>
                p.IdProperty == "2" && string.IsNullOrEmpty(p.ImageUrl)));
        }





        [Test]
        public async Task GetAllAsync_ReturnsEmpty_WhenNoProperties()
        {
            _mockPropertyRepo
                .Setup(repo => repo.GetAllAsync(null, null, null, null, 1, 10))
                .ReturnsAsync((new List<Property>(), 0L));

            var response = await _service.GetAllAsync(null, null, null, null);

            ClassicAssert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);
            ClassicAssert.AreEqual(0, response.Data.TotalCount);
            ClassicAssert.IsEmpty(response.Data.Properties);
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

            var property = new Property
            {
                IdProperty = "1",
                Name = request.Name,
                Address = request.Address,
                Price = request.Price,
                IdOwner = request.IdOwner
            };

            _mockMapper
                .Setup(m => m.Map<Property>(It.IsAny<CreatePropertyDto>()))
                .Returns(property);

            _mockMapper
    .Setup(m => m.Map<PropertyDto>(It.IsAny<Property>()))
    .Returns((Property prop) => new PropertyDto
    {
        IdProperty = prop.IdProperty,
        Name = prop.Name,
        Address = prop.Address,
        Price = prop.Price,
        ImageUrl = null
    });


            _mockPropertyRepo
                .Setup(repo => repo.AddAsync(It.IsAny<Property>()))
                .Returns(Task.CompletedTask);

            var response = await _service.CreateAsync(request);

            ClassicAssert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);
            ClassicAssert.IsNotNull(response.Data);
            ClassicAssert.AreEqual("Nueva Casa", response.Data.Name);
            ClassicAssert.AreEqual("Calle 123", response.Data.Address);
            ClassicAssert.AreEqual(150000, response.Data.Price);
            ClassicAssert.IsTrue(string.IsNullOrEmpty(response.Data.ImageUrl));
        }
    }
}
