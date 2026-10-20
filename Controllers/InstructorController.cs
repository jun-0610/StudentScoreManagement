using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentScoreManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StudentScoreManagement.Controllers
{
    public class InstructorController : Controller
    {
        private readonly AppDbContext _context;

        public InstructorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Instructor
        public async Task<IActionResult> Index()
        {
            var instructors = _context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Manager);
            return View(await instructors.ToListAsync());
        }

        // GET: Instructor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var instructor = await _context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Manager)
                .Include(i => i.Subordinates)
                .Include(i => i.Courses)
                .Include(i => i.ProjectsLed)
                .FirstOrDefaultAsync(m => m.InstructorID == id);

            if (instructor == null) return NotFound();

            return View(instructor);
        }

        // GET: Instructor/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentName");
            ViewData["ManagerID"] = new SelectList(_context.Instructors.Select(i => new { i.InstructorID, FullName = i.LastName + i.FirstName }), "InstructorID", "FullName");
            return View();
        }

        // POST: Instructor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructorID,FirstName,LastName,Email,Salary,DepartmentID,ManagerID")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentName", instructor.DepartmentID);
            ViewData["ManagerID"] = new SelectList(_context.Instructors.Select(i => new { i.InstructorID, FullName = i.LastName + i.FirstName }), "InstructorID", "FullName", instructor.ManagerID);
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null) return NotFound();

            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentName", instructor.DepartmentID);
            // Exclude current instructor from manager candidates to prevent self-loop
            var managerCandidates = _context.Instructors
                .Where(i => i.InstructorID != id)
                .Select(i => new { i.InstructorID, FullName = i.LastName + i.FirstName });
            ViewData["ManagerID"] = new SelectList(managerCandidates, "InstructorID", "FullName", instructor.ManagerID);
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstructorID,FirstName,LastName,Email,Salary,DepartmentID,ManagerID")] Instructor instructor)
        {
            if (id != instructor.InstructorID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.InstructorID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentName", instructor.DepartmentID);
            var managerCandidates = _context.Instructors
                .Where(i => i.InstructorID != id)
                .Select(i => new { i.InstructorID, FullName = i.LastName + i.FirstName });
            ViewData["ManagerID"] = new SelectList(managerCandidates, "InstructorID", "FullName", instructor.ManagerID);
            return View(instructor);
        }

        // GET: Instructor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var instructor = await _context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Manager)
                .FirstOrDefaultAsync(m => m.InstructorID == id);
            if (instructor == null) return NotFound();

            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.InstructorID == id);
        }
    }
}
