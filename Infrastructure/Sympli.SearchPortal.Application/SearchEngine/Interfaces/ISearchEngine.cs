using Sympli.SearchPortal.Domain.Models.Dtos;

namespace Sympli.SearchPortal.Application.SearchEngine.Interfaces
{
    public interface ISearchEngine
    {
        string Name { get; }
        Task<SearchResponseDto> SearchAsync(SearchRequestDto searchRequest);
    }
}
