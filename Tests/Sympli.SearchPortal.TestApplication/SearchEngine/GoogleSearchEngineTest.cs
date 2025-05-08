using System.Net;
using Microsoft.Extensions.Options;
using Moq;
using Sympli.SearchPortal.Application.SearchEngine;
using Sympli.SearchPortal.Application.Services.Interfaces;
using Sympli.SearchPortal.Domain.Enums;
using Sympli.SearchPortal.Domain.Models.Dtos;
using Sympli.SearchPortal.Domain.Models.Settings;

namespace Sympli.SearchPortal.TestApplication.SearchEngine
{
    public class GoogleSearchEngineTest
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<IOptions<SearchEngineOption>> _optionsMock;
        private readonly GoogleSearchEngine _googleSearchEngine;

        public GoogleSearchEngineTest()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _cacheServiceMock = new Mock<ICacheService>();
            _optionsMock = new Mock<IOptions<SearchEngineOption>>();

            var httpClient = new HttpClient(new MockHttpMessageHandler())
            {
                BaseAddress = new Uri("https://www.google.com")
            };
            _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _optionsMock.Setup(o => o.Value).Returns(new SearchEngineOption
            {
                Limit = 10,
                CacheDuration = 5
            });

            _googleSearchEngine = new GoogleSearchEngine(_httpClientFactoryMock.Object, _cacheServiceMock.Object, _optionsMock.Object);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnCachedResult_WhenCacheExists()
        {
            // Arrange
            var requestDto = new SearchRequestDto { Keywords = "test", TargetUrl = "example.com", SearchEngine = SearchEngineEnum.Google};
            var cachedResponse = new SearchResponseDto { Positions = new List<int> { 1, 2 } };

            _cacheServiceMock.Setup(c => c.TryGet(It.IsAny<string>(), out cachedResponse)).Returns(true);

            // Act
            var result = await _googleSearchEngine.SearchAsync(requestDto);

            // Assert
            Assert.Equal(cachedResponse.Positions, result.Positions);
            _cacheServiceMock.Verify(c => c.TryGet(It.IsAny<string>(), out cachedResponse), Times.Once);
        }

        [Fact]
        public async Task SearchAsync_ShouldFetchAndCacheResult_WhenCacheDoesNotExist()
        {
            // Arrange
            var requestDto = new SearchRequestDto { Keywords = "test", TargetUrl = "example.com", SearchEngine = SearchEngineEnum.Google };
            var cacheKey = $"Google_test_example.com";
            SearchResponseDto? cachedResponse = null;

            _cacheServiceMock.Setup(c => c.TryGet(It.IsAny<string>(), out cachedResponse)).Returns(false);

            // Act
            var result = await _googleSearchEngine.SearchAsync(requestDto);

            // Assert
            Assert.NotNull(result.Positions);
            _cacheServiceMock.Verify(c => c.Set(cacheKey, It.IsAny<SearchResponseDto>(), It.IsAny<TimeSpan>()), Times.Once);
        }

        private class MockHttpMessageHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("<a href=\"/url?q=http://example.com&\">Example</a>")
                };
                return Task.FromResult(response);
            }
        }
    }
}
