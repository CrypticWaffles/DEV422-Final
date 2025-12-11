

CREATE USER [sean.miles@bellevuecollege.edu] FROM EXTERNAL PROVIDER;
CREATE USER [miles.griffith@bellevuecollege.edu] FROM EXTERNAL PROVIDER;
CREATE USER [usman.rizvi@bellevuecollege.edu] FROM EXTERNAL PROVIDER;

ALTER ROLE db_datareader ADD MEMBER [sean.miles@bellevuecollege.edu];
ALTER ROLE db_datawriter ADD MEMBER [sean.miles@bellevuecollege.edu];

ALTER ROLE db_datareader ADD MEMBER [miles.griffith@bellevuecollege.edu];
ALTER ROLE db_datawriter ADD MEMBER [miles.griffith@bellevuecollege.edu];

ALTER ROLE db_datareader ADD MEMBER [usman.rizvi@bellevuecollege.edu];
ALTER ROLE db_datawriter ADD MEMBER [usman.rizvi@bellevuecollege.edu];


-- RUN THIS IN YOUR APPLICATION DATABASE (not master)
-- e.g., MyAppDb
CREATE USER [myappuser] WITH PASSWORD = 'Strong!Passw0rdGoesHere';
ALTER ROLE db_datareader ADD MEMBER [myappuser];
ALTER ROLE db_datawriter ADD MEMBER [myappuser];
-- If migrations need schema changes:
ALTER ROLE db_ddladmin  ADD MEMBER [myappuser];
-- Or full ownership (be careful; broad privileges):
-- EXEC sp_addrolemember N'db_owner', N'myappuser';

--------------------------------------------
-- Teams (owned by Team Management Service)
--------------------------------------------
IF OBJECT_ID(N'dbo.Teams', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Teams (
        teamId       UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
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
        teamId       UNIQUEIDENTIFIER NULL,  -- NULL = undrafted
        CONSTRAINT PK_Players PRIMARY KEY (playerId),
        CONSTRAINT FK_Players_Teams_teamId FOREIGN KEY (teamId) REFERENCES dbo.Teams(teamId)
    );

    CREATE INDEX IX_Players_teamId ON dbo.Players(teamId);
END;

--------------------------------------------
-- PerformanceStats (owned by Performance Tracking Service)
--------------------------------------------
IF OBJECT_ID(N'dbo.PerformanceStats', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PerformanceStats (
        statId          UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        playerId        UNIQUEIDENTIFIER NOT NULL,
        points          INT              NOT NULL,
        assists         INT              NOT NULL,
        rebounds        INT              NOT NULL,
        gameDate        DATETIME2(0)     NOT NULL DEFAULT SYSUTCDATETIME(),
        competitionName NVARCHAR(100)    NULL,
        CONSTRAINT PK_PerformanceStats PRIMARY KEY (statId),
        CONSTRAINT FK_PerformanceStats_Players_playerId FOREIGN KEY (playerId) REFERENCES dbo.Players(playerId)
    );

    CREATE INDEX IX_Perf_Player_Date ON dbo.PerformanceStats(playerId, gameDate DESC);
END;
=======

--------------------------------------------
-- Teams (owned by Team Management Service)
--------------------------------------------
IF OBJECT_ID(N'dbo.Teams', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Teams (
        teamId       UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
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
        teamId       UNIQUEIDENTIFIER NULL,  -- NULL = undrafted
        CONSTRAINT PK_Players PRIMARY KEY (playerId),
        CONSTRAINT FK_Players_Teams_teamId FOREIGN KEY (teamId) REFERENCES dbo.Teams(teamId)
    );

    CREATE INDEX IX_Players_teamId ON dbo.Players(teamId);
END;

--------------------------------------------
-- PerformanceStats (owned by Performance Tracking Service)
--------------------------------------------
IF OBJECT_ID(N'dbo.PerformanceStats', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PerformanceStats (
        statId          UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        playerId        UNIQUEIDENTIFIER NOT NULL,
        points          INT              NOT NULL,
        assists         INT              NOT NULL,
        rebounds        INT              NOT NULL,
        gameDate        DATETIME2(0)     NOT NULL DEFAULT SYSUTCDATETIME(),
        competitionName NVARCHAR(100)    NULL,
        CONSTRAINT PK_PerformanceStats PRIMARY KEY (statId),
        CONSTRAINT FK_PerformanceStats_Players_playerId FOREIGN KEY (playerId) REFERENCES dbo.Players(playerId)
    );

    CREATE INDEX IX_Perf_Player_Date ON dbo.PerformanceStats(playerId, gameDate DESC);
END;

--------------------------------------------------------
-- SEEDS FOR TEST
--------------------------------------------------------

-- Create one test team
INSERT INTO dbo.Teams (teamName) VALUES (N'Test Team A'); 
-- Capture the teamId GUID
SELECT teamId, teamName, createdDate FROM dbo.Teams WHERE teamName = N'Test Team A';

-- Create two undrafted players
INSERT INTO dbo.Players (playerName, position) VALUES (N'Alice Adams', N'Forward');
INSERT INTO dbo.Players (playerName, position) VALUES (N'Bob Baker', N'Guard');
-- Capture their GUIDs


