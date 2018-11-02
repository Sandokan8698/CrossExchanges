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
    public class ShareControllerTests
    {
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();

        private readonly ShareController _shareController;

        public ShareControllerTests()
        {
            _shareController = new ShareController(_shareRepositoryMock.Object);
        }

        [Test]
        public async Task Post_ShouldInsertHourlySharePrice()
        {
            // Arrenge
            var hourRate = new Share
            {
                Id = 1,
                Symbol = "CBI",
                Rate = 330.0M,
                TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
            };

            _shareRepositoryMock
                .Setup(m => m.InsertAsync(It.IsAny<Share>()))
                .Returns(Task.FromResult(hourRate));

            // Act
            var result = await _shareController.Post(hourRate);

            // Assert
            var okResult = result.Should().BeOfType<CreatedResult>().Subject;
            var shareCreated = okResult.Value.Should().BeAssignableTo<Share>().Subject;

            shareCreated.Id.Should().Be(1);
            shareCreated.Symbol.Should().Be("CBI");
            shareCreated.Rate.Should().Be(330.0M);
            shareCreated.TimeStamp.Should().Be(new DateTime(2018, 08, 17, 5, 0, 0));

            _shareRepositoryMock.Verify(mock => mock.InsertAsync(It.IsAny<Share>()));
        }

        [Test]
        public async Task CanReturnShareBySymbol()
        {
            // Arrege
            _shareRepositoryMock
                .Setup(m => m.FindAsync(It.IsAny<Expression<Func<Share, bool>>>()))
                .Returns(Task.FromResult(new List<Share>()
                {
                    new Share()
                }));

            // Act
            var result = await _shareController.Get("REL");

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var shareList = okResult.Value.Should().BeAssignableTo<IEnumerable<Share>>().Subject;

            shareList.Should().NotBeEmpty();
            _shareRepositoryMock.Verify(mock => mock.FindAsync(It.IsAny<Expression<Func<Share, bool>>>()));

        }


        [Test]
        public async Task CanReturnLatestPriceBySymbol()
        {
            // Arrege
            _shareRepositoryMock
                .Setup(m => m.FindLastBySymbolAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new Share(){Rate = 100.0M}));

            // Act
            var result = await _shareController.GetLatestPrice("REL");

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var lastetPrice = okResult.Value;

            Assert.AreEqual(100.0M, lastetPrice);
            _shareRepositoryMock.Verify(mock => mock.FindLastBySymbolAsync(It.IsAny<string>()));

        }
    
        [Test]
        public async Task ShareControllerReturnsBadRequestWhenModelStateIsInvalid()
        {
            // Arrange
            var hourRate = new Share {Symbol = "REL", Rate = 100.0M};
            _shareController.ModelState.AddModelError("TimeStamp", "The input was not valid.");

            // Act
            var result = await _shareController.Post(hourRate);

            // Assert
            var badRequest = result.Should().BeOfType<BadRequestObjectResult>();
            Assert.AreEqual(new SerializableError(_shareController.ModelState), badRequest.Subject.Value);
        }

    }
}
