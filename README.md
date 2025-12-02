# DEV 422 Final Project - Fantasy Sports Team Management System

## Meeting Notes - First Team Meeting
**Date:** 4:45 PM – 3:15 PM  
**Team Members:** Miles Griffith, Peter Troendle, Sean Miles

---

## Role Assignments
- **Sean:** Team Management Service  
- **Griffith:** Player Management Service  
- **Troendle:** Performance Tracking Service

---

## Responsibilities & Deliverables

### Member 1: Team Management Service
**Responsibilities:**
- Implement APIs for creating, updating, and listing teams.
- Integrate with Player Management Service via REST API to fetch player details.
- Manage `Teams` table in Azure SQL.

**Deliverables:**
- Service code + API docs
- Azure deployment URL
- Unit tests for team operations

### Member 2: Player Management Service
**Responsibilities:**
- Implement APIs for player pool management (draft/release).
- Notify Team Management Service when players are drafted/released.
- Manage `Players` table in Azure SQL.

**Deliverables:**
- Service code + API docs
- Azure deployment URL
- Unit tests for player operations

### Member 3: Performance Tracking Service
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

**Note:** All tables use CamelCase for columns and PascalCase for table names.
**Note:** All fonts use Sans Serif 12.

**Note:** TEAMS MEETING 2 - SUNDAY DEC 7 - 4 PM.
