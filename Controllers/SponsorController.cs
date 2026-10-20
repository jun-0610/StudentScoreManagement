using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentScoreManagement.Models;
using System.Threading.Tasks;

namespace StudentScoreManagement.Controllers
{
    public class SponsorController : Controller
    {
        private readonly AppDbContext _context;

        public SponsorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Sponsor
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sponsors.ToListAsync());
        }

        // GET: Sponsor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var sponsor = await _context.Sponsors
                .Include(s => s.ProjectFundings)
                .ThenInclude(pf => pf.Project)
                .FirstOrDefaultAsync(m => m.SponsorID == id);

            if (sponsor == null) return NotFound();

            return View(sponsor);
        }

        // GET: Sponsor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sponsor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SponsorID,SponsorName,ContactEmail")] Sponsor sponsor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sponsor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sponsor);
        }

        // GET: Sponsor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var sponsor = await _context.Sponsors.FindAsync(id);
            if (sponsor == null) return NotFound();

            return View(sponsor);
        }

        // POST: Sponsor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SponsorID,SponsorName,ContactEmail")] Sponsor sponsor)
        {
            if (id != sponsor.SponsorID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sponsor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SponsorExists(sponsor.SponsorID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sponsor);
        }

        // GET: Sponsor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var sponsor = await _context.Sponsors
                .FirstOrDefaultAsync(m => m.SponsorID == id);
            if (sponsor == null) return NotFound();

            return View(sponsor);
        }

        // POST: Sponsor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sponsor = await _context.Sponsors.FindAsync(id);
            if (sponsor != null)
            {
                _context.Sponsors.Remove(sponsor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SponsorExists(int id)
        {
            return _context.Sponsors.Any(e => e.SponsorID == id);
        }
    }
}
