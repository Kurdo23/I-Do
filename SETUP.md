# I-Do Setup & Architecture Guide

This document provides detailed technical context for Claude CLI and developers working on the I-Do project.

## 🎯 Project Goals

1. **Portfolio Development** — Build a production-ready full-stack application
2. **Technology Exposure** — Learn .NET, React, PostgreSQL, Docker, Azure
3. **Code Quality** — Follow SOLID principles, clean architecture, maintainability first
4. **CV Value** — Demonstrate REST API design, database work, containerization, cloud deployment

## 📊 Architecture Overview

### Technology Choices & Rationale

| Component | Tech | Rationale |
|-----------|------|-----------|
| **Backend API** | ASP.NET Core 8.0 | Modern, performant, widely used in enterprise |
| **Language** | C# | Strong typing, excellent tooling, SOLID-friendly |
| **Primary DB** | PostgreSQL | Open-source, free tier available, modern features (JSONB) |
| **Secondary DB** | SQL Server | Learn SQL Server for enterprise roles |
| **ORM** | EF Core | Standard .NET ORM, Code-First migrations |
| **Frontend** | React 18 | Industry standard, component-based, good learning |
| **API Client** | Axios | Promise-based, cleaner than fetch |
| **Styling** | Tailwind CSS | Utility-first, rapid development, scalable |
| **Containerization** | Docker | Industry standard, local dev parity, cloud-ready |
| **Orchestration** | Docker Compose | Simple multi-container setup for local dev |
| **Cloud** | Azure | Free tier, native .NET support, good learning |
| **CI/CD** | GitHub Actions | Native GitHub integration, free for public repos |

## 🗂️ Backend Structure (ASP.NET Core)

### Directory Organization

```
backend/IDotAPI/
├── Controllers/          # REST endpoint handlers
│   └── TodoController.cs
├── Models/              # Domain models & DTOs
│   ├── Todo.cs         # Entity
│   ├── Category.cs     # Entity (future)
│   └── CreateTodoDto.cs
├── Data/               # Database layer
│   ├── AppDbContext.cs
│   └── Migrations/
├── Services/           # Business logic
│   └── TodoService.cs
├── Program.cs          # Configuration & dependency injection
├── appsettings.json
├── appsettings.Development.json
├── Dockerfile
└── IDotAPI.csproj
```

### Core Components

#### **Program.cs** (Startup Configuration)
Responsibilities:
- Register services (DbContext, repositories, services)
- Configure middleware (logging, CORS, error handling)
- Setup Swagger/OpenAPI documentation
- Database initialization

Example structure:
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddScoped<ITodoService, TodoService>();

// Configure middleware
var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapControllers();

app.Run();
```

#### **Models Layer**
- **Entities:** Core domain models (Todo, Category)
- **DTOs:** Data Transfer Objects for API requests/responses
- Keep logic minimal; use Services for business rules

#### **Services Layer**
- Encapsulates business logic
- Repository pattern for data access
- Dependency injection via constructor
- Return DTOs, not entities

#### **Controllers Layer**
- Route handlers (endpoints)
- Minimal logic; delegate to Services
- Proper HTTP status codes (200, 201, 400, 404, etc.)
- Input validation via attributes or Fluent Validation

### Database Strategy

#### **EF Core Migrations**
```bash
# Create migration
dotnet ef migrations add AddTodoTable

# Apply migration (creates/updates schema)
dotnet ef database update

