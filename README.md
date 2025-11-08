# MichaelKim.Hello.v2

New and improved Full-Stack Portfolio Web Application built with React, .NET Aspire, and PostgreSQL. 
- **Descriptions Stored in SQL Database:** \
Details can now be updated without re-deployment.

- **Unified Endpoints:** \
Values are grouped and exposed by tables, instead of individual values.
- **Automated Deployment:** \
Building and Deploying are now handled via Github Actions.
- **Automated Testing:** \
Tests are now unified and run before deployment.
-  **Monitoring and Logging:** \
Telemetry data and logs are saved to an Azure's Application Insight service.

**Live Website:** [MichaelKim.Hello.v2](https://black-pond-08086d30f.3.azurestaticapps.net/)

## 🚀 Features
- 🔄 **Automated Project Showcase:** \
Dynamically scrapes and syncs data from my GitHub pinned repositories to automatically populate featured projects. The Gtihub API does not provide access to pinned repositories.
- 🗄️ **Database-Driven Content Management:** \
Stores and manages repository metadata (titles, descriptions, links, emails, etc.) in a PostgreSQL database, allowing content updates without redeploying the apiservice.
- ⚙️ **Full-Stack Integration:** \
.NET backend provides structured APIs for the React frontend to fetch and display data seamlessly.
- ☁️ **Cloud Hosting & Monitoring:** \
Fully hosted on Microsoft Azure and SupaBase (Postgresql Database), featuring integrated Azure Application Insights for observability and performance monitoring.
- 🔁 **CI/CD Pipeline:** \
Continuous integration and deployment implemented with GitHub Actions, enabling automated builds, tests, and deployments on push.
- 🧪 **Automated Testing:** \
Comprehensive backend testing implemented with xUnit, integrated into the CI/CD pipeline to maintain code reliability.
- 🧩 **Modular Architecture:** \
Built with maintainability and scalability in mind, leveraging .NET Aspire for service orchestration.

## TBD
- Configuring more xUnit tests.

## 🔧 Tech Stack

- **Backend**: .NET Aspire, Postgresql, xUnit
- **Frontend**: React (Next.js with TypeScript)
- **Styling**: Tailwind CSS (nim-template)
- **Hosting**: Microsoft Azure (App Service, Application Insights), Supabase (Postgresql)
- **CI/CD & DevOps**: GitHub Actions • Azure Monitor
- **Dev Tools**: C#, Typescript, SQL, Docker

- **NOTE**: may break with certain browser extensions
