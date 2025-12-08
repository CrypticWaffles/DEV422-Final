
-- Optional: create schemas (default is dbo)
-- CREATE SCHEMA Team AUTHORIZATION dbo;
-- CREATE SCHEMA Player AUTHORIZATION dbo;
-- CREATE SCHEMA Perf AUTHORIZATION dbo;

--------------------------------------------
-- Teams (owned by Team Management Service)
--------------------------------------------
IF OBJECT_ID(N'dbo.Teams', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Teams (
        teamId       UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), -- or INT IDENTITY if your teammates chose ints
        teamName     NVARCHAR(100)    NOT NULL,
        createdDate  DATETIME2(0)     NOT NULL DEFAULT SYSUTCDATETIME(),
        CONSTRAINT PK_Teams PRIMARY KEY (teamId)
    );
END;

--------------------------------------------
-- Players (owned by Player Management Service)
--------------------------------------------
IF OBJECT_ID(N'dbo.Players', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Players (
        playerId     UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        playerName   NVARCHAR(100)    NOT NULL,
        position     NVARCHAR(50)     NULL,
        teamId       UNIQUEIDENTIFIER NULL, -- NULL means undrafted; non-NULL means drafted
        CONSTRAINT PK_Players PRIMARY KEY (playerId),
        CONSTRAINT FK_Players_Teams_teamId FOREIGN KEY (teamId) REFERENCES dbo.Teams(teamId)
    );

    -- Helpful index when filtering drafted players
    CREATE INDEX IX_Players_teamId ON dbo.Players(teamId);
END;

--------------------------------------------
-- PerformanceStats (owned by Performance Tracking Service)
--------------------------------------------
IF OBJECT_ID(N'dbo.PerformanceStats', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PerformanceStats (
        statId       UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), -- or INT IDENTITY
        playerId     UNIQUEIDENTIFIER NOT NULL,
        points       INT              NOT NULL,
        assists      INT              NOT NULL,
        rebounds     INT              NOT NULL,
        gameDate     DATETIME2(0)     NOT NULL DEFAULT SYSUTCDATETIME(),
        competitionName NVARCHAR(100) NULL,
        CONSTRAINT PK_PerformanceStats PRIMARY KEY (statId),
        CONSTRAINT FK_PerformanceStats_Players_playerId FOREIGN KEY (playerId) REFERENCES dbo.Players(playerId)
    );

    CREATE INDEX IX_PerformanceStats_Player_Date ON dbo.PerformanceStats(playerId, gameDate DESC);
END;
