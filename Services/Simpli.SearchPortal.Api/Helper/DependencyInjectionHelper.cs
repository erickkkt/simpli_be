
using Microsoft.Extensions.Options;
using Sympli.SearchPortal.Application.Extensions;
using Sympli.SearchPortal.Application.SearchEngine.Interfaces;
using Sympli.SearchPortal.Application.SearchEngine;
using Sympli.SearchPortal.Application.Services;
using Sympli.SearchPortal.Application.Services.Interfaces;
using Sympli.SearchPortal.Domain.Enums;
using Sympli.SearchPortal.Domain.Models.Settings;

namespace Simpli.SearchPortal.Api.Helper
{
    /// <summary>
    /// This class is used to help with dependency injection in the application.
    /// </summary>
    public static class DependencyInjectionHelper
    {
        /// <summary>
        /// AddBaseSettings method
        /// </summary>
        public static void AddBaseSettings(this IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            // Base API Configuration Access
            services.Configure<SearchEngineOption>(configurationRoot.GetSection("SearchEngine"));
        }

        /// <summary>
        /// AddServices method
        /// </summary>
        public static void AddServices(this IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            services.AddMemoryCache();

            //Add HttpClients
            services.AddHttpClient(SearchEngineEnum.Google.GetDescription(), (provider, client) =>
            {
                var searchEngineOption = provider.GetRequiredService<IOptions<SearchEngineOption>>().Value;

                var endpoint = searchEngineOption.GoogleUrl + "/";
                client.BaseAddress = new Uri(endpoint);
                client.Timeout = TimeSpan.FromSeconds(searchEngineOption.RequestTimeout);
            });

            services.AddHttpClient(SearchEngineEnum.Bing.GetDescription(), (provider, client) =>
            {
                var searchEngineOption = provider.GetRequiredService<IOptions<SearchEngineOption>>().Value;

                var endpoint = searchEngineOption.BingUrl + "/";
                client.BaseAddress = new Uri(endpoint);
                client.Timeout = TimeSpan.FromSeconds(searchEngineOption.RequestTimeout);
            });

            services.AddSingleton<ICacheService, CacheService>();

            services.AddScoped<ISearchEngineFactory, SearchEngineFactory>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<ISearchEngine, GoogleSearchEngine>();
            services.AddScoped<ISearchEngine, BingSearchEngine>();
        }
    }
}
