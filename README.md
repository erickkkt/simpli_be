# Simpli Search Ranking
A sophisticated search ranking service designed to provide optimized and efficient search results. This project leverages modern software practices and technologies to deliver high-performance search functionalities.

# 🚀 Features
- Highly optimized search ranking algorithms.
- Layered architecture with factory pattern for maintainable and scalable code. I know CQRS architecture, but I think Layered architecture is suiable for this requirement due to it's simpler and eary for implementation and mainternance.
- Exception Middleware for centralized error handling.
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
PNG

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
