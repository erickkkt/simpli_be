
using Microsoft.Extensions.Options;
using Moq;
using Sympli.SearchPortal.Application.SearchEngine;
using Sympli.SearchPortal.Application.Services.Interfaces;
using Sympli.SearchPortal.Domain.Enums;
using Sympli.SearchPortal.Domain.Models.Dtos;
using Sympli.SearchPortal.Domain.Models.Settings;
using System.Net;


namespace Sympli.SearchPortal.TestApplication.SearchEngine
{
    public class BingSearchEngineTest
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<IOptions<SearchEngineOption>> _optionsMock;
        private readonly BingSearchEngine _bingSearchEngine;

        public BingSearchEngineTest()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _cacheServiceMock = new Mock<ICacheService>();
            _optionsMock = new Mock<IOptions<SearchEngineOption>>();

            var httpClient = new HttpClient(new MockHttpMessageHandler())
            {
                BaseAddress = new Uri("https://www.bing.com")
            };
            _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _optionsMock.Setup(o => o.Value).Returns(new SearchEngineOption
            {
                Limit = 50,
                CacheDuration = 10
            });

            _bingSearchEngine = new BingSearchEngine(_httpClientFactoryMock.Object, _cacheServiceMock.Object,
                _optionsMock.Object);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnCachedResponse_WhenCacheExists()
        {
            // Arrange
            var request = new SearchRequestDto
            { Keywords = "test", TargetUrl = "example.com", SearchEngine = SearchEngineEnum.Bing };
            var cachedResponse = new SearchResponseDto { Positions = new List<int> { 1, 2, 3 } };

            _cacheServiceMock.Setup(c => c.TryGet(It.IsAny<string>(), out cachedResponse)).Returns(true);

            // Act
            var result = await _bingSearchEngine.SearchAsync(request);

            // Assert
            Assert.Equal(cachedResponse.Positions, result.Positions);
            _cacheServiceMock.Verify(c => c.TryGet(It.IsAny<string>(), out cachedResponse), Times.Once);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnResultFromBingSearchEngine_WhenNoCacheExists()
        {
            // Arrange
            var request = new SearchRequestDto
            {
                Keywords = "test",
                TargetUrl = "example.com",
                SearchEngine = SearchEngineEnum.Bing
            };

            var cacheKey = $"Bing_{request.Keywords}_{request.TargetUrl}";
            SearchResponseDto? cachedResponse = null;

            _cacheServiceMock.Setup(c => c.TryGet(cacheKey, out cachedResponse)).Returns(false);

            // Act
            var result = await _bingSearchEngine.SearchAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Contains(1, result.Positions);
            _cacheServiceMock.Verify(c => c.Set(cacheKey, It.IsAny<SearchResponseDto>(), It.IsAny<TimeSpan>()),
                Times.Once);
        }

        private class MockHttpMessageHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var responseContent = @"
                        <html>
                            <body>
                                <li class=""b_algo result"">
                                  <h2><a href=""https://example.com"">Example</a></h2>
                                  <p>Description here</p>
                                </li>
                            </body>
                        </html>";

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(responseContent)
                };

                return Task.FromResult(response);
            }
        }
    }
}
