using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Sympli.SearchPortal.Application.Extensions;
using Sympli.SearchPortal.Application.SearchEngine.Interfaces;
using Sympli.SearchPortal.Application.Services.Interfaces;
using Sympli.SearchPortal.Domain.Constants;
using Sympli.SearchPortal.Domain.Enums;
using Sympli.SearchPortal.Domain.Models.Dtos;
using Sympli.SearchPortal.Domain.Models.Settings;

namespace Sympli.SearchPortal.Application.SearchEngine
{
    public class GoogleSearchEngine : ISearchEngine
    {
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cacheService;
        private readonly IOptions<SearchEngineOption> _searchEngineOption;

        public string Name => SearchEngineEnum.Google.GetDescription();
        public GoogleSearchEngine(IHttpClientFactory httpClientFactory, ICacheService cacheService, IOptions<SearchEngineOption> searchEngineOption)
        {
            _httpClient = httpClientFactory.CreateClient(Name);
            _cacheService = cacheService;
            _searchEngineOption = searchEngineOption;
        }

        public async Task<SearchResponseDto> SearchAsync(SearchRequestDto requestSearchDto)
        {
            var cacheKey = $"{Name}_{requestSearchDto.Keywords}_{requestSearchDto.TargetUrl}";
            if (_cacheService.TryGet(cacheKey, out SearchResponseDto cached))
                return cached;

            var url = $"search?q={Uri.EscapeDataString(requestSearchDto.Keywords)}&num={_searchEngineOption.Value.Limit}";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", Constant.DefaultUserAgent);

            var html = await _httpClient.GetStringAsync(url);

            //Using Selenium to scrape the page
            //using var scraper = new ChromeScraperService();
            //var html = scraper.ScrapeHtmlPage($"{_searchEngineOption.Value.GoogleUrl}/{url}");

            string pattern = @"<a[^>]*class=""zReHs""[^>]*href=""([^""]+)""[^>]*>(.*?)<\/a>";

            var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var matches = regex.Matches(html);
            var results = matches.Select(m => m.Groups[1].Value).ToList();

            var positions = results
                .Select((val, idx) => new { Url = val, Position = idx + 1 })
                .Where(x => x.Url.Contains(requestSearchDto.TargetUrl, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Position)
                .ToList();

            var searchResponse = new SearchResponseDto
            {
                Positions = positions
            };

            _cacheService.Set(cacheKey, searchResponse, TimeSpan.FromMinutes(_searchEngineOption.Value.CacheDuration));
            return searchResponse;
        }
    }

}
