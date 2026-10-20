using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentScoreManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StudentScoreManagement.Controllers
{
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Project
        public async Task<IActionResult> Index()
        {
            var projects = _context.Projects.Include(p => p.LeadInstructor);
            return View(await projects.ToListAsync());
        }

        // GET: Project/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var project = await _context.Projects
                .Include(p => p.LeadInstructor)
                .Include(p => p.ProjectTasks)
                .ThenInclude(t => t.AssignedStudent)
                .Include(p => p.ProjectMembers)
                .ThenInclude(pm => pm.Student)
                .Include(p => p.ProjectFundings)
                .ThenInclude(pf => pf.Sponsor)
                .FirstOrDefaultAsync(m => m.ProjectID == id);

            if (project == null) return NotFound();

            // Setup view bag options for adding members or funding quickly
            ViewData["StudentID"] = new SelectList(_context.Students.Select(s => new { s.StudentID, FullName = s.LastName + s.FirstName }), "StudentID", "FullName");
            return View(project);
        }

        // GET: Project/Create
        public IActionResult Create()
        {
            ViewData["LeadInstructorID"] = new SelectList(_context.Instructors.Select(i => new { i.InstructorID, FullName = i.LastName + i.FirstName }), "InstructorID", "FullName");
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectID,ProjectName,Budget,StartDate,EndDate,LeadInstructorID")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LeadInstructorID"] = new SelectList(_context.Instructors.Select(i => new { i.InstructorID, FullName = i.LastName + i.FirstName }), "InstructorID", "FullName", project.LeadInstructorID);
            return View(project);
        }

        // GET: Project/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();

            ViewData["LeadInstructorID"] = new SelectList(_context.Instructors.Select(i => new { i.InstructorID, FullName = i.LastName + i.FirstName }), "InstructorID", "FullName", project.LeadInstructorID);
            return View(project);
        }

        // POST: Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectID,ProjectName,Budget,StartDate,EndDate,LeadInstructorID")] Project project)
        {
            if (id != project.ProjectID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LeadInstructorID"] = new SelectList(_context.Instructors.Select(i => new { i.InstructorID, FullName = i.LastName + i.FirstName }), "InstructorID", "FullName", project.LeadInstructorID);
            return View(project);
        }

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var project = await _context.Projects
                .Include(p => p.LeadInstructor)
                .FirstOrDefaultAsync(m => m.ProjectID == id);
            if (project == null) return NotFound();

            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Project/AddMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(int projectId, int studentId, string role, int hoursAllocated)
        {
            var memberExists = await _context.ProjectMembers.AnyAsync(pm => pm.ProjectID == projectId && pm.StudentID == studentId);
            if (!memberExists)
            {
                var member = new ProjectMember
                {
                    ProjectID = projectId,
                    StudentID = studentId,
                    Role = role,
                    HoursAllocated = hoursAllocated
                };
                _context.ProjectMembers.Add(member);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Details), new { id = projectId });
        }

        // POST: Project/RemoveMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(int projectId, int studentId)
        {
            var member = await _context.ProjectMembers.FirstOrDefaultAsync(pm => pm.ProjectID == projectId && pm.StudentID == studentId);
            if (member != null)
            {
                _context.ProjectMembers.Remove(member);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Details), new { id = projectId });
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectID == id);
        }
    }
}
