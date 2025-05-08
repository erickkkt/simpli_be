
namespace Sympli.SearchPortal.Application.SearchEngine.Interfaces
{
    public interface ISearchEngineFactory
    {
        ISearchEngine GetEngine(string name);
    }

}
