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
    public class TradeControllerTests
    {
        private readonly Mock<ITradeRepository> _tradeRepositoryMock = new Mock<ITradeRepository>();

        private readonly TradeController _tradeController;

        public TradeControllerTests()
        {
            _tradeController = new TradeController(_tradeRepositoryMock.Object);
        }

        [Test]
        public async Task CanGetAllTradingsByPortafolioId()
        {
            // Arrege
            _tradeRepositoryMock
                .Setup(m => m.FindAsync(It.IsAny<Expression<Func<Trade, bool>>>())).Returns(Task.FromResult(new List<Trade>()
                {
                    new Trade()
                }));

            // Act
            var result = await _tradeController.GetAllTradings(1);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var tradeList = okResult.Value.Should().BeAssignableTo<IEnumerable<Trade>>().Subject;

            tradeList.Should().NotBeEmpty();
            _tradeRepositoryMock.Verify(mock => mock.FindAsync(It.IsAny<Expression<Func<Trade, bool>>>()));

        }
        
        [Test]
        public async Task CanGetAnalysisBySymbol()
        {
            // Arrege
            _tradeRepositoryMock
                .Setup(m => m.GetAnalysis(It.IsAny<string>())).Returns(Task.FromResult(new List<TradeAnalysis>()
                {
                    new TradeAnalysis()
                }));

            // Act
            var result = await _tradeController.GetAnalysis("REL");

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var tradeAnaysisLit = okResult.Value.Should().BeAssignableTo<IEnumerable<TradeAnalysis>>().Subject;

            tradeAnaysisLit.Should().NotBeEmpty();
            _tradeRepositoryMock.Verify(mock => mock.GetAnalysis(It.IsAny<string>()));

        }

        
    }
}