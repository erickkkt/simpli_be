using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Sympli.SearchPortal.Application.Extensions;
using Sympli.SearchPortal.Application.SearchEngine.Interfaces;
using Sympli.SearchPortal.Application.Services.Interfaces;
using Sympli.SearchPortal.Domain.Enums;
using Sympli.SearchPortal.Domain.Models.Dtos;
using Sympli.SearchPortal.Domain.Models.Settings;
using Sympli.SearchPortal.Domain.Constants;

namespace Sympli.SearchPortal.Application.SearchEngine
{   
    public class BingSearchEngine : ISearchEngine
    {
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cacheService;
        private readonly IOptions<SearchEngineOption> _searchEngineOption;

        public string Name => SearchEngineEnum.Bing.GetDescription();
        public BingSearchEngine(IHttpClientFactory httpClientFactory, ICacheService cacheService, IOptions<SearchEngineOption> searchEngineOption)
        {
            _httpClient = httpClientFactory.CreateClient(Name);
            _cacheService = cacheService;
            _searchEngineOption = searchEngineOption;
        }

        public async Task<SearchResponseDto> SearchAsync(SearchRequestDto request)
        {
            var cacheKey = $"{Name}_{request.Keywords}_{request.TargetUrl}";
            if (_cacheService.TryGet(cacheKey, out SearchResponseDto cached))
                return cached;

            var positions = new List<int>();
            var totalPages = _searchEngineOption.Value.Limit / Constant.BingResultPerPage;

            for (var page = 0; page < totalPages; page++)
            {
                var offset = page * Constant.BingResultPerPage + 1;
                
                var url = $"search?q={Uri.EscapeDataString(request.Keywords)}&first={offset}";

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("User-Agent", Constant.DefaultUserAgent);

                var html = await _httpClient.GetStringAsync(url);

                var liTagPattern = @"<li class=""b_algo.*?"">(.*?)<\/li>";
                var liTagRegex = new Regex(liTagPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                var liTagMatches = liTagRegex.Matches(html);
                var results = liTagMatches.Select(m => m.Groups[1].Value).ToList();

                var liPositions = results
                    .Select((val, idx) => new { Url = val, Position = idx + 1 })
                    .Where(x => x.Url.Contains(request.TargetUrl, StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.Position)
                    .ToList();
                positions.AddRange(liPositions.Select(p => p + offset - 1));
            }

            var searchResponse = new SearchResponseDto
            {
                Positions = positions
            };

            _cacheService.Set(cacheKey, searchResponse, TimeSpan.FromMinutes(_searchEngineOption.Value.CacheDuration));
            return searchResponse;
        }
    }

}