# Revert last migration
dotnet ef migrations remove
```

#### **Connection Strings**
Keep in `appsettings.Development.json` (local dev):
```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Server=localhost;Database=ido_db;User Id=postgres;Password=postgres;",
    "SqlServer": "Server=localhost,1433;Database=ido_db;User Id=sa;Password=YourStrongPassword123!;"
  }
}
```

#### **Multiple Databases**
To support both PostgreSQL and SQL Server:
1. Create separate DbContext for each (or conditional configuration)
2. Use environment variables to switch
3. Keep migrations separate per DB type

### API Design Principles

#### **RESTful Endpoints**
```
GET    /api/todos           # List all todos
GET    /api/todos/{id}      # Get single todo
POST   /api/todos           # Create todo
PUT    /api/todos/{id}      # Update todo
DELETE /api/todos/{id}      # Delete todo
```

#### **Status Codes**
- `200 OK` — Successful GET/PUT
- `201 Created` — Successful POST
- `204 No Content` — Successful DELETE
- `400 Bad Request` — Invalid input
- `404 Not Found` — Resource not found
- `500 Internal Server Error` — Server error

#### **Request/Response Format**
```csharp
// POST /api/todos
{
  "title": "Buy groceries",
  "description": "Milk, eggs, bread",
  "dueDate": "2026-09-25"
}

// Response (201 Created)
{
  "id": 1,
  "title": "Buy groceries",
  "description": "Milk, eggs, bread",
  "dueDate": "2026-09-25",
  "completed": false,
  "createdAt": "2026-09-20T12:30:00Z"
}
```

### Testing Strategy

#### **Unit Tests (xUnit)**
Location: `backend/IDotAPI.Tests/`
```bash
dotnet test
```

Test patterns:
- Arrange-Act-Assert
- Mock external dependencies
- Test business logic in Services
- Test edge cases (null, empty, invalid input)

## 🎨 Frontend Structure (React)

### Directory Organization

```
frontend/
├── src/
│   ├── components/       # Reusable React components
│   │   ├── TodoList.jsx
│   │   ├── TodoForm.jsx
│   │   └── TodoItem.jsx
│   ├── pages/            # Page-level components
│   │   └── Home.jsx
│   ├── services/         # API integration
│   │   └── todoService.js
│   ├── hooks/            # Custom React hooks
│   │   └── useTodos.js
│   ├── App.jsx           # Root component
│   ├── App.css
│   └── index.js          # Entry point
├── public/               # Static assets
├── package.json
├── Dockerfile
└── .gitignore
```

### API Integration (Axios)

Create `src/services/todoService.js`:
```javascript
import axios from 'axios';

const API = axios.create({
  baseURL: process.env.REACT_APP_API_URL || 'http://localhost:5000/api'
});

export const todoService = {
  getAll: () => API.get('/todos'),
  getOne: (id) => API.get(`/todos/${id}`),
  create: (data) => API.post('/todos', data),
  update: (id, data) => API.put(`/todos/${id}`, data),
  delete: (id) => API.delete(`/todos/${id}`),
};
```

### Custom Hooks Pattern

Create `src/hooks/useTodos.js`:
```javascript
import { useState, useEffect } from 'react';
import { todoService } from '../services/todoService';

export function useTodos() {
  const [todos, setTodos] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const fetchTodos = async () => {
    setLoading(true);
    try {
      const response = await todoService.getAll();
      setTodos(response.data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTodos();
  }, []);

  return { todos, loading, error, refetch: fetchTodos };
}
```

### Component Structure

Functional components with hooks:
```jsx
export function TodoList() {
  const { todos, loading, error, refetch } = useTodos();
  const [newTitle, setNewTitle] = useState('');

  const handleAdd = async () => {
    await todoService.create({ title: newTitle });
    refetch();
    setNewTitle('');
  };

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div>
      <input 
        value={newTitle} 
        onChange={(e) => setNewTitle(e.target.value)} 
      />
      <button onClick={handleAdd}>Add Todo</button>
      {todos.map(todo => <TodoItem key={todo.id} todo={todo} />)}
    </div>
  );
}
```

### Environment Variables

Create `.env` in frontend root:
```
REACT_APP_API_URL=http://localhost:5000/api
REACT_APP_ENV=development
```

Access in React:
```javascript
const apiUrl = process.env.REACT_APP_API_URL;
```

## 🐳 Docker & Docker Compose

### Dockerfile — Backend

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["IDotAPI.csproj", "."]
RUN dotnet restore "IDotAPI.csproj"
COPY . .
RUN dotnet build "IDotAPI.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/build .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "IDotAPI.dll"]
```

