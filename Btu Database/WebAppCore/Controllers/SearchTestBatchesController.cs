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
    public class SearchTestBatchesController : Controller
    {
        private readonly Btu_DatabaseContext _context;

        public SearchTestBatchesController(Btu_DatabaseContext context)
        {
            _context = context;
        }

        // GET: SearchTestBatches
        public async Task<IActionResult> Index(string searchString)
        {
            var btu_DatabaseContext = _context.Batch.Include(b => b.AuthorUser).Include(b => b.Sim).Include(b => b.TesterUser);
            var batch = from info in btu_DatabaseContext
                        select info;

            if (!String.IsNullOrEmpty(searchString))
            {
                batch = batch.Where(s => s.Name.Contains(searchString));
            }
            return View(await btu_DatabaseContext.ToListAsync());
        }

        // GET: SearchTestBatches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _context.Batch
                .Include(b => b.AuthorUser)
                .Include(b => b.Sim)
                .Include(b => b.BatchId)
                .Include(b => b.TesterUser)
                .Include(b => b.BatchVersion)
                .SingleOrDefaultAsync(m => m.BatchId == id);
            if (batch == null)
            {
                return NotFound();
            }

            return View(batch);
        }

        // GET: SearchTestBatches/Create
        public IActionResult Create()
        {
            ViewData["AuthorUserId"] = new SelectList(_context.User, "UserId", "Email");
            ViewData["SimId"] = new SelectList(_context.Simulator, "SimId", "SimId");
            ViewData["TesterUserId"] = new SelectList(_context.User, "UserId", "Email");
            return View();
        }

        // POST: SearchTestBatches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BatchId,BatchVersion,AuthorUserId,TesterUserId,SimId,Name,Status,DateCreated,DateRun,Display")] Batch batch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(batch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorUserId"] = new SelectList(_context.User, "UserId", "Email", batch.AuthorUserId);
            ViewData["SimId"] = new SelectList(_context.Simulator, "SimId", "SimId", batch.SimId);
            ViewData["TesterUserId"] = new SelectList(_context.User, "UserId", "Email", batch.TesterUserId);
            return View(batch);
        }

        // GET: SearchTestBatches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _context.Batch.SingleOrDefaultAsync(m => m.BatchId == id);
            if (batch == null)
            {
                return NotFound();
            }
            ViewData["AuthorUserId"] = new SelectList(_context.User, "UserId", "Email", batch.AuthorUserId);
            ViewData["SimId"] = new SelectList(_context.Simulator, "SimId", "SimId", batch.SimId);
            ViewData["TesterUserId"] = new SelectList(_context.User, "UserId", "Email", batch.TesterUserId);
            return View(batch);
        }

        // POST: SearchTestBatches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BatchId,BatchVersion,AuthorUserId,TesterUserId,SimId,Name,Status,DateCreated,DateRun,Display")] Batch batch)
        {
            if (id != batch.BatchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(batch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BatchExists(batch.BatchId))
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
            ViewData["AuthorUserId"] = new SelectList(_context.User, "UserId", "Email", batch.AuthorUserId);
            ViewData["SimId"] = new SelectList(_context.Simulator, "SimId", "SimId", batch.SimId);
            ViewData["TesterUserId"] = new SelectList(_context.User, "UserId", "Email", batch.TesterUserId);
            return View(batch);
        }

        // GET: SearchTestBatches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _context.Batch
                .Include(b => b.AuthorUser)
                .Include(b => b.Sim)
                .Include(b => b.TesterUser)
                .SingleOrDefaultAsync(m => m.BatchId == id);
            if (batch == null)
            {
                return NotFound();
            }

            return View(batch);
        }

        // POST: SearchTestBatches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var batch = await _context.Batch.SingleOrDefaultAsync(m => m.BatchId == id);
            _context.Batch.Remove(batch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BatchExists(int id)
        {
            return _context.Batch.Any(e => e.BatchId == id);
        }
    }
}
