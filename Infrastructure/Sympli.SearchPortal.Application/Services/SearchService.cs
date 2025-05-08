using Sympli.SearchPortal.Application.Extensions;
using Sympli.SearchPortal.Application.SearchEngine.Interfaces;
using Sympli.SearchPortal.Application.Services.Interfaces;
using Sympli.SearchPortal.Domain.Models.Dtos;

namespace Sympli.SearchPortal.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISearchEngineFactory _engineFactory;
        public SearchService(ISearchEngineFactory engineFactory) 
        {
            _engineFactory = engineFactory;
        }
        public async Task<SearchResponseDto> SearchAsync(SearchRequestDto searchRequest)
        {
            var engine = _engineFactory.GetEngine(searchRequest.SearchEngine.GetDescription());
            var result = await engine.SearchAsync(searchRequest);
            return result;
        }
    }
}