### Dockerfile — Frontend

```dockerfile
FROM node:18-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/build /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

### Docker Compose Configuration

`docker-compose.yml` at root:
```yaml
version: '3.8'

services:
  postgres:
    image: postgres:15
    environment:
      POSTGRES_DB: ido_db
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      SA_PASSWORD: "YourStrongPassword123!"
      ACCEPT_EULA: "Y"
    ports:
      - "1433:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql

  api:
    build:
      context: ./backend/IDotAPI
    ports:
      - "5000:5000"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__PostgreSQL=Server=postgres;Database=ido_db;User Id=postgres;Password=postgres;
    depends_on:
      - postgres
      - sqlserver

  frontend:
    build:
      context: ./frontend
    ports:
      - "3000:3000"
    depends_on:
      - api

volumes:
  postgres_data:
  sqlserver_data:
```

### Commands

```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f api

# Stop services
docker-compose down

# Rebuild images
docker-compose up -d --build
```

## ☁️ Azure Deployment

### Free Tier Resources Needed

1. **Azure Database for PostgreSQL** — `postgres-ido.postgres.database.azure.com`
2. **App Service** — `ido-api.azurewebsites.net`
3. **Azure Container Registry** (optional) — `idoregistry.azurecr.io`

### Deployment Flow

```
Local Code → Git Push → GitHub Actions → Build Docker Image → Push to ACR → Deploy to App Service
```

### GitHub Actions Workflow

Create `.github/workflows/deploy.yml`:
```yaml
name: Deploy to Azure

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Build Docker image
        run: docker build -t idoapi:${{ github.sha }} ./backend
      
      - name: Push to Azure Container Registry
        run: |
          docker tag idoapi:${{ github.sha }} ${{ secrets.REGISTRY_URL }}/idoapi:latest
          docker push ${{ secrets.REGISTRY_URL }}/idoapi:latest
      
      - name: Deploy to App Service
        uses: azure/webapps-deploy@v2
        with:
          app-name: 'ido-api'
          images: '${{ secrets.REGISTRY_URL }}/idoapi:latest'
```

## 🧪 Development Workflow

### Local Development Cycle

1. **Start services:**
   ```bash
   docker-compose up -d
   ```

2. **Backend development:**
   ```bash
   cd backend/IDotAPI
   dotnet watch run  # Auto-rebuild on file changes
   ```

3. **Frontend development:**
   ```bash
   cd frontend
   npm start  # Auto-reload on file changes
   ```

4. **Test API:**
   ```bash
   curl http://localhost:5000/api/todos
   # or use Postman / REST Client
   ```

5. **Commit changes:**
   ```bash
   git add .
   git commit -m "feat: add todo create endpoint"
   git push
   ```

### Code Quality Standards

- **Naming:** Descriptive, PascalCase (C#), camelCase (JS)
- **Comments:** Explain "why" not "what"
- **Functions:** Single responsibility, under 20 lines
- **Error Handling:** Explicit try-catch, meaningful messages
- **Testing:** Unit tests for business logic, >70% coverage goal
- **Git:** Meaningful commit messages, one feature per commit

### Common Claude CLI Commands

```bash
# Scaffolding
claude "create docker-compose.yml for postgres and sql server"

# Implementation
claude "implement Todo CRUD endpoints in TodoController.cs"

# Debugging
claude "debug this error: [error message]"

# Documentation
claude "generate swagger/openapi documentation for TodoController"

# Optimization
claude "optimize this SQL query: [query]"
```

## 🚀 Next Steps

1. **Setup databases** with docker-compose
2. **Create Todo model** and EF Core DbContext
3. **Add API endpoints** (CRUD operations)
4. **Setup React components** for todo list UI
5. **Connect frontend to API** with Axios
6. **Test locally** with Docker Compose
7. **Deploy to Azure** Free Tier
8. **Setup CI/CD** with GitHub Actions

---

**Questions?** Refer to README.md for quick start or API documentation (Swagger).