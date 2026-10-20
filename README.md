# ProjectScoreHub

智慧生產專案管理資料庫系統

## 專案簡介

ProjectScoreHub 為資料庫管理課程個人專案，以「智慧生產專案管理」為情境，設計並實作一套整合學術管理、專案管理、選課成績管理、客戶與贊助管理及報表查詢功能的資訊系統。

本專案從需求分析開始，規劃完整的關聯式資料庫架構，建立資料字典（Data Dictionary）、資料表關聯以及資料完整性約束，並使用 SQL Server 建立資料庫，搭配 ASP.NET Core MVC 開發系統操作介面。

專案重點在於資料庫分析與設計，而非單純的網頁介面開發，希望透過實際系統建置過程，培養資料建模、資料管理及資訊系統開發能力。

---

## 專案目標

- 建立完整關聯式資料庫系統
- 規劃資料表關聯與資料完整性
- 實作資料字典（Data Dictionary）
- 練習 SQL Server 資料庫設計
- 熟悉 ASP.NET Core MVC 開發流程
- 建立資料新增、查詢、修改與刪除功能（CRUD）

---

## 開發環境

### Backend

- ASP.NET Core MVC
- C#
- Entity Framework Core

### Database

- SQL Server
- SQL

### Frontend

- HTML5
- CSS3
- Bootstrap
- JavaScript

### Development Tools

- Visual Studio 2022
- SQL Server Management Studio (SSMS)
- Git
- GitHub

---

## 系統功能

### 行政學術管理

- 科系資料管理（Departments）
- 教師資料管理（Instructors）
- 學生資料管理（Students）
- 課程資料管理（Courses）

### 選課與成績管理

- 學生選課管理
- 成績紀錄管理
- 修課資料查詢
- 成績統計與分析

### 專案與任務管理

- 專案建立與維護
- 任務分派與管理
- 專案進度追蹤

### 客戶與贊助管理

- 客戶資料管理
- 贊助資料管理
- 專案合作紀錄管理

### 報表與查詢

- 條件式資料查詢
- 歷史資料檢視
- 系統資料統計報表

---

## 資料庫設計

本系統共設計 10 個核心資料表，透過主鍵（Primary Key）與外鍵（Foreign Key）建立資料關聯。

### 核心資料表

1. Departments（科系部門表）
2. Instructors（教師主管表）
3. Students（學生基本資料表）
4. Courses（課程表）
5. Enrollments（學生選課與評分表）
6. Projects（專案表）
7. Tasks（任務表）
8. Clients（客戶表）
9. Sponsors（贊助表）
10. Reports（報表資料表）

---

## 資料表關聯特色

- 一對多（One-to-Many）關聯設計
- 多對多（Many-to-Many）關聯設計
- 主鍵與外鍵約束
- 參照完整性（Referential Integrity）
- 資料驗證規則（Validation Rules）
- CHECK Constraint 檢查條件

### 關聯範例

```text
Students
    │
    │ 1:N
    ▼
Enrollments
    ▲
    │ N:1
    │
Courses
```

學生可以修習多門課程，而一門課程也可由多位學生修習，因此透過 Enrollments 建立多對多關聯。

---

## Data Dictionary

專案建立完整資料字典，包含：

- 欄位名稱
- 資料型態
- 欄位長度
- 主鍵設定
- 外鍵設定
- Null限制
- 資料說明
- 約束條件

範例：

### Enrollments

| 欄位名稱 | 型態 | 說明 |
|----------|------|------|
| EnrollmentID | INT | 選課流水號 |
| StudentID | INT | 學生編號 |
| CourseID | INT | 課程編號 |
| Semester | NVARCHAR(20) | 修課學期 |
| Grade | DECIMAL(5,2) | 成績 |

---

## 專案展示

### Demo 影片

系統操作展示影片：

🔗 https://yzu365-my.sharepoint.com/:v:/g/personal/s1131708_mail_yzu_edu_tw/IQA7trf5j8sQTbrM_9FY8AiqAZfqfGRB5db8OIkshum9IYg

影片內容包含：

- 系統架構介紹
- 資料字典展示
- 科系管理功能
- 學生管理功能
- 課程管理功能
- 選課與成績管理
- 專案與任務管理
- 報表查詢功能

---

## 個人負責內容

本專案由本人獨立完成：

- 系統需求分析
- 資料庫規劃與設計
- ERD建模
- Data Dictionary建置
- SQL資料庫建立
- 主鍵與外鍵關聯設計
- Entity Framework Core 實作
- ASP.NET Core MVC 開發
- CRUD功能實作
- 系統測試與除錯

---

## 專案收穫

透過本專案，我學習如何從需求分析開始規劃資料庫架構，並建立完整的關聯式資料庫系統。

在開發過程中，我實際運用了資料表正規化、主外鍵關聯設計、資料完整性控制以及 SQL 查詢語法，進一步理解資料庫管理的核心概念。同時也透過 ASP.NET Core MVC 建立系統介面，將資料庫理論與實務開發結合，提升系統分析與問題解決能力。

---

## 專案成果

- 完成 10 張核心資料表設計
- 建立完整 Data Dictionary
- 實作資料表關聯與約束條件
- 完成 CRUD 資料管理功能
- 建立資料查詢與報表功能
- 完成 ASP.NET Core MVC 系統開發

---

## Developer

梁文心

元智大學 資訊管理學系

Database Management Course Project
