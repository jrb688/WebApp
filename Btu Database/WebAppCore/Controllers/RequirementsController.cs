using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppCore.Models;

namespace WebAppCore.Controllers
{
    public class RequirementsController : Controller
    {
        private readonly Btu_DatabaseContext _context;

        public RequirementsController(Btu_DatabaseContext context)
        {
            _context = context;
        }

        // GET: Requirements
        public async Task<IActionResult> Index(int id, int version)
        {

            var btu_DatabaseContext = _context.TestProc.Include(t => t.Batch).Include(t => t.Proc).Include(t => t.Req).Include(t => t.Test);
            var results = from info in btu_DatabaseContext
                          select info;
            results = results.Where(s => s.TestId.Equals(id));
            results = results.Where(t => t.TestVersion.Equals(version));

            return View(await results.ToListAsync());

        }

        // GET: Requirements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requirement = await _context.Requirement
                .Include(r => r.Test)
                .SingleOrDefaultAsync(m => m.ReqId == id);
            if (requirement == null)
            {
                return NotFound();
            }

            return View(requirement);
        }

        // GET: Requirements/Create
        public IActionResult Create(int TestId, int TestVersion)
        {
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId");
            return View();
        }

        // POST: Requirements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReqId,TestId,TestVersion,Description")] Requirement requirement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requirement);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "ViewTests", new { area = "ViewTests" });
            }
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId", requirement.TestId);
            return View(requirement);
        }

        // GET: Requirements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requirement = await _context.Requirement.SingleOrDefaultAsync(m => m.ReqId == id);
            if (requirement == null)
            {
                return NotFound();
            }
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId", requirement.TestId);
            return View(requirement);
        }

        // POST: Requirements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReqId,TestId,TestVersion,Description")] Requirement requirement)
        {
            if (id != requirement.ReqId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requirement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequirementExists(requirement.ReqId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId", requirement.TestId);
            return View(requirement);
        }

        // GET: Requirements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requirement = await _context.Requirement
                .Include(r => r.Test)
                .SingleOrDefaultAsync(m => m.ReqId == id);
            if (requirement == null)
            {
                return NotFound();
            }

            return View(requirement);
        }

        // POST: Requirements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requirement = await _context.Requirement.SingleOrDefaultAsync(m => m.ReqId == id);
            _context.Requirement.Remove(requirement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequirementExists(int id)
        {
            return _context.Requirement.Any(e => e.ReqId == id);
        }
    }
}
