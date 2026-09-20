# I-Do — Todo Management Application

A full-stack portfolio project demonstrating modern web development with .NET Core, React, and cloud deployment.

## 📋 Project Overview

**I-Do** is a task management application built to showcase:
- ASP.NET Core REST API development
- React frontend with modern tooling
- PostgreSQL & SQL Server database integration
- Docker containerization & Docker Compose
- CI/CD with GitHub Actions
- Azure cloud deployment
- SOLID principles & clean code architecture

## 🏗️ Tech Stack

### Backend
- **Framework:** ASP.NET Core 8.0
- **Language:** C#
- **Database:** PostgreSQL (primary) + SQL Server (secondary)
- **ORM:** Entity Framework Core
- **Testing:** xUnit
- **Containerization:** Docker

### Frontend
- **Framework:** React 18
- **HTTP Client:** Axios
- **Styling:** Tailwind CSS
- **State Management:** React Context / Hooks

### DevOps & Cloud
- **Containerization:** Docker & Docker Compose
- **CI/CD:** GitHub Actions
- **Cloud Platform:** Azure (Free Tier)
- **Services:** App Service, Azure Database for PostgreSQL

## 📁 Project Structure

```
I-Do/
├── backend/
│   └── IDotAPI/
│       ├── Controllers/       # API endpoints
│       ├── Models/            # Domain models
│       ├── Data/              # DbContext & migrations
│       ├── Services/          # Business logic
│       ├── Program.cs         # Configuration & startup
│       ├── Dockerfile
│       └── IDotAPI.csproj
├── frontend/
│   ├── src/
│   │   ├── components/        # React components
│   │   ├── pages/             # Page components
│   │   ├── services/          # API services
│   │   ├── App.js
│   │   └── index.js
│   ├── public/
│   ├── Dockerfile
│   └── package.json
├── docker-compose.yml         # Local dev environment
├── .github/
│   └── workflows/             # CI/CD pipelines
├── README.md                  # This file
├── SETUP.md                   # Detailed setup & architecture
└── LICENSE                    # MIT License
```

## 🚀 Quick Start

### Prerequisites
- .NET SDK 8.0+
- Node.js 18+
- Docker Desktop
- Git

### Local Development

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/I-Do.git
   cd I-Do
   ```

2. **Start databases & backend with Docker Compose**
   ```bash
   docker-compose up -d
   ```
   This starts:
   - PostgreSQL on `localhost:5432`
   - SQL Server on `localhost:1433`
   - API will be ready at `http://localhost:5000`

3. **Run the backend (alternative to Docker)**
   ```bash
   cd backend/IDotAPI
   dotnet restore
   dotnet ef database update
   dotnet run
   ```

4. **Run the frontend**
   ```bash
   cd frontend
   npm install
   npm start
   ```
   Frontend runs on `http://localhost:3000`

## 🔧 Architecture & Conventions

### Backend Architecture
- **Layered Architecture:** Controllers → Services → Data Access
- **Design Patterns:** Repository Pattern, Dependency Injection, SOLID Principles
- **Database:** EF Core with Code-First Migrations
- **Error Handling:** Global exception middleware
- **Validation:** Fluent Validation or DataAnnotations

### API Endpoints
All endpoints follow REST conventions:
- `GET /api/todos` — Get all todos
- `POST /api/todos` — Create todo
- `PUT /api/todos/{id}` — Update todo
- `DELETE /api/todos/{id}` — Delete todo

### Frontend Components
- Functional components with Hooks
- API integration via custom hooks
- Tailwind for styling
- Responsive design

### Database
- **PostgreSQL:** Primary database (development & production)
- **SQL Server:** Secondary for learning SQL Server integration
- Migrations managed via EF Core CLI

## 📦 Docker & Containerization

### Build Docker images
```bash
# Backend
docker build -t ido-api:latest ./backend

# Frontend
docker build -t ido-frontend:latest ./frontend
```

### Docker Compose (local dev)
```bash
docker-compose up -d      # Start all services
docker-compose down       # Stop all services
docker-compose logs -f    # View logs
```

## 🧪 Testing

### Backend Tests
```bash
cd backend/IDotAPI
dotnet test
```

### Frontend Tests
```bash
cd frontend
npm test
```

## 🚀 Deployment

### Azure Deployment
1. Create Azure Free Account (https://azure.microsoft.com/free)
2. Create resources:
   - App Service (for API)
   - Azure Database for PostgreSQL
   - Container Registry (optional)
3. Deploy via GitHub Actions or Azure CLI

### GitHub Actions (CI/CD)
The `.github/workflows/` directory contains automated pipelines for:
- Testing on every push
- Building Docker images
- Deploying to Azure

## 📚 Learning Goals

This project demonstrates competency in:
- ✅ .NET API development
- ✅ PostgreSQL & SQL Server databases
- ✅ EF Core ORM & migrations
- ✅ React frontend development
- ✅ Docker & containerization
- ✅ Azure cloud services
- ✅ CI/CD pipelines
- ✅ REST API design
- ✅ Clean code & SOLID principles

## 🛣️ Roadmap

**Phase 1 (Current):** Basic Todo CRUD
- API scaffolding & database setup
- React components & API integration
- Docker containerization
- Basic deploy to Azure

**Phase 2:** Advanced Features
- User authentication (JWT)
- Todo categories
- Due dates & reminders
- Search & filtering

**Phase 3:** Production Polish
- Unit tests (xUnit, Jest)
- Error logging (Serilog)
- API documentation (Swagger)
- Performance optimization

## 📝 License

MIT License — See [LICENSE](LICENSE) for details.

## 👤 Author

Mirkan — Portfolio project for job search (September 2026)

---

**Questions or Issues?**
- Check [SETUP.md](SETUP.md) for detailed setup instructions
- Open an issue on GitHub
- Review API documentation (Swagger/OpenAPI)