<p align="center">
  <a href="https://github.com/yourusername/sigmastats" target="_blank">
<img width="500" height="200" alt="image" src="https://github.com/user-attachments/assets/7f88fc8f-54f3-4380-96c6-e76a8b0f5266" />
  </a>
</p>

<p align="center">
  <a href="https://dotnet.microsoft.com/">
    <img src="https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white" alt=".NET">
  </a>
  <a href="https://dotnet.microsoft.com/apps/aspnet">
    <img src="https://img.shields.io/badge/ASP.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white" alt="ASP.NET">
  </a>
  <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API">
    <img src="https://img.shields.io/badge/Steam_API-000000?style=for-the-badge&logo=steam&logoColor=white" alt="Steam API">
  </a>
  <a href="https://github.com/SteamRE/SteamKit">
    <img src="https://img.shields.io/badge/SteamKit2-1E1E1E?style=for-the-badge&logo=steam&logoColor=white" alt="SteamKit2">
  </a>
  <a href="https://restfulapi.net/">
    <img src="https://img.shields.io/badge/REST_API-FF6C37?style=for-the-badge&logo=rest&logoColor=white" alt="REST API">
  </a>
  <a href="https://opensource.org/licenses/MIT">
    <img src="https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge" alt="License">
  </a>
</p>

# SigmaStats - Steam Game Analytics

A responsive web application for deep analysis of Steam game statistics using official Steam APIs and community tools.

## 🎮 Core Functionality

### 1. Steam Games Data Integration

https://github.com/user-attachments/assets/3dccd34b-920a-4480-b5fe-5e05645bd4e5

- **Comprehensive Game Data**:
  - Retrieve game metadata from Steam Web API
  - Fetch playtime statistics and achievements
  - SteamID resolution and profile parsing
- **Data Processing**:
  - SteamKit2 for authentication (2FA support)
  - SteamDB integration for extended metadata
  - Automatic data refresh scheduling

### 2. User Games Management

https://github.com/user-attachments/assets/2062f0e5-f6de-4086-ba37-99bf51bbe31a

- **Custom Game Tracking**:
  - User-specific game collections
  - Playtime tracking from Steam data
  - Personal ratings system

### 3. Custom Database CRUD

https://github.com/user-attachments/assets/ce792229-f8c4-46c6-ada1-9f5179146b6e

- **Full Data Control**:
  - Create: Add custom game entries
  - Read: Reading data from DB
  - Update: Modify every data you need
  - Delete: Cleanup what you need

## 🛠️ Technical Implementation

```mermaid
graph LR
    A[ASP.NET Frontend] -->|HTTP Requests| B[.NET Core API]
    B -->|REST Calls| C[Steam Web API]
    B -->|Two-Factor Auth| D[SteamKit2]
    D -->|Steam phone App| E[Phone Confirmation]
    B -->|Data Enrichment| F[(SteamDB)]
    B -->|CRUD Operations| G[(Custom Game DB)]
    A -->|Caching| J[(Local Cache)]

  style G fill:#9147FF,color:white
  style F fill:#9147FF,color:white


```
## 🛠️ Technical Stack

### Backend Services
- **ASP.NET Core Web API**
  - RESTful endpoints with JWT authentication
  - Custom action filters for request validation
- **Steam Integration**
  - Steam Web API
  - SteamKit2 for:
    - Two-factor authentication (Steam Guard)
    - Session management

### Frontend Application
- **ASP.NET Core MVC**
  - Razor Pages with ViewComponents
  - Model binding with custom validators
- **Data Visualization**
  - Chart.js for:
    - Playtime trend graphs
    - Achievement progress charts
- **UI Framework**
  - Bootstrap with:
    - Custom Steam-inspired theme
    - Responsive grid layouts

### Development Prerequisites
- **Tools**
  - .NET SDK
  - Visual Studio 2022 (with ASP.NET workload)
  - SQL Server Management Studio
- **Accounts**
  - Steam Developer API Key
  - Steam account with 2FA enabled
- **Configuration**
  - `appsettings.json` requirements:
    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "ConnectionStrings": {
        "SteamContext": "your_path_DB"
      },
      "AllowedHosts": "*",
      "SteamApiKey": "your_steam_api_key"
    }

    ```

## 💻 Installation

### Prerequisites
- .NET SDK
- Steam Developer API Key
- SQL Server (or your DB engine)

```bash
git clone https://github.com/ArtemSheliekhov/aspnet-course-project.git
cd aspnet-course-project
dotnet restore

# Configure appsettings.json with:
# - Steam API keys
# - Database connection strings

dotnet run
```

## 🤝 Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## 📜 License

[MIT](https://choosealicense.com/licenses/mit/)



