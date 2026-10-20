-- =========================================================================
-- Complex Database Schema for Academic, Research Project & Sponsorship Management
-- Target Database: MS SQL Server (Pure English Version)
-- =========================================================================

-- -------------------------------------------------------------------------
-- [備註] 自動清除既有的外鍵約束與資料表，確保腳本可重複執行 (Idempotent)
-- -------------------------------------------------------------------------
-- Disable and drop all foreign key constraints to prevent dependency errors during drops
DECLARE @drop_fks NVARCHAR(MAX) = N'';
SELECT @drop_fks += N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + N'.' + QUOTENAME(OBJECT_NAME(parent_object_id)) + 
                     N' DROP CONSTRAINT ' + QUOTENAME(name) + N';' + CHAR(13)
FROM sys.foreign_keys;
EXEC sp_executesql @drop_fks;
GO

-- Drop existing tables to ensure a clean slate
IF OBJECT_ID('dbo.ProjectTasks', 'U') IS NOT NULL DROP TABLE dbo.ProjectTasks;
IF OBJECT_ID('dbo.ProjectFundings', 'U') IS NOT NULL DROP TABLE dbo.ProjectFundings;
IF OBJECT_ID('dbo.ProjectMembers', 'U') IS NOT NULL DROP TABLE dbo.ProjectMembers;
IF OBJECT_ID('dbo.Projects', 'U') IS NOT NULL DROP TABLE dbo.Projects;
IF OBJECT_ID('dbo.Enrollments', 'U') IS NOT NULL DROP TABLE dbo.Enrollments;
IF OBJECT_ID('dbo.Courses', 'U') IS NOT NULL DROP TABLE dbo.Courses;
IF OBJECT_ID('dbo.Students', 'U') IS NOT NULL DROP TABLE dbo.Students;
IF OBJECT_ID('dbo.Instructors', 'U') IS NOT NULL DROP TABLE dbo.Instructors;
IF OBJECT_ID('dbo.Departments', 'U') IS NOT NULL DROP TABLE dbo.Departments;
IF OBJECT_ID('dbo.Sponsors', 'U') IS NOT NULL DROP TABLE dbo.Sponsors;
IF OBJECT_ID('dbo.Scores', 'U') IS NOT NULL DROP TABLE dbo.Scores; -- Clean up obsolete scores table
GO

-- ==========================================
-- 2. CREATE TABLES & CONSTRAINTS (DDL)
-- ==========================================

-- -------------------------------------------------------------------------
-- Table 1: Departments (部門資料表 - 階層 Level 1)
-- [業務邏輯] 記錄學院內的系所部門，每個部門有其獨立預算，且預算不可為負數。
-- -------------------------------------------------------------------------
CREATE TABLE dbo.Departments (
    DepartmentID INT IDENTITY(1,1) NOT NULL, -- 部門流水號 (主鍵)
    DepartmentName NVARCHAR(100) NOT NULL,   -- 部門名稱 (唯一值)
    OfficeLocation NVARCHAR(100) NULL,       -- 辦公室位置
    Budget DECIMAL(18,2) NOT NULL DEFAULT 0.00, -- 部門經費預算
    CONSTRAINT PK_Departments PRIMARY KEY (DepartmentID),
    CONSTRAINT UQ_DepartmentName UNIQUE (DepartmentName),
    CONSTRAINT CK_Departments_Budget CHECK (Budget >= 0) -- 限制：預算必須大於或等於 0
);
GO

