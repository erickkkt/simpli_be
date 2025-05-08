using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Sympli.SearchPortal.ChromeScraper
{
    public class ChromeScraperService : IDisposable
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private bool _disposed = false;

        public ChromeScraperService()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());

            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-dev-shm-usage");

            _driver = new ChromeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public string ScrapeHtmlPage(string url)
        {
            try
            {
                _driver.Navigate().GoToUrl(url);
                _wait.Until(driver => driver.Title.Length > 0);
                return _driver.PageSource;
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine($"Timeout while loading page: {ex.Message}");
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"Element not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return "Failed to get title";
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _driver.Quit();
                _driver.Dispose();
                _disposed = true;
            }
        }
    }
}
