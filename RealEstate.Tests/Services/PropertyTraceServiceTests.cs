using AutoMapper;
using Moq;
using NUnit.Framework;
using RealEstate.Application.DTOs;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace RealEstate.Tests.Services
{
    [TestFixture]
    public class PropertyTraceServiceTests
    {
        private Mock<IPropertyTraceRepository> _propertyTraceRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private PropertyTraceService _service;

        [SetUp]
        public void SetUp()
        {
            _propertyTraceRepositoryMock = new Mock<IPropertyTraceRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new PropertyTraceService(_propertyTraceRepositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task CreateAsync_ShouldReturnOk_WhenPropertyTraceIsCreated()
        {
            var request = new CreatePropertyTraceDto { IdProperty = "1" };
            var propertyTrace = new PropertyTrace { IdProperty = "1" };
            var propertyTraceDto = new PropertyTraceDto { IdPropertyTrace = "trace-123" };

            _mapperMock.Setup(m => m.Map<PropertyTrace>(request)).Returns(propertyTrace);
            _mapperMock.Setup(m => m.Map<PropertyTraceDto>(propertyTrace)).Returns(propertyTraceDto);
            _propertyTraceRepositoryMock.Setup(r => r.AddAsync(propertyTrace)).Returns(Task.CompletedTask);

            var response = await _service.CreateAsync(request);

            Assert.That(response.HttpStatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.IdPropertyTrace, Is.EqualTo("trace-123"));
        }

        [Test]
        public async Task CreateAsync_ShouldReturnError_WhenExceptionIsThrown()
        {
            var request = new CreatePropertyTraceDto { IdProperty = "1" };
            _mapperMock.Setup(m => m.Map<PropertyTrace>(request)).Throws(new Exception("Error mapping"));

            var response = await _service.CreateAsync(request);

            Assert.That(response.HttpStatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
            Assert.That(response.Message, Is.EqualTo("Error en el service de Property Trace"));
        }


        [Test]
        public async Task GetByIdPropertyAsync_ShouldReturnNotFound_WhenPropertyTraceDoesNotExist()
        {

            _propertyTraceRepositoryMock.Setup(r => r.GetByIdPropertyAsync("1")).ReturnsAsync((PropertyTrace)null);

            var response = await _service.GetByIdPropertyAsync("1");

            Assert.That(response.HttpStatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(response.Data, Is.Null);
            Assert.That(response.Message, Does.Contain("No se encontró ningún PropertyTrace"));
        }

        [Test]
        public async Task GetByIdPropertyAsync_ShouldReturnError_WhenExceptionIsThrown()
        {
            _propertyTraceRepositoryMock.Setup(r => r.GetByIdPropertyAsync("1")).ThrowsAsync(new Exception("DB error"));

            var response = await _service.GetByIdPropertyAsync("1");

            Assert.That(response.HttpStatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
            Assert.That(response.Message, Is.EqualTo("Error en el service de Property Trace"));
        }
    }
}