-- -------------------------------------------------------------------------
-- Table 2: Instructors (教師/教授資料表 - 階層 Level 2，含組織自我關聯)
-- [業務邏輯] 隸屬於特定部門。ManagerID 建立自我關聯（如：系主任管理教授），
--            因 SQL Server 限制，自我關聯不可設 CASCADE 以免造成無窮迴圈。
-- -------------------------------------------------------------------------
CREATE TABLE dbo.Instructors (
    InstructorID INT IDENTITY(1,1) NOT NULL, -- 教師員工編號 (主鍵)
    FirstName NVARCHAR(50) NOT NULL,         -- 名
    LastName NVARCHAR(50) NOT NULL,          -- 姓
    Email NVARCHAR(100) NOT NULL,            -- 電子信箱 (唯一值)
    Salary DECIMAL(18,2) NOT NULL,           -- 薪資
    DepartmentID INT NOT NULL,               -- 所屬部門編號 (外鍵)
    ManagerID INT NULL,                      -- 直屬主管編號 (自我關聯外鍵，允許為空值)
    CONSTRAINT PK_Instructors PRIMARY KEY (InstructorID),
    CONSTRAINT UQ_Instructors_Email UNIQUE (Email),
    CONSTRAINT CK_Instructors_Salary CHECK (Salary >= 0), -- 限制：薪資不可為負數
    -- 當部門被刪除時，連帶刪除該部門的所有教師 (ON DELETE CASCADE)
    CONSTRAINT FK_Instructors_Departments FOREIGN KEY (DepartmentID) REFERENCES dbo.Departments (DepartmentID) ON DELETE CASCADE,
    -- 自我關聯外鍵：不設定級聯刪除，避免遞迴刪除錯誤
    CONSTRAINT FK_Instructors_Instructors_Manager FOREIGN KEY (ManagerID) REFERENCES dbo.Instructors (InstructorID) 
);
GO

-- -------------------------------------------------------------------------
-- Table 3: Students (學生資料表)
-- [業務邏輯] 記錄學生基本資料與入學日期，並強制歸屬於某個主要學系。
-- -------------------------------------------------------------------------
CREATE TABLE dbo.Students (
    StudentID INT IDENTITY(1,1) NOT NULL,    -- 學生學號 (主鍵)
    FirstName NVARCHAR(50) NOT NULL,         -- 名
    LastName NVARCHAR(50) NOT NULL,          -- 姓
    Email NVARCHAR(100) NOT NULL,            -- 電子信箱 (唯一值)
    EnrollmentDate DATE NOT NULL DEFAULT GETDATE(), -- 入學日期 (預設為當天)
    DepartmentID INT NOT NULL,               -- 主修部門編號 (外鍵)
    CONSTRAINT PK_Students PRIMARY KEY (StudentID),
    CONSTRAINT UQ_Students_Email UNIQUE (Email),
    CONSTRAINT FK_Students_Departments FOREIGN KEY (DepartmentID) REFERENCES dbo.Departments (DepartmentID)
);
GO

-- -------------------------------------------------------------------------
-- Table 4: Courses (課程資料表)
-- [業務邏輯] 由特定部門開設、並由特定教授開課。學分數限制在 1~10 之間。
-- -------------------------------------------------------------------------
CREATE TABLE dbo.Courses (
    CourseID INT IDENTITY(1,1) NOT NULL,     -- 課程編號 (主鍵)
    CourseName NVARCHAR(100) NOT NULL,       -- 課程名稱
    Credits INT NOT NULL DEFAULT 3,          -- 學分數 (預設 3 學分)
    DepartmentID INT NOT NULL,               -- 開課部門 (外鍵)
    InstructorID INT NOT NULL,               -- 任課教授 (外鍵)
    CONSTRAINT PK_Courses PRIMARY KEY (CourseID),
    CONSTRAINT CK_Courses_Credits CHECK (Credits > 0 AND Credits <= 10), -- 限制：學分必須在 1 到 10 之間
    CONSTRAINT FK_Courses_Departments FOREIGN KEY (DepartmentID) REFERENCES dbo.Departments (DepartmentID),
    CONSTRAINT FK_Courses_Instructors FOREIGN KEY (InstructorID) REFERENCES dbo.Instructors (InstructorID)
);
GO

