using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentScoreManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StudentScoreManagement.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly AppDbContext _context;

        public EnrollmentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Enrollment
        public async Task<IActionResult> Index()
        {
            var enrollments = _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course);
            return View(await enrollments.ToListAsync());
        }

        // GET: Enrollment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var enrollment = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);

            if (enrollment == null) return NotFound();

            return View(enrollment);
        }

        // GET: Enrollment/Create
        public IActionResult Create()
        {
            ViewData["StudentID"] = new SelectList(_context.Students.Select(s => new { s.StudentID, FullName = s.LastName + s.FirstName }), "StudentID", "FullName");
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "CourseName");
            return View();
        }

        // POST: Enrollment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentID,StudentID,CourseID,Semester,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentID"] = new SelectList(_context.Students.Select(s => new { s.StudentID, FullName = s.LastName + s.FirstName }), "StudentID", "FullName", enrollment.StudentID);
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "CourseName", enrollment.CourseID);
            return View(enrollment);
        }

        // GET: Enrollment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null) return NotFound();

            ViewData["StudentID"] = new SelectList(_context.Students.Select(s => new { s.StudentID, FullName = s.LastName + s.FirstName }), "StudentID", "FullName", enrollment.StudentID);
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "CourseName", enrollment.CourseID);
            return View(enrollment);
        }

        // POST: Enrollment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnrollmentID,StudentID,CourseID,Semester,Grade")] Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.EnrollmentID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentID"] = new SelectList(_context.Students.Select(s => new { s.StudentID, FullName = s.LastName + s.FirstName }), "StudentID", "FullName", enrollment.StudentID);
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "CourseName", enrollment.CourseID);
            return View(enrollment);
        }

        // GET: Enrollment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var enrollment = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enrollment == null) return NotFound();

            return View(enrollment);
        }

        // POST: Enrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.EnrollmentID == id);
        }
    }
}
