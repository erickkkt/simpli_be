
using Sympli.SearchPortal.Domain.Models.Dtos;

namespace Sympli.SearchPortal.Application.Services.Interfaces
{
    public interface ISearchService
    {
        Task<SearchResponseDto> SearchAsync(SearchRequestDto searchRequest);
    }
}