-- -------------------------------------------------------------------------
-- Table 5: Enrollments (選課紀錄資料表 - 學生與課程的多對多交叉關聯)
-- [業務邏輯] 記錄學生在某個學期選修某門課的成績。
--            複合唯一約束（UQ）防止同一個學生在同一學期重複選修同一門課。
-- -------------------------------------------------------------------------
CREATE TABLE dbo.Enrollments (
    EnrollmentID INT IDENTITY(1,1) NOT NULL, -- 選課紀錄流水號 (主鍵)
    StudentID INT NOT NULL,                  -- 學生學號 (外鍵)
    CourseID INT NOT NULL,                   -- 課程編號 (外鍵)
    Semester NVARCHAR(20) NOT NULL,          -- 開課學期 (例如 '112-1')
    Grade DECIMAL(5,2) NULL,                 -- 最終成績 (允許先選課後填分數)
    CONSTRAINT PK_Enrollments PRIMARY KEY (EnrollmentID),
    -- 限制聯合唯一：同學生+同課程+同學期 只能有一筆選課紀錄
    CONSTRAINT UQ_Enrollments_Student_Course UNIQUE (StudentID, CourseID, Semester),
    CONSTRAINT CK_Enrollments_Grade CHECK (Grade >= 0.00 AND Grade <= 100.00), -- 限制：分數必須在 0 到 100 之間
    -- 當學生或課程被刪除時，對應的選課紀錄自動連帶刪除 (ON DELETE CASCADE)
    CONSTRAINT FK_Enrollments_Students FOREIGN KEY (StudentID) REFERENCES dbo.Students (StudentID) ON DELETE CASCADE,
    CONSTRAINT FK_Enrollments_Courses FOREIGN KEY (CourseID) REFERENCES dbo.Courses (CourseID) ON DELETE CASCADE
);
GO

-- -------------------------------------------------------------------------
-- Table 6: Projects (研究專案資料表 - 階層 Level 3)
-- [業務邏輯] 由某位教授（LeadInstructor）主導的研究專案，包含專案預算與起訖日。
-- -------------------------------------------------------------------------
CREATE TABLE dbo.Projects (
    ProjectID INT IDENTITY(1,1) NOT NULL,    -- 專案編號 (主鍵)
    ProjectName NVARCHAR(150) NOT NULL,      -- 專案名稱
    Budget DECIMAL(18,2) NOT NULL DEFAULT 0.00, -- 專案總預算
    StartDate DATE NOT NULL,                 -- 執行開始日期
    EndDate DATE NULL,                       -- 預期結束日期 (未結束可為 NULL)
    LeadInstructorID INT NOT NULL,           -- 主持教授編號 (外鍵)
    CONSTRAINT PK_Projects PRIMARY KEY (ProjectID),
    CONSTRAINT CK_Projects_Budget CHECK (Budget >= 0),
    CONSTRAINT CK_Projects_Dates CHECK (EndDate IS NULL OR EndDate >= StartDate), -- 限制：結束日必須大於或等於開始日
    CONSTRAINT FK_Projects_Instructors FOREIGN KEY (LeadInstructorID) REFERENCES dbo.Instructors (InstructorID)
);
GO

-- -------------------------------------------------------------------------
-- Table 7: ProjectMembers (專案成員資料表 - 學生與專案的多對多關聯)
-- [業務邏輯] 記錄哪些學生參與了哪些研究專案、擔任什麼角色、以及每週投入時數。
-- -------------------------------------------------------------------------
CREATE TABLE dbo.ProjectMembers (
    ProjectID INT NOT NULL,                  -- 專案編號 (複合主鍵/外鍵)
    StudentID INT NOT NULL,                  -- 學生學號 (複合主鍵/外鍵)
    Role NVARCHAR(50) NOT NULL DEFAULT 'Research Assistant', -- 成員角色/職稱
    HoursAllocated INT NOT NULL DEFAULT 10,  -- 每週分派配額時數
    CONSTRAINT PK_ProjectMembers PRIMARY KEY (ProjectID, StudentID), -- 建立複合主鍵
    CONSTRAINT CK_ProjectMembers_Hours CHECK (HoursAllocated > 0),    -- 限制：投入時數必須大於 0
    -- 當專案或學生資料被刪，該專案成員配置自動清除
    CONSTRAINT FK_ProjectMembers_Projects FOREIGN KEY (ProjectID) REFERENCES dbo.Projects (ProjectID) ON DELETE CASCADE,
    CONSTRAINT FK_ProjectMembers_Students FOREIGN KEY (StudentID) REFERENCES dbo.Students (StudentID) ON DELETE CASCADE
);
GO

