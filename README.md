
**Note on next Meeting:**  DEC 9    Meeting  4PM  Making the 5 min Video
 
# DEV 422 Final Project - Fantasy Sports Team Management System


## Meeting Minutes - Second Team Meeting
**Date:** Dec 7 4 PM – 4:15 PM  
**Team Members:** Miles Griffith, Peter Troendle, Sean Miles

1. Team project pushed TeamManagement     **should (TASK FOR SEAN)** be >>>>>>>>>TeamManagementService
2. Player Project pushed PlayerManagementService
3. Integration top
4. Performance is now using the data 1. and 2. provide (Integration and Statistics) 
- DEC 8 I am Providing the Azure SQL Secure access using connection strings stored in Azure App Service settings
  
 


## Meeting Minutes - First Team Meeting
**Date:** 4:45 PM – 3:15 PM  
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

---
#### Azure SQL Database Deployment
- Method: .NET/C# project with REST services and Azure SQL tables (Teams, Players, PerformanceStats)
- Connection Strings: ADO.NET (SQL authentication)
- Server=tcp:fantasysports-sqlsrv.database.windows.net,1433;
Initial Catalog=DEV422FantasySportsDB;
Persist Security Info=False;
User ID=sqladmin;
Password=SQLadmin2015;
MultipleActiveResultSets=False;
Encrypt=True;
TrustServerCertificate=False;
Connection Timeout=30;

- Database Name: DEV422FantasySportsDB
- Server Name: fantasysports-sqlsrv.database.windows.net
- Status: Online 
- Subscription: Azure for Students 
- Region: West US 2 

**Note on Form:** All tables use CamelCase for columns and PascalCase for table names. All fonts use Sans Serif 12.

---
**GIT - HOW TO ADD CHANGES ON LOCAL COPY TO THE GITHUB TEAMS WORK**

  - 927  git clone https://github.com/CrypticWaffles/DEV422-Final.git
  - 933  cd DEV422-Final\
  - 934  git remote -v
  - 935  git add .
  - 936  git status
  - 937  git push origin HEAD
  - 938  git commit -m "Update README (point 4)"
  - 939  git push origin main

    
