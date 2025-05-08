using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.SearchPortal.Domain.Models.Settings
{
    public class SearchEngineOption
    {
        public string GoogleUrl { get; set; }

        public string BingUrl { get; set; }

        public int Limit { get; set; }

        public int CacheDuration { get; set; }
        public int RequestTimeout { get; set; }
    }
}
