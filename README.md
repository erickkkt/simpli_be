# Simpli Search Ranking
A sophisticated search ranking service designed to provide optimized and efficient search results. This project leverages modern software practices and technologies to deliver high-performance search functionalities.

# 🚀 Features
- Highly optimized search ranking algorithms.
- Layered architecture with factory pattern for maintainable and scalable code. we can implement with CQRS, I chose Layered Architecture for this project due to its simplicity and ease of implementation and maintenance..
- Support search on 2 engines: Bing, Google.
- Scraping Google Search results directly with HttpClient is not recommended and generally unreliable due to the following reasons: [Google Cause SEO Outages](https://www.searchenginejournal.com/google-causes-global-seo-tool-outages/537604/)
  <ul>
    <li>Google uses bot detection techniques like CAPTCHA, IP rate limiting, and browser fingerprinting. HttpClient is easily flagged as a bot.</li>
    <li>Google constantly changes its HTML, so your scraper may break frequently.</li>
  </ul>
  
  *** ⚠️ For this project, I used HttpClient to demonstrate technical skills. For production, I recommend using the Google Search API or tools like Selenium WebDriver. I’ve also implemented a separate ChromeScraper portal using Selenium WebDriver (not referenced in this project).***


- Using IHttpClientFactory with Named Client. promoting better resource management and testability [Benefit of using IHttpClientFactory](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#benefits-of-using-ihttpclientfactory)
- Exception Middleware for centralized error handling.
- Unit tests written using xUnit and Moq for mocking dependencies and services.
- Using the options pattern uses classes to provide strongly typed access to groups of related settings.
- Built-in support for Docker and Azure-based deployments.
- Integrated CI/CD pipeline for seamless updates and deployments.
- Real-time monitoring with Azure Application Insights.
  
# 🛠️ Technologies Used
- Programming Language: C# (.NET 8 - Long Term Support)
- Architecture: Layered architecture combined with the Factory Pattern
- Containerization: Docker
- CI/CD Pipeline: Azure Pipelines
- Deployment: Azure Web App Service with Azure Container Registry
- Monitoring: Azure Application Insights
  
# 📦 Installation and Usage Instructions
- Clone the repository:
<pre> ```
bash
git clone https://github.com/erickkkt/simpli_be.git
cd simpli_be
```
</pre>
- Set up the environment:
Ensure you have Docker installed on your system.
<pre>
```
bash
docker compose -f docker-compose.yml build
docker compose -f docker-compose.yml up
```
</pre>

Access the application:
- Azure: https://simpli-search-api.azurewebsites.net/swagger/index.html
- Local: https://localhost:8080/swagger/index.html


# 🏗️ Architecture
This project implements a Layered Architecture for clear separation of concerns:

Presentation Layer: Handles API requests and responses.
Business Logic Layer: Encapsulates the core logic of the application.
Data Access Layer: Interfaces with the database.
Additionally, the Factory Pattern is incorporated to support flexible object creation and dependency injection. This approach ensures maintainability and scalability.
- Diagram:
[PNG](https://gitdiagram.com/erickkkt/simpli_be/)

# 🚀 CI/CD with Azure Pipelines
This project uses Azure Pipelines for continuous integration and deployment:

Build Pipeline: Automatically builds and tests the application.
Release Pipeline: Deploys the application to Azure Web App Service using Azure Container Registry.
The entire process is automated to ensure reliable and seamless deployments.

# 📊 Monitoring with Azure Application Insights
Azure Application Insights is enabled to monitor application performance and usage:

Tracks incoming requests and their response times.
Logs errors and exceptions for debugging.
Provides detailed analytics on application usage.
To view insights, navigate to the Azure Portal and access the Application Insights resource linked to the deployed application.
