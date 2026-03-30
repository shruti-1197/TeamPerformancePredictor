# 🤖 Team Performance Predictor
### ML-Powered HR Analytics System | C# · ASP.NET Core 8 · ML.NET · SQL Server

---

## 📌 Project Overview

An intelligent HR analytics web application that uses **machine learning** to predict employee productivity scores and detect burnout risk before it becomes a crisis.

Built with:
- **C# / ASP.NET Core 8 MVC** — Backend framework
- **ML.NET 3.0** — Microsoft's ML library (FastTree Regression + SDCA Classification)
- **Entity Framework Core 8** — Database ORM
- **SQL Server Express / LocalDB** — Database
- **JWT Authentication** — Secure login with role-based access
- **Chart.js** — Interactive data visualizations
- **Bootstrap 5** — Responsive frontend

---

## 🤖 Machine Learning Architecture

### Two ML Models Running in Parallel

| Model | Algorithm | Output | Purpose |
|-------|-----------|--------|---------|
| Regression | **FastTree (Gradient Boosting)** | 0–100 score | Predicts productivity score |
| Classification | **SDCA Multi-class** | Low/Medium/High | Predicts burnout risk level |

### 7 Input Features (per employee, averaged over 4 weeks)
1. `HoursWorked` — Total hours per week
2. `TasksCompleted` — Deliverables finished
3. `MeetingsAttended` — Collaboration signal
4. `OvertimeHours` — Key burnout indicator
5. `LeaveDaysTaken` — Absence signal
6. `DeadlinesMissed` — Performance pressure
7. `PeerCollaborationScore` — Peer rating (1–5)

### Training Strategy
- Models train automatically at application startup
- Training data is generated from all historical `ActivityLog` records
- Ground-truth labels computed via business rules to bootstrap the ML
- Falls back to rule-based engine if training data < 10 samples

---

## 🚀 Setup & Run

### Prerequisites
- Visual Studio 2022 (Community or higher)
- .NET 8 SDK
- SQL Server Express or LocalDB (included with Visual Studio)

### Step-by-Step

**1. Clone / open project in Visual Studio**

**2. Install NuGet packages** (Tools → NuGet Package Manager → Package Manager Console):
```powershell
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
Install-Package BCrypt.Net-Next
Install-Package Microsoft.ML
Install-Package Microsoft.ML.FastTree
Install-Package ClosedXML
```

**3. Create and migrate the database:**
```powershell
Add-Migration InitialCreate
Update-Database
```

**4. Press F5 — the app will:**
- Auto-create database with 8 employees & 6 weeks of activity data
- Train both ML models on startup
- Open at `https://localhost:xxxx/Auth/Login`

### Login Credentials
| Role | Username | Password |
|------|----------|----------|
| Admin | `admin` | `Admin@123` |
| Manager | `manager` | `Manager@123` |
| HR | `hr` | `HR@123` |

---

## 📁 Project Structure

```
TeamPerformancePredictor/
│
├── Controllers/
│   ├── AuthController.cs         JWT login/logout
│   ├── DashboardController.cs    Analytics dashboard
│   ├── EmployeeController.cs     Full CRUD + soft delete
│   ├── ActivityLogController.cs  Weekly data entry
│   └── ExportController.cs       Excel report export
│
├── Models/
│   ├── Employee.cs
│   ├── Team.cs
│   ├── ActivityLog.cs            7 ML feature columns
│   ├── PredictionResult.cs       Stores ML output
│   ├── ApplicationUser.cs
│   └── MLModels.cs               ML.NET input/output schemas
│
├── Data/
│   └── AppDbContext.cs           EF Core context + seed data
│
├── Services/
│   └── MLPredictionService.cs    ← Core ML engine
│       ├── TrainModels()         FastTree + SDCA training
│       ├── Predict()             Runs inference
│       ├── PredictWithML()       ML.NET path
│       ├── PredictWithRules()    Fallback path
│       └── GenerateTrainingData() Converts logs → ML inputs
│
├── Views/
│   ├── Auth/Login.cshtml         Split-panel with animated orbs
│   ├── Dashboard/Index.cshtml    KPI cards + 3 Chart.js charts
│   ├── Employee/
│   │   ├── Index.cshtml          Card grid with search/filter
│   │   ├── Details.cshtml        4 trend charts + activity table
│   │   ├── Create.cshtml
│   │   └── Edit.cshtml
│   ├── ActivityLog/
│   │   ├── Index.cshtml
│   │   └── Create.cshtml         ML feature explainer form
│   └── Shared/_Layout.cshtml     Dark sidebar layout
│
├── Program.cs                    Startup + ML training
└── appsettings.json
```

---

## 🔐 Security Features

- **JWT Bearer tokens** stored in HttpOnly cookies (XSS protection)
- **BCrypt password hashing** (never stored as plain text)
- **Role-based authorization**: Admin > Manager > HR
- **Anti-forgery tokens** on all POST forms
- **Soft delete** for employees (data preserved for ML history)

---

## 📊 Features

- ✅ Real ML.NET prediction (FastTree + SDCA) with rule-based fallback
- ✅ Dashboard with 3 Chart.js visualizations
- ✅ Per-employee detail page with 4 trend charts
- ✅ Full employee CRUD with search/filter
- ✅ Weekly activity log data entry with ML feature explanations
- ✅ Excel export (2 sheets: Predictions + Raw Logs)
- ✅ JWT authentication with 3 role levels
- ✅ Responsive design with premium dark sidebar

---

## 🧠 Resume Talking Points

> *"Built an ML-powered HR analytics platform using C# and ASP.NET Core 8 with Microsoft ML.NET. Implemented a FastTree gradient boosting regression model to predict employee productivity (0–100) and an SDCA multi-class classifier to categorize burnout risk. The system ingests 7 weekly activity features per employee, trains at application startup, and falls back to a rule-based engine when data is insufficient. Secured with JWT authentication and role-based access control."*

---

## 📄 Tech Stack Summary (for CV)

`C#` · `ASP.NET Core 8 MVC` · `ML.NET 3.0` · `Entity Framework Core` · `SQL Server` · `JWT` · `BCrypt` · `Chart.js` · `Bootstrap 5` · `ClosedXML`
