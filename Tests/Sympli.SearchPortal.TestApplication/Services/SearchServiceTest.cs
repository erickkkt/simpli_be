using Moq;
using Sympli.SearchPortal.Application.Extensions;
using Sympli.SearchPortal.Application.SearchEngine.Interfaces;
using Sympli.SearchPortal.Application.Services;
using Sympli.SearchPortal.Application.Services.Interfaces;
using Sympli.SearchPortal.Domain.Enums;
using Sympli.SearchPortal.Domain.Models.Dtos;

namespace Sympli.SearchPortal.TestApplication.Services
{
    public class SearchServiceTest
    {
        private readonly Mock<ISearchEngineFactory> _mockEngineFactory;
        private readonly Mock<ISearchEngine> _mockSearchEngine;
        private readonly SearchService _searchService;

        public SearchServiceTest()
        {
            _mockEngineFactory = new Mock<ISearchEngineFactory>();
            _mockSearchEngine = new Mock<ISearchEngine>();
            _searchService = new SearchService(_mockEngineFactory.Object);
        }

        [Fact]
        public async Task SearchAsync_ReturnsSearchResponseDto_WhenValidRequest()
        {
            // Arrange
            var searchRequest = new SearchRequestDto
            {
                Keywords = "e-settlements",
                TargetUrl = "www.sympli.com.au",
                SearchEngine = SearchEngineEnum.Google
            };

            var expectedResponse = new SearchResponseDto
            {
                Positions = new List<int> { 1, 2, 3 }
            };

            _mockEngineFactory
                .Setup(f => f.GetEngine(searchRequest.SearchEngine.GetDescription()))
                .Returns(_mockSearchEngine.Object);

            _mockSearchEngine
                .Setup(e => e.SearchAsync(searchRequest))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _searchService.SearchAsync(searchRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.Positions.Count, result.Positions.Count);
        }

        [Fact]
        public async Task SearchAsync_ReturnsNull_WhenNoResultsFound()
        {
            // Arrange
            var searchRequest = new SearchRequestDto
            {
                Keywords = "nonexistent-keyword",
                TargetUrl = "www.sympli.com.au",
                SearchEngine = SearchEngineEnum.Google
            };

            _mockEngineFactory
                .Setup(f => f.GetEngine(searchRequest.SearchEngine.GetDescription()))
                .Returns(_mockSearchEngine.Object);

            _mockSearchEngine
                .Setup(e => e.SearchAsync(searchRequest))
                .ReturnsAsync((SearchResponseDto)null);

            // Act
            var result = await _searchService.SearchAsync(searchRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SearchAsync_ThrowsException_WhenEngineFactoryFails()
        {
            // Arrange
            var searchRequest = new SearchRequestDto
            {
                Keywords = "e-settlements",
                TargetUrl = "www.sympli.com.au",
                SearchEngine = SearchEngineEnum.Google
            };

            _mockEngineFactory
                .Setup(f => f.GetEngine(searchRequest.SearchEngine.GetDescription()))
                .Throws(new InvalidOperationException("Engine not found"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _searchService.SearchAsync(searchRequest));
        }
    }
}