-- -------------------------------------------------------------------------
-- Table 8: ProjectTasks (專案任務資料表 - 階層 Level 4)
-- [業務邏輯] 將專案拆解為具體任務，並指派給參與的學生，追蹤執行狀態（Status）。
-- -------------------------------------------------------------------------
CREATE TABLE dbo.ProjectTasks (
    TaskID INT IDENTITY(1,1) NOT NULL,       -- 任務編號 (主鍵)
    TaskName NVARCHAR(100) NOT NULL,         -- 任務名稱描述
    ProjectID INT NOT NULL,                  -- 所屬專案編號 (外鍵)
    AssignedStudentID INT NOT NULL,          -- 被指派的學生學號 (外鍵)
    Status NVARCHAR(20) NOT NULL DEFAULT 'To Do', -- 任務狀態
    DueDate DATE NULL,                       -- 到期日
    CONSTRAINT PK_ProjectTasks PRIMARY KEY (TaskID),
    CONSTRAINT CK_ProjectTasks_Status CHECK (Status IN ('To Do', 'In Progress', 'Completed')), -- 限制：狀態只能是這三種之一
    CONSTRAINT FK_ProjectTasks_Projects FOREIGN KEY (ProjectID) REFERENCES dbo.Projects (ProjectID) ON DELETE CASCADE,
    CONSTRAINT FK_ProjectTasks_Students FOREIGN KEY (AssignedStudentID) REFERENCES dbo.Students (StudentID)
);
GO

-- -------------------------------------------------------------------------
-- Table 9: Sponsors (資助廠商/外部贊助單位資料表)
-- [業務邏輯] 提供專案研究經費的外部機構或政府單位（例如：國科會、台電）。
-- -------------------------------------------------------------------------
CREATE TABLE dbo.Sponsors (
    SponsorID INT IDENTITY(1,1) NOT NULL,    -- 贊助商編號 (主鍵)
    SponsorName NVARCHAR(100) NOT NULL,      -- 贊助商/機構名稱 (唯一值)
    ContactEmail NVARCHAR(100) NULL,         -- 聯絡電子信箱
    CONSTRAINT PK_Sponsors PRIMARY KEY (SponsorID),
    CONSTRAINT UQ_Sponsors_Name UNIQUE (SponsorName)
);
GO

-- -------------------------------------------------------------------------
-- Table 10: ProjectFundings (專案資助金流水帳資料表)
-- [業務邏輯] 實質媒合「贊助商」與「研究專案」的經費提撥紀錄，一筆專案可接受多筆資助。
-- -------------------------------------------------------------------------
CREATE TABLE dbo.ProjectFundings (
    FundingID INT IDENTITY(1,1) NOT NULL,    -- 資助紀錄編號 (主鍵)
    SponsorID INT NOT NULL,                  -- 贊助商編號 (外鍵)
    ProjectID INT NOT NULL,                  -- 受資助專案編號 (外鍵)
    FundingAmount DECIMAL(18,2) NOT NULL,    -- 資助金額
    FundingDate DATE NOT NULL DEFAULT GETDATE(), -- 撥款日期
    CONSTRAINT PK_ProjectFundings PRIMARY KEY (FundingID),
    CONSTRAINT CK_ProjectFundings_Amount CHECK (FundingAmount > 0), -- 限制：資助金額必須大於 0
    -- 贊助商或專案被刪除時，對應的資助金歷史流水帳一併移除
    CONSTRAINT FK_ProjectFundings_Sponsors FOREIGN KEY (SponsorID) REFERENCES dbo.Sponsors (SponsorID) ON DELETE CASCADE,
    CONSTRAINT FK_ProjectFundings_Projects FOREIGN KEY (ProjectID) REFERENCES dbo.Projects (ProjectID) ON DELETE CASCADE
);
GO

