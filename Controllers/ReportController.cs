using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentScoreManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StudentScoreManagement.Controllers
{
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        // 1. 查詢功能 (Query) - 依學生姓名查詢
        public async Task<IActionResult> Search(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var students = _context.Students.Include(s => s.Department).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => EF.Functions.Like(s.LastName + s.FirstName, $"%{searchString}%"));
            }

            return View(await students.ToListAsync());
        }

        // 2. Aggregate / Statistics功能 - 舊統計頁面更新
        public async Task<IActionResult> Statistics()
        {
            var enrollments = await _context.Enrollments.ToListAsync();
            
            if (enrollments.Count == 0 || !enrollments.Any(e => e.Grade.HasValue))
            {
                return View(new StatisticsViewModel 
                { 
                    StudentAverages = new System.Collections.Generic.List<StudentAverageViewModel>() 
                });
            }

            var activeGrades = enrollments.Where(e => e.Grade.HasValue).Select(e => (double)e.Grade!.Value).ToList();

            var model = new StatisticsViewModel
            {
                AverageScore = activeGrades.Average(),
                MaxScore = activeGrades.Max(),
                MinScore = activeGrades.Min(),
            };

            var studentAvgQuery = from e in _context.Enrollments
                                  where e.Grade.HasValue
                                  join st in _context.Students on e.StudentID equals st.StudentID
                                  group e by st.LastName + st.FirstName into g
                                  select new StudentAverageViewModel
                                  {
                                      StudentName = g.Key,
                                      Average = g.Average(x => (double)x.Grade!.Value)
                                  };

            model.StudentAverages = await studentAvgQuery.ToListAsync();

            return View(model);
        }

        // 3. 進階 SQL 查詢範例報表 (5 Advanced Queries)
        public async Task<IActionResult> AdvancedQueries()
        {
            var viewModel = new AdvancedQueriesReportViewModel();

            // Query 1: 查詢某部門員工(教師)的平均薪資
            viewModel.DeptAvgSalaries = await _context.Departments
                .Select(d => new DeptAvgSalaryViewModel
                {
                    DepartmentName = d.DepartmentName,
                    TotalInstructors = d.Instructors.Count(),
                    AverageSalary = d.Instructors.Any() ? d.Instructors.Average(i => i.Salary) : 0
                }).ToListAsync();

            // Query 2: 找出同時參與多個專案的員工 (此處為學生)
            viewModel.MultipleProjectsStudents = await _context.Students
                .Where(s => s.ProjectMembers.Count() > 1)
                .Select(s => new StudentMultipleProjectsViewModel
                {
                    StudentID = s.StudentID,
                    StudentName = s.LastName + s.FirstName,
                    DepartmentName = s.Department.DepartmentName,
                    ProjectCount = s.ProjectMembers.Count()
                }).ToListAsync();

            // Query 3: 計算每位學生修課的總學分 (成績 >= 60.00 及格學分)
            viewModel.StudentCredits = await _context.Students
                .Select(s => new StudentCreditsViewModel
                {
                    StudentID = s.StudentID,
                    StudentName = s.LastName + s.FirstName,
                    CoursesTaken = s.Enrollments.Count(),
                    TotalEarnedCredits = s.Enrollments
                        .Where(e => e.Grade >= 60)
                        .Sum(e => e.Course.Credits)
                }).ToListAsync();

            // Query 4: 查詢某客戶(贊助商)的訂單(贊助)總金額
            viewModel.SponsorFundings = await _context.Sponsors
                .Select(sp => new SponsorFundingViewModel
                {
                    SponsorID = sp.SponsorID,
                    SponsorName = sp.SponsorName,
                    ContactEmail = sp.ContactEmail,
                    TotalFunding = sp.ProjectFundings.Sum(pf => pf.FundingAmount)
                }).ToListAsync();

            // Query 5: 找出課程中最高分與最低分的學生
            viewModel.CourseScorers = await _context.Courses
                .Select(c => new CourseScorerViewModel
                {
                    CourseID = c.CourseID,
                    CourseName = c.CourseName,
                    HighestScoringStudent = c.Enrollments.Where(e => e.Grade.HasValue)
                        .OrderByDescending(e => e.Grade)
                        .Select(e => e.Student.LastName + e.Student.FirstName + " (" + e.Grade + "分)")
                        .FirstOrDefault() ?? "尚無成績",
                    LowestScoringStudent = c.Enrollments.Where(e => e.Grade.HasValue)
                        .OrderBy(e => e.Grade)
                        .Select(e => e.Student.LastName + e.Student.FirstName + " (" + e.Grade + "分)")
                        .FirstOrDefault() ?? "尚無成績"
                }).ToListAsync();

            return View(viewModel);
        }

        // 4. Data Dictionary頁面
        public IActionResult DataDictionary()
        {
            return View();
        }
    }
}
