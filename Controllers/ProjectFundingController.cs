using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentScoreManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StudentScoreManagement.Controllers
{
    public class ProjectFundingController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectFundingController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ProjectFunding
        public async Task<IActionResult> Index()
        {
            var projectFundings = _context.ProjectFundings.Include(p => p.Project).Include(p => p.Sponsor);
            return View(await projectFundings.ToListAsync());
        }

        // GET: ProjectFunding/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var projectFunding = await _context.ProjectFundings
                .Include(p => p.Project)
                .Include(p => p.Sponsor)
                .FirstOrDefaultAsync(m => m.FundingID == id);

            if (projectFunding == null) return NotFound();

            return View(projectFunding);
        }

        // GET: ProjectFunding/Create
        public IActionResult Create()
        {
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectName");
            ViewData["SponsorID"] = new SelectList(_context.Sponsors, "SponsorID", "SponsorName");
            return View();
        }

        // POST: ProjectFunding/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FundingID,SponsorID,ProjectID,FundingAmount,FundingDate")] ProjectFunding projectFunding)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectFunding);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectName", projectFunding.ProjectID);
            ViewData["SponsorID"] = new SelectList(_context.Sponsors, "SponsorID", "SponsorName", projectFunding.SponsorID);
            return View(projectFunding);
        }

        // GET: ProjectFunding/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var projectFunding = await _context.ProjectFundings.FindAsync(id);
            if (projectFunding == null) return NotFound();

            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectName", projectFunding.ProjectID);
            ViewData["SponsorID"] = new SelectList(_context.Sponsors, "SponsorID", "SponsorName", projectFunding.SponsorID);
            return View(projectFunding);
        }

        // POST: ProjectFunding/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FundingID,SponsorID,ProjectID,FundingAmount,FundingDate")] ProjectFunding projectFunding)
        {
            if (id != projectFunding.FundingID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectFunding);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectFundingExists(projectFunding.FundingID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "ProjectName", projectFunding.ProjectID);
            ViewData["SponsorID"] = new SelectList(_context.Sponsors, "SponsorID", "SponsorName", projectFunding.SponsorID);
            return View(projectFunding);
        }

        // GET: ProjectFunding/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var projectFunding = await _context.ProjectFundings
                .Include(p => p.Project)
                .Include(p => p.Sponsor)
                .FirstOrDefaultAsync(m => m.FundingID == id);
            if (projectFunding == null) return NotFound();

            return View(projectFunding);
        }

        // POST: ProjectFunding/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectFunding = await _context.ProjectFundings.FindAsync(id);
            if (projectFunding != null)
            {
                _context.ProjectFundings.Remove(projectFunding);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectFundingExists(int id)
        {
            return _context.ProjectFundings.Any(e => e.FundingID == id);
        }
    }
}