-- -------------------------------------------------------------------------
-- [效能優化] 為所有高頻率用於 JOIN 或 WHERE 條件的外鍵欄位建立非叢集索引 (Non-Clustered Indexes)
-- -------------------------------------------------------------------------
CREATE INDEX IX_Instructors_Dept ON dbo.Instructors(DepartmentID);
CREATE INDEX IX_Students_Dept ON dbo.Students(DepartmentID);
CREATE INDEX IX_Courses_Dept ON dbo.Courses(DepartmentID);
CREATE INDEX IX_Enrollments_Student ON dbo.Enrollments(StudentID);
CREATE INDEX IX_Enrollments_Course ON dbo.Enrollments(CourseID);
CREATE INDEX IX_Projects_Lead ON dbo.Projects(LeadInstructorID);
CREATE INDEX IX_ProjectTasks_Proj ON dbo.ProjectTasks(ProjectID);
CREATE INDEX IX_ProjectFundings_Sponsor ON dbo.ProjectFundings(SponsorID);
GO


-- ==========================================
-- 3. SAMPLE DATA SEEDING (DML)
-- ==========================================

-- 填入部門測試資料
INSERT INTO dbo.Departments (DepartmentName, OfficeLocation, Budget) VALUES
('Computer Science', 'Engineering Hall I 401', 5000000.00),
('Electrical Engineering', 'Engineering Hall II 205', 6500000.00),
('Information Management', 'Management Hall I 302', 3500000.00);

-- 填入教師測試資料 (包含主管階層：主管的 ManagerID 為 NULL，一般教授會指向其主管)
-- 1. 資工系主管
INSERT INTO dbo.Instructors (FirstName, LastName, Email, Salary, DepartmentID, ManagerID) VALUES
('Kuo-Hua', 'Chen', 'chen.kh@univ.edu', 120000.00, 1, NULL);
-- 2. 電機系主管
INSERT INTO dbo.Instructors (FirstName, LastName, Email, Salary, DepartmentID, ManagerID) VALUES
('Chih-Ming', 'Lin', 'lin.cm@univ.edu', 130000.00, 2, NULL);

-- 3. 資工系教授 (主管均為 Chen，即 InstructorID = 1)
INSERT INTO dbo.Instructors (FirstName, LastName, Email, Salary, DepartmentID, ManagerID) VALUES
('Mei-Ling', 'Chang', 'chang.ml@univ.edu', 95000.00, 1, 1),
('Kuan-Yu', 'Wang', 'wang.ky@univ.edu', 88000.00, 1, 1);
-- 4. 電機系教授 (主管為 Lin，即 InstructorID = 2)
INSERT INTO dbo.Instructors (FirstName, LastName, Email, Salary, DepartmentID, ManagerID) VALUES
('Ya-Ting', 'Lee', 'lee.yt@univ.edu', 92000.00, 2, 2);

-- 填入學生測試資料
INSERT INTO dbo.Students (FirstName, LastName, Email, EnrollmentDate, DepartmentID) VALUES
('Chia-Hao', 'Lin', 'lin.jh@student.edu', '2023-09-01', 1),
('Shu-Fen', 'Chen', 'chen.sf@student.edu', '2023-09-01', 1),
('Chun-Chieh', 'Huang', 'huang.cj@student.edu', '2024-09-01', 2),
('Yi-Chun', 'Chang', 'chang.yc@student.edu', '2024-09-01', 2),
('Cheng-En', 'Lee', 'lee.ce@student.edu', '2025-09-01', 3);

