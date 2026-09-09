# AndreRosler.AspNetApi

ASP.NET Core Web API with **Tasks CRUD** and **JWT authentication** — portfolio project by André Rösler.

---

## English

### Stack
- ASP.NET Core (`net10.0`)
- Entity Framework Core + SQLite (`app.db`)
- ASP.NET Core Identity + JWT Bearer

### Prerequisites
- [.NET SDK 10+](https://dotnet.microsoft.com/download) (or the SDK that matches the project TFM)

### How to run
```bash
cd AndreRosler.AspNetApi
dotnet restore
dotnet run
```

By default the app listens on the URLs in `Properties/launchSettings.json` (typically `https://localhost:7xxx` / `http://localhost:5xxx`).

On first run, EF Core creates the SQLite database file `app.db` via `EnsureCreated()`.

### Configuration
| Setting | File | Notes |
|---------|------|--------|
| Connection string | `appsettings.json` | `Data Source=app.db` |
| JWT key (placeholder) | `appsettings.json` | **Must be overridden in production** |
| JWT key (dev) | `appsettings.Development.json` | Strong random key for local Development only |

**Production:** do not ship the Development JWT key. Override `Jwt:Key` (and connection string) with environment variables, user secrets, or a secret store:
```bash
dotnet user-secrets set "Jwt:Key" "<long-random-secret>"
# or
setx Jwt__Key "<long-random-secret>"
```

### Endpoints

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| POST | `/api/auth/register` | Public | Register and receive JWT |
| POST | `/api/auth/login` | Public | Login and receive JWT |
| GET | `/api/tasks` | Public | List tasks |
| GET | `/api/tasks/{id}` | Public | Get task by id |
| POST | `/api/tasks` | JWT | Create task |
| PUT | `/api/tasks/{id}` | JWT | Update task |
| DELETE | `/api/tasks/{id}` | JWT | Delete task |

OpenAPI document (Development): `/openapi/v1.json`

### Sample curl

**Register**
```bash
curl -s -X POST https://localhost:7xxx/api/auth/register \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"demo@example.com\",\"password\":\"Secret1\"}" -k
```

**Login**
```bash
curl -s -X POST https://localhost:7xxx/api/auth/login \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"demo@example.com\",\"password\":\"Secret1\"}" -k
```

**Create task (JWT required)** — replace `TOKEN` and port:
```bash
curl -s -X POST https://localhost:7xxx/api/tasks \
  -H "Authorization: Bearer TOKEN" \
  -H "Content-Type: application/json" \
  -d "{\"title\":\"Buy milk\",\"description\":\"2 liters\",\"isDone\":false}" -k
```

**List tasks (public)**
```bash
curl -s https://localhost:7xxx/api/tasks -k
```

**Update task**
```bash
curl -s -X PUT https://localhost:7xxx/api/tasks/1 \
  -H "Authorization: Bearer TOKEN" \
  -H "Content-Type: application/json" \
  -d "{\"title\":\"Buy milk\",\"description\":\"done\",\"isDone\":true}" -k
```

**Delete task**
```bash
curl -s -X DELETE https://localhost:7xxx/api/tasks/1 \
  -H "Authorization: Bearer TOKEN" -k
```

### Entity: Tasks
| Field | Type | Notes |
|-------|------|--------|
| Id | int | PK |
| Title | string | required, max 200 |
| Description | string? | optional, max 2000 |
| IsDone | bool | default false |
| CreatedAt | DateTime | UTC |

---

## Português

### Stack
- ASP.NET Core (`net10.0`)
- Entity Framework Core + SQLite (`app.db`)
- ASP.NET Core Identity + JWT Bearer

### Pré-requisitos
- [.NET SDK 10+](https://dotnet.microsoft.com/download) (ou o SDK compatível com o TFM do projeto)

### Como executar
```bash
cd AndreRosler.AspNetApi
dotnet restore
dotnet run
```

Por padrão a API usa as URLs de `Properties/launchSettings.json` (geralmente `https://localhost:7xxx` / `http://localhost:5xxx`).

Na primeira execução, o EF Core cria o arquivo SQLite `app.db` com `EnsureCreated()`.

### Configuração
| Configuração | Arquivo | Observações |
|--------------|---------|-------------|
| Connection string | `appsettings.json` | `Data Source=app.db` |
| Chave JWT (placeholder) | `appsettings.json` | **Deve ser sobrescrita em produção** |
| Chave JWT (dev) | `appsettings.Development.json` | Chave aleatória forte só para Development |

**Produção:** não use a chave de Development. Sobrescreva `Jwt:Key` (e a connection string) com variáveis de ambiente, user secrets ou um cofre de segredos:
```bash
dotnet user-secrets set "Jwt:Key" "<segredo-longo-aleatorio>"
# ou
setx Jwt__Key "<segredo-longo-aleatorio>"
```

### Endpoints

| Método | Caminho | Auth | Descrição |
|--------|---------|------|-----------|
| POST | `/api/auth/register` | Público | Registrar e receber JWT |
| POST | `/api/auth/login` | Público | Login e receber JWT |
| GET | `/api/tasks` | Público | Listar tarefas |
| GET | `/api/tasks/{id}` | Público | Obter tarefa por id |
| POST | `/api/tasks` | JWT | Criar tarefa |
| PUT | `/api/tasks/{id}` | JWT | Atualizar tarefa |
| DELETE | `/api/tasks/{id}` | JWT | Excluir tarefa |

Documento OpenAPI (Development): `/openapi/v1.json`

### Exemplos curl

**Registrar**
```bash
curl -s -X POST https://localhost:7xxx/api/auth/register \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"demo@example.com\",\"password\":\"Secret1\"}" -k
```

**Login**
```bash
curl -s -X POST https://localhost:7xxx/api/auth/login \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"demo@example.com\",\"password\":\"Secret1\"}" -k
```

**Criar tarefa (JWT obrigatório)** — substitua `TOKEN` e a porta:
```bash
curl -s -X POST https://localhost:7xxx/api/tasks \
  -H "Authorization: Bearer TOKEN" \
  -H "Content-Type: application/json" \
  -d "{\"title\":\"Comprar leite\",\"description\":\"2 litros\",\"isDone\":false}" -k
```

**Listar tarefas (público)**
```bash
curl -s https://localhost:7xxx/api/tasks -k
```

**Atualizar tarefa**
```bash
curl -s -X PUT https://localhost:7xxx/api/tasks/1 \
  -H "Authorization: Bearer TOKEN" \
  -H "Content-Type: application/json" \
  -d "{\"title\":\"Comprar leite\",\"description\":\"feito\",\"isDone\":true}" -k
```

**Excluir tarefa**
```bash
curl -s -X DELETE https://localhost:7xxx/api/tasks/1 \
  -H "Authorization: Bearer TOKEN" -k
```

### Entidade: Tasks
| Campo | Tipo | Observações |
|-------|------|-------------|
| Id | int | PK |
| Title | string | obrigatório, máx. 200 |
| Description | string? | opcional, máx. 2000 |
| IsDone | bool | padrão false |
| CreatedAt | DateTime | UTC |

---

## License
MIT (portfolio / personal use)
