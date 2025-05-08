using Sympli.SearchPortal.Application.SearchEngine.Interfaces;

namespace Sympli.SearchPortal.Application.SearchEngine
{
    public class SearchEngineFactory : ISearchEngineFactory
    {
        private readonly Dictionary<string, ISearchEngine> _engines;

        public SearchEngineFactory(IEnumerable<ISearchEngine> engines)
        {
            _engines = engines.ToDictionary(e => e.Name, StringComparer.OrdinalIgnoreCase);
        }

        public ISearchEngine GetEngine(string name)
        {
            if (_engines.TryGetValue(name, out var engine))
                return engine;

            throw new ArgumentException($"Search engine '{name}' not found.");
        }
    }

}