-- 填入課程測試資料
INSERT INTO dbo.Courses (CourseName, Credits, DepartmentID, InstructorID) VALUES
('Database Systems', 3, 1, 3), 
('Algorithms', 3, 1, 4),      
('Microprocessors', 4, 2, 5),    
('Computer Organization', 3, 2, 2);   

-- 填入選課紀錄與成績資料 (區分 112 與 113 學期)
INSERT INTO dbo.Enrollments (StudentID, CourseID, Semester, Grade) VALUES
(1, 1, '112-1', 92.50), 
(1, 2, '112-1', 85.00), 
(2, 1, '112-1', 58.00), -- 未及格 (展示 Query 3 算學分時會被過濾)
(2, 2, '112-1', 91.00), 
(3, 3, '113-1', 88.00), 
(3, 4, '113-1', 74.50), 
(4, 3, '113-1', 95.00), 
(4, 4, '113-1', 42.00); 

-- 填入研究專案測試資料
INSERT INTO dbo.Projects (ProjectName, Budget, StartDate, EndDate, LeadInstructorID) VALUES
('Smart IoT Sensing System', 1500000.00, '2025-01-01', '2026-12-31', 3), 
('Next-Gen Green Grid R&D', 2800000.00, '2025-06-01', '2027-05-31', 5), 
('Generative AI Cross-Domain Applications', 900000.00, '2026-02-01', NULL, 4);         

-- 填入專案成員資料 (多對多媒合)
INSERT INTO dbo.ProjectMembers (ProjectID, StudentID, Role, HoursAllocated) VALUES
(1, 1, 'Lead Developer', 20), 
(1, 2, 'Data Analyst', 15),   
(2, 3, 'Hardware Tester', 12), 
(2, 4, 'Firmware Dev', 18),   
(3, 1, 'AI Researcher', 10),  
(3, 2, 'UI Designer', 8);     

-- 填入任務進度追蹤資料
INSERT INTO dbo.ProjectTasks (TaskName, ProjectID, AssignedStudentID, Status, DueDate) VALUES
('IoT node data collection & calibration', 1, 1, 'Completed', '2025-06-30'),
('Cloud dashboard API development', 1, 1, 'In Progress', '2026-03-31'),
('Big data cleaning & feature engineering', 1, 2, 'Completed', '2025-10-31'),
('Grid load simulation testing', 2, 3, 'To Do', '2026-09-30'),
('Microcontroller firmware development', 2, 4, 'Completed', '2025-12-15'),
('Large language model fine-tuning', 3, 1, 'In Progress', '2026-08-31');

-- 填入外部贊助單位測試資料
INSERT INTO dbo.Sponsors (SponsorName, ContactEmail) VALUES
('National Science and Technology Council', 'contact@nstc.gov.tw'),
('Foxconn Technology Group', 'tech.sponsor@foxconn.com'),
('Taiwan Power Company', 'research@taipower.com.tw');

-- 填入贊助撥款紀錄測試資料
INSERT INTO dbo.ProjectFundings (SponsorID, ProjectID, FundingAmount, FundingDate) VALUES
(1, 1, 1000000.00, '2025-01-10'), 
(2, 1, 500000.00, '2025-02-15'),  
(3, 2, 2800000.00, '2025-06-05'), 
(1, 3, 900000.00, '2026-01-20');  
GO


-- ==========================================
-- 4. ADVANCED SQL QUERY EXAMPLES
-- ==========================================

-- -------------------------------------------------------------------------
-- [Query 1] 計算「資訊工程系 (Computer Science)」所有教師的平均薪資與總人數
-- -------------------------------------------------------------------------
SELECT d.DepartmentName, 
       COUNT(i.InstructorID) AS TotalInstructors, -- 該系教師總人數
       AVG(i.Salary) AS AverageSalary             -- 該系平均薪水
FROM dbo.Departments d
INNER JOIN dbo.Instructors i ON d.DepartmentID = i.DepartmentID
WHERE d.DepartmentName = 'Computer Science'
GROUP BY d.DepartmentName;
GO

