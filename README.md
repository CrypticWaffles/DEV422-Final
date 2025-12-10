
**Note on next Meeting:** DEC 10 – Meeting 6 PM 
– Authentification back to ENTRA (Bellevue college credentials)
– All IP's should be alowed - will confirm the connection strings - DEC 10 morning
– All Postman Test - Endpoints
– All Index
– Making the 5 min Video

# DEV 422 Final Project - Fantasy Sports Team Management System

---

## Meeting Minutes - Third Team Meeting
**Date:** Dec 10, 4:00 PM – 4:15 PM  
**Team Members:** Miles Griffith, Peter Troendle, Sean Miles

– Authentification failed
– IP's of other Teams members blocked
– All Postman Test failed to connect
– SQL Azure Database is facing Authentification issues / Plan : revert to ENTRA Authentif.
---

## Meeting Minutes - Second Team Meeting
**Date:** Dec 7, 4:00 PM – 4:15 PM  
**Team Members:** Miles Griffith, Peter Troendle, Sean Miles

1. Team project pushed TeamManagementService
2. Player Project pushed PlayerManagementService
3. Integration top
4. Performance is now using the data 1 and 2 provide (Integration and Statistics)
- DEC 8: Providing Azure SQL secure access using connection strings stored in Azure App Service settings

---

## Meeting Minutes - First Team Meeting
**Date:** Dec 4, 4:45 PM – 5:15 PM  
**Team Members:** Miles Griffith, Peter Troendle, Sean Miles

---

## Role Assignments
- **Sean:** Team Management Service  
- **Griffith:** Player Management Service  
- **Troendle:** Performance Tracking Service

---

## Responsibilities & Deliverables

### Sean: Team Management Service
**Responsibilities:**
- Implement APIs for creating, updating, and listing teams.
- Integrate with Player Management Service via REST API to fetch player details.
- Manage `Teams` table in Azure SQL.

**Deliverables:**
- Service code + API docs
- Azure deployment URL
- Unit tests for team operations

### Griffith: Player Management Service
**Responsibilities:**
- Implement APIs for player pool management (draft/release).
- Notify Team Management Service when players are drafted/released.
- Manage `Players` table in Azure SQL.

**Deliverables:**
- Service code + API docs
- Azure deployment URL
- Unit tests for player operations

### Troendle: Performance Tracking Service
**Responsibilities:**
- Simulate competitions and update player stats.
- Notify Leaderboard Service (if implemented).
- Manage `PerformanceStats` table in Azure SQL.

**Deliverables:**
- Service code + API docs
- Azure deployment URL
- Unit tests for performance updates

--- 
## API Endpoints

**port mapping based on the launchSettings.json files you shared for each service:**

**Team Management Service**

- HTTP: http://localhost:5014
- HTTPS: https://localhost:7076
- IIS Express (optional): http://localhost:15754 (SSL: 44398)


**Player Management Service**

- HTTP: http://localhost:5275
- HTTPS: https://localhost:7063
- IIS Express (optional): http://localhost:22980 (SSL: 44325)


**Performance Tracking Service**

- HTTP: http://localhost:19700
- HTTPS: https://localhost:19699

--- 



### Team Management Service
- `GET /api/teams` → List all teams
- `GET /api/graph` → Graph
- `GET /api/teams/{id}` → Get team by ID
- `POST /api/teams` → Create a new team
- `PUT /api/teams/{id}` → Update team details
- `DELETE /api/teams/{id}` → Delete a team

### Player Management Service
- `GET /api/players` → List all players
- `GET /api/graph` → Graph
- `GET /api/players/{id}` → Get player details
- `POST /api/players/draft` → Draft a player to a team
- `POST /api/players/release` → Release a player from a team

### Performance Tracking Service
- `GET /api/performance` → Get all performance stats
- `GET /api/performance/{playerId}` → Get stats for a player
- `POST /api/performance/simulate` → Simulate competition and update stats

---

## Database Schema (CamelCase Naming)

### Tables and Columns

#### Teams Table
- `teamId` (Primary Key)
- `teamName`
- `createdDate`

#### Players Table
- `playerId` (Primary Key)
- `playerName`
- `position`
- `teamId` (Foreign Key)

#### PerformanceStats Table
- `statId` (Primary Key)
- `playerId` (Foreign Key)
- `points`
- `assists`
- `rebounds`
- `gameDate`
- `competitionName`

---

## Azure SQL Database & Connection String (App Service)

**Server**: `fantasysports-sqlsrv.database.windows.net`  
**Database**: `DEV422FantasySportsDB`  
**Region**: West US 2  
**Auth**: Microsoft Entra (server admin set) or SQL auth (admin user)  
**Firewall**: Client IP allowed; Azure services access enabled for App Services.

### Added connection string in Azure Portal
1. In **App Service → Settings → Environment variables → Connection strings**.
2. **+ Add**:
   - **Name**: `conn`
   - **Type**: `SQLAzure`
   - **Value** (Passwords and Username are private and dont belong in Github as its public):
     ```
     Server=tcp:fantasysports-sqlsrv.database.windows.net,1433;
     Initial Catalog=DEV422FantasySportsDB;
     Persist Security Info=False;
     User ID=<your-sql-user>;  <<<<<<<< Private& Confidential
     Password=<your-strong-password>;   <<<<<<< Private & Confidential
     MultipleActiveResultSets=False;
     Encrypt=True;
     TrustServerCertificate=False;
     Connection Timeout=30;
     ```

> At runtime, App Service exposes this connection string to your app configuration and as environment variables (e.g., `conn`). Reading it via `Configuration.GetConnectionString("<Name>")` is the usual approach in ASP.NET Core.

### Use in C\# code
// Program.cs
var conn = builder.Configuration.GetConnectionString("conn");
builder.Services.AddDbContext<PerformanceContext>(opt => opt.UseSqlServer(conn));
