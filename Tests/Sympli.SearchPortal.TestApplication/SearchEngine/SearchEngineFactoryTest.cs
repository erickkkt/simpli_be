using Sympli.SearchPortal.Application.SearchEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sympli.SearchPortal.Application.SearchEngine;
using Sympli.SearchPortal.Domain.Models.Dtos;

namespace Sympli.SearchPortal.TestApplication.SearchEngine
{
    public class SearchEngineFactoryTest
    {
        private class MockSearchEngine : ISearchEngine
        {
            public string Name { get; }

            public MockSearchEngine(string name)
            {
                Name = name;
            }

            public Task<SearchResponseDto> SearchAsync(SearchRequestDto searchRequest)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void Constructor_ShouldInitializeEnginesDictionary()
        {
            // Arrange
            var engines = new List<ISearchEngine>
                {
                    new MockSearchEngine("Google"),
                    new MockSearchEngine("Bing")
                };

            // Act
            var factory = new SearchEngineFactory(engines);

            // Assert
            Assert.NotNull(factory);
        }

        [Fact]
        public void GetEngine_ShouldReturnCorrectEngine_WhenEngineExists()
        {
            // Arrange
            var engines = new List<ISearchEngine>
                {
                    new MockSearchEngine("Google"),
                    new MockSearchEngine("Bing")
                };
            var factory = new SearchEngineFactory(engines);

            // Act
            var engine = factory.GetEngine("Google");

            // Assert
            Assert.NotNull(engine);
            Assert.Equal("Google", engine.Name);
        }

        [Fact]
        public void GetEngine_ShouldThrowArgumentException_WhenEngineDoesNotExist()
        {
            // Arrange
            var engines = new List<ISearchEngine>
                {
                    new MockSearchEngine("Google"),
                    new MockSearchEngine("Bing")
                };
            var factory = new SearchEngineFactory(engines);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => factory.GetEngine("Duck"));
            Assert.Equal("Search engine 'Duck' not found.", exception.Message);
        }
    }
}
