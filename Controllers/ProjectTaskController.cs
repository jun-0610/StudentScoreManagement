using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentScoreManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StudentScoreManagement.Controllers
{
    public class ProjectTaskController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectTaskController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ProjectTask
        public async Task<IActionResult> Index()
        {
            var tasks = _context.ProjectTasks.Include(t => t.Project).Include(t => t.AssignedStudent);
            return View(await tasks.ToListAsync());
        }

        // GET: ProjectTask/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.ProjectTasks
                .Include(t => t.Project)
                .Include(t => t.AssignedStudent)
                .FirstOrDefaultAsync(m => m.TaskID == id);

            if (task == null) return NotFound();

            return View(task);
        }

        // GET: ProjectTask/Create
        public IActionResult Create()
        {
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectName");
            ViewData["AssignedStudentID"] = new SelectList(_context.Students.Select(s => new { s.StudentID, FullName = s.LastName + s.FirstName }), "StudentID", "FullName");
            return View();
        }

        // POST: ProjectTask/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskID,TaskName,ProjectID,AssignedStudentID,Status,DueDate")] ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectName", task.ProjectID);
            ViewData["AssignedStudentID"] = new SelectList(_context.Students.Select(s => new { s.StudentID, FullName = s.LastName + s.FirstName }), "StudentID", "FullName", task.AssignedStudentID);
            return View(task);
        }

        // GET: ProjectTask/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.ProjectTasks.FindAsync(id);
            if (task == null) return NotFound();

            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectName", task.ProjectID);
            ViewData["AssignedStudentID"] = new SelectList(_context.Students.Select(s => new { s.StudentID, FullName = s.LastName + s.FirstName }), "StudentID", "FullName", task.AssignedStudentID);
            return View(task);
        }

        // POST: ProjectTask/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskID,TaskName,ProjectID,AssignedStudentID,Status,DueDate")] ProjectTask task)
        {
            if (id != task.TaskID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectTaskExists(task.TaskID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectName", task.ProjectID);
            ViewData["AssignedStudentID"] = new SelectList(_context.Students.Select(s => new { s.StudentID, FullName = s.LastName + s.FirstName }), "StudentID", "FullName", task.AssignedStudentID);
            return View(task);
        }

        // GET: ProjectTask/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.ProjectTasks
                .Include(t => t.Project)
                .Include(t => t.AssignedStudent)
                .FirstOrDefaultAsync(m => m.TaskID == id);
            if (task == null) return NotFound();

            return View(task);
        }

        // POST: ProjectTask/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.ProjectTasks.FindAsync(id);
            if (task != null)
            {
                _context.ProjectTasks.Remove(task);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectTaskExists(int id)
        {
            return _context.ProjectTasks.Any(e => e.TaskID == id);
        }
    }
}
