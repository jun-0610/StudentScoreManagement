using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudentScoreManagement.Models;
using System.Linq;


namespace StudentScoreManagement.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        ViewBag.StudentCount = _context.Students.Count();
        ViewBag.CourseCount = _context.Courses.Count();
        ViewBag.ScoreCount = _context.Enrollments.Count();

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
