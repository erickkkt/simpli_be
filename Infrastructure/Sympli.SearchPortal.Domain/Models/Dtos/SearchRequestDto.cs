
using Sympli.SearchPortal.Domain.Enums;

namespace Sympli.SearchPortal.Domain.Models.Dtos
{
    public class SearchRequestDto
    {
        public string Keywords { get; set; }
        public string TargetUrl { get; set; }
        public SearchEngineEnum SearchEngine { get; set; } = SearchEngineEnum.Google;

    }
}
