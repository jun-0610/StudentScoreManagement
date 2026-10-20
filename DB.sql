IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Courses] (
    [CourseID] int NOT NULL IDENTITY,
    [CourseName] nvarchar(100) NOT NULL,
    [Credits] int NOT NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY ([CourseID])
);
GO

CREATE TABLE [Students] (
    [StudentID] int NOT NULL IDENTITY,
    [StudentName] nvarchar(50) NOT NULL,
    [Gender] nvarchar(10) NOT NULL,
    [Department] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY ([StudentID])
);
GO

CREATE TABLE [Scores] (
    [ScoreID] int NOT NULL IDENTITY,
    [StudentID] int NOT NULL,
    [CourseID] int NOT NULL,
    [Score] int NOT NULL,
    [Semester] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_Scores] PRIMARY KEY ([ScoreID]),
    CONSTRAINT [FK_Scores_Courses_CourseID] FOREIGN KEY ([CourseID]) REFERENCES [Courses] ([CourseID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Scores_Students_StudentID] FOREIGN KEY ([StudentID]) REFERENCES [Students] ([StudentID]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Scores_CourseID] ON [Scores] ([CourseID]);
GO

CREATE INDEX [IX_Scores_StudentID] ON [Scores] ([StudentID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260623151312_InitialCreate', N'8.0.28');
GO

COMMIT;
GO

