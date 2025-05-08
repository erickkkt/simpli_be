
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Sympli.SearchPortal.Application.Services;

namespace Sympli.SearchPortal.TestApplication.Services
{
    public class CacheServiceTest
    {
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly CacheService _cacheService;

        public CacheServiceTest()
        {
            _memoryCacheMock = new Mock<IMemoryCache>();
            _cacheService = new CacheService(_memoryCacheMock.Object);
        }

        [Fact]
        public void Set_ShouldStoreValueInCache()
        {
            // Arrange
            var key = "testKey";
            var value = "testValue";
            var duration = TimeSpan.FromMinutes(5);

            var cacheEntryMock = new Mock<ICacheEntry>();
            _memoryCacheMock
                .Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                .Returns(cacheEntryMock.Object);

            // Act
            _cacheService.Set(key, value, duration);

            // Assert
            _memoryCacheMock.Verify(cache => cache.CreateEntry(key), Times.Once);
            cacheEntryMock.VerifySet(entry => entry.AbsoluteExpirationRelativeToNow = duration, Times.Once);
        }

        [Fact]
        public void TryGet_ShouldReturnTrueAndValue_WhenKeyExists()
        {
            // Arrange
            var key = "testKey";
            var expectedValue = "testValue";
            object outValue = expectedValue;

            _memoryCacheMock
                .Setup(cache => cache.TryGetValue(key, out outValue))
                .Returns(true);

            // Act
            var result = _cacheService.TryGet(key, out string? actualValue);

            // Assert
            Assert.True(result);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void TryGet_ShouldReturnFalse_WhenKeyDoesNotExist()
        {
            // Arrange
            var key = "nonExistentKey";
            object outValue = null;

            _memoryCacheMock
                .Setup(cache => cache.TryGetValue(key, out outValue))
                .Returns(false);

            // Act
            var result = _cacheService.TryGet(key, out string? actualValue);

            // Assert
            Assert.False(result);
            Assert.Null(actualValue);
        }
    }
}
