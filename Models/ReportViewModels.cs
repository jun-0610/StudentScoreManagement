using System.Collections.Generic;

namespace StudentScoreManagement.Models
{
    // Original or updated score reports (if needed)
    public class ScoreReportViewModel
    {
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public decimal Score { get; set; }
        public string Semester { get; set; }
    }

    public class StatisticsViewModel
    {
        public double AverageScore { get; set; }
        public double MaxScore { get; set; }
        public double MinScore { get; set; }
        public List<StudentAverageViewModel> StudentAverages { get; set; }
    }

    public class StudentAverageViewModel
    {
        public string StudentName { get; set; }
        public double Average { get; set; }
    }

    // View models for the 5 advanced SQL queries
    public class DeptAvgSalaryViewModel
    {
        public string DepartmentName { get; set; }
        public int TotalInstructors { get; set; }
        public decimal AverageSalary { get; set; }
    }

    public class StudentMultipleProjectsViewModel
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string DepartmentName { get; set; }
        public int ProjectCount { get; set; }
    }

    public class StudentCreditsViewModel
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int CoursesTaken { get; set; }
        public int TotalEarnedCredits { get; set; }
    }

    public class SponsorFundingViewModel
    {
        public int SponsorID { get; set; }
        public string SponsorName { get; set; }
        public string? ContactEmail { get; set; }
        public decimal TotalFunding { get; set; }
    }

    public class CourseScorerViewModel
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string HighestScoringStudent { get; set; }
        public string LowestScoringStudent { get; set; }
    }

    // Combined advanced queries report model
    public class AdvancedQueriesReportViewModel
    {
        public List<DeptAvgSalaryViewModel> DeptAvgSalaries { get; set; } = new();
        public List<StudentMultipleProjectsViewModel> MultipleProjectsStudents { get; set; } = new();
        public List<StudentCreditsViewModel> StudentCredits { get; set; } = new();
        public List<SponsorFundingViewModel> SponsorFundings { get; set; } = new();
        public List<CourseScorerViewModel> CourseScorers { get; set; } = new();
    }
}