-- -------------------------------------------------------------------------
-- [Query 2] 找出同時參與「超過一個（複數）」研究專案的優秀學生名單與其所屬學系
-- -------------------------------------------------------------------------
SELECT s.StudentID, 
       s.FirstName + ' ' + s.LastName AS StudentName, -- 合併學生姓名
       d.DepartmentName,
       COUNT(pm.ProjectID) AS ProjectCount            -- 該生參與的專案總數
FROM dbo.Students s
INNER JOIN dbo.ProjectMembers pm ON s.StudentID = pm.StudentID
INNER JOIN dbo.Departments d ON s.DepartmentID = d.DepartmentID
GROUP BY s.StudentID, s.FirstName, s.LastName, d.DepartmentName
HAVING COUNT(pm.ProjectID) > 1; -- 篩選條件：參與數量大於 1
GO

-- -------------------------------------------------------------------------
-- [Query 3] 計算每位學生目前累積「已修過且及格 (成績 >= 60)」的總學分數
-- -------------------------------------------------------------------------
SELECT s.StudentID, 
       s.FirstName + ' ' + s.LastName AS StudentName, 
       COUNT(e.CourseID) AS CoursesTaken, -- 總共選修了幾門課
       -- 使用 CASE WHEN 篩選：只有分數大於等於 60 分才能採計該課學分，否則計為 0 分
       SUM(CASE WHEN e.Grade >= 60.00 THEN c.Credits ELSE 0 END) AS TotalEarnedCredits
FROM dbo.Students s
INNER JOIN dbo.Enrollments e ON s.StudentID = e.StudentID
INNER JOIN dbo.Courses c ON e.CourseID = c.CourseID
GROUP BY s.StudentID, s.FirstName, s.LastName;
GO

-- -------------------------------------------------------------------------
-- [Query 4] 統計各個贊助商的歷史贊助總金額 (使用 LEFT JOIN 確保沒撥過款的廠商也會列出)
-- -------------------------------------------------------------------------
SELECT sp.SponsorID, 
       sp.SponsorName, 
       sp.ContactEmail,
       -- 若完全沒有撥款紀錄（NULL），利用 ISNULL 函式將其自動轉為 0.00 呈現
       ISNULL(SUM(pf.FundingAmount), 0.00) AS TotalSponsorshipAmount
FROM dbo.Sponsors sp
LEFT JOIN dbo.ProjectFundings pf ON sp.SponsorID = pf.SponsorID
GROUP BY sp.SponsorID, sp.SponsorName, sp.ContactEmail;
GO

-- -------------------------------------------------------------------------
-- [Query 5] 列出每門課程中「最高分」與「最低分」的學生姓名與分數
--            (利用相關子查詢 Correlated Subquery 與 TOP 1 進行動態排名過濾)
-- -------------------------------------------------------------------------
SELECT 
    c.CourseID,
    c.CourseName,
    -- 相關子查詢 1：抓出該課程中最高分的學生姓名與分數 (DESC 遞減排序)
    (SELECT TOP 1 s.FirstName + ' ' + s.LastName + ' (' + CAST(e.Grade AS VARCHAR) + ')'
     FROM dbo.Enrollments e
     INNER JOIN dbo.Students s ON e.StudentID = s.StudentID
     WHERE e.CourseID = c.CourseID
     ORDER BY e.Grade DESC, s.StudentID ASC) AS HighestScoringStudent,
    -- 相關子查詢 2：抓出該課程中最低分的學生姓名與分數 (ASC 遞增排序)
    (SELECT TOP 1 s.FirstName + ' ' + s.LastName + ' (' + CAST(e.Grade AS VARCHAR) + ')'
     FROM dbo.Enrollments e
     INNER JOIN dbo.Students s ON e.StudentID = s.StudentID
     WHERE e.CourseID = c.CourseID
     ORDER BY e.Grade ASC, s.StudentID ASC) AS LowestScoringStudent
FROM dbo.Courses c;
GO