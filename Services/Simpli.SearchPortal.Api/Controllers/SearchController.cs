using Microsoft.AspNetCore.Mvc;
using Sympli.SearchPortal.Application.Services.Interfaces;
using Sympli.SearchPortal.Domain.Enums;
using Sympli.SearchPortal.Domain.Models.Dtos;

namespace Simpli.SearchPortal.Api.Controllers;

[ApiController]
[Route("api/[controller]/v1")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    /// <summary>
    /// Searches for the specified keywords in the given search engine and returns the positions of the target URL.
    /// </summary>
    /// <param name="keywords"></param>
    /// <param name="targetUrl"></param>
    /// <param name="searchEngine"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<SearchResponseDto>> Search([FromQuery] string keywords, [FromQuery] string targetUrl, [FromQuery] SearchEngineEnum searchEngine = SearchEngineEnum.Google)
    {
        if(string.IsNullOrWhiteSpace(keywords))
            return BadRequest("Keywords cannot be empty.");
        
        if (string.IsNullOrWhiteSpace(targetUrl))
            return BadRequest("Target URL cannot be empty.");

        var searchRequest = new SearchRequestDto
        {
            Keywords = keywords,
            TargetUrl = targetUrl,
            SearchEngine = searchEngine
        };

        var result = await _searchService.SearchAsync(searchRequest);

        if (result == null)
            return NotFound("No results found.");
        
        return Ok(result);
    }
}
