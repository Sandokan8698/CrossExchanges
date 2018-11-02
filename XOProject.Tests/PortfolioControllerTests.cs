using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using XOProject.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;

namespace XOProject.Tests
{
    public class PortfolioControllerTests
    {
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock = new Mock<IPortfolioRepository>();

        private readonly PortfolioController _portfolioController;

        public PortfolioControllerTests()
        {
            _portfolioController = new PortfolioController(_portfolioRepositoryMock.Object);
        }

        [Test]
        public async Task CanGetGetPortfolioInfo()
        {
            // Arrege
            var portFolio = new Portfolio();
            _portfolioRepositoryMock
                .Setup(m => m.FindByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(portFolio));

            // Act
            var result = await _portfolioController.GetPortfolioInfo(1);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var portFolioResult = okResult.Value;

            Assert.AreEqual(portFolio, portFolioResult);
            _portfolioRepositoryMock.Verify(mock => mock.FindByIdAsync(It.IsAny<int>()));

        }
        
        [Test]
        public async Task PostMethodReturnsBadRequestWhenModelStateIsInvalid()
        {
            // Arrange
            var portFolio = new Portfolio() {Name = ""};
            _portfolioController.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = await _portfolioController.Post(portFolio);

            // Assert
            var badRequest = result.Should().BeOfType<BadRequestObjectResult>();
            Assert.AreEqual(new SerializableError(_portfolioController.ModelState), badRequest.Subject.Value);
        }

        
    }
}