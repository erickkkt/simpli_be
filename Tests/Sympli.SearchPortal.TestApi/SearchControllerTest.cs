using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Simpli.SearchPortal.Api.Controllers;
using Sympli.SearchPortal.Application.Services.Interfaces;
using Sympli.SearchPortal.Domain.Enums;
using Sympli.SearchPortal.Domain.Models.Dtos;

namespace Sympli.SearchPortal.TestApi
{
    public class SearchControllerTest
    {
        private readonly Mock<ISearchService> _mockSearchService;
        private readonly SearchController _controller;

        public SearchControllerTest()
        {
            _mockSearchService = new Mock<ISearchService>();
            _controller = new SearchController(_mockSearchService.Object);
        }

        [Fact]
        public async Task Search_ReturnsOkResult_WithSearchResults_Google()
        {
            // Arrange
            var keyWords = "e-settlements";
            var targetUrl = "www.sympli.com.au";

            var mockResult = new SearchResponseDto()
            {
                Positions = new List<int> { 1, 2, 3 },
            };
            _mockSearchService
                .Setup(s => s.SearchAsync(It.Is<SearchRequestDto>(dto =>
                    dto.Keywords == keyWords &&
                    dto.TargetUrl == targetUrl &&
                    dto.SearchEngine == SearchEngineEnum.Google)))
                .ReturnsAsync(mockResult);
            // Act
            var result = await _controller.Search(keyWords, targetUrl, SearchEngineEnum.Google);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<SearchResponseDto>(okResult.Value);
            Assert.Equal(mockResult.Positions.Count, returnValue.Positions.Count);
        }

        [Fact]
        public async Task Search_ReturnsOkResult_WithSearchResults_Bing()
        {
            // Arrange
            var keyWords = "e-settlements";
            var targetUrl = "www.sympli.com.au";
    
            var mockResult = new SearchResponseDto()
            {
                Positions = new List<int> { 1, 2, 3 },
            };
            _mockSearchService
                .Setup(s => s.SearchAsync(It.Is<SearchRequestDto>(dto =>
                    dto.Keywords == keyWords &&
                    dto.TargetUrl == targetUrl &&
                    dto.SearchEngine == SearchEngineEnum.Bing)))
                .ReturnsAsync(mockResult);
            // Act
            var result = await _controller.Search(keyWords, targetUrl, SearchEngineEnum.Bing);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<SearchResponseDto>(okResult.Value);
            Assert.Equal(mockResult.Positions.Count, returnValue.Positions.Count);
        }

        [Fact]
        public async Task Search_ReturnsBadRequest_WhenKeyWordsIsNullOrEmpty()
        {
            string keyWords = null;
            // Act
            var result = await _controller.Search(keyWords, It.IsAny<string>(), It.IsAny<SearchEngineEnum>());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Search_ReturnsBadRequest_WhenTargetUrlIsNullOrEmpty()
        {
            string targetUrl = null;
            // Act
            var result = await _controller.Search(It.IsAny<string>(), targetUrl, It.IsAny<SearchEngineEnum>());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Search_ReturnsNotFound_WhenResultIsNull()
        {
            var keyWords = "e-settlements";
            var targetUrl = "www.sympli.com.au";
            var searchDto = new SearchRequestDto()
            {
                Keywords = keyWords,
                TargetUrl = targetUrl,
                SearchEngine = It.IsAny<SearchEngineEnum>()
            };
            SearchResponseDto mockResult = null;
            _mockSearchService.Setup(s => s.SearchAsync(searchDto)).ReturnsAsync(mockResult);


            // Act
            var result = await _controller.Search(keyWords, targetUrl, It.IsAny<SearchEngineEnum>());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}