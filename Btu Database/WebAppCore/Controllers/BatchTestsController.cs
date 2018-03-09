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
    public class BatchTestsController : Controller
    {
        private readonly Btu_DatabaseContext _context;

        public BatchTestsController(Btu_DatabaseContext context)
        {
            _context = context;
        }

        // GET: BatchTests
        public async Task<IActionResult> Index()
        {
            var btu_DatabaseContext = _context.BatchTest.Include(b => b.Batch).Include(b => b.Batch.Sim).Include(b => b.Batch.Sim.Ecu).Include(b => b.Test);
            return View(await btu_DatabaseContext.ToListAsync());
        }

        // GET: BatchTests/Details/5
        public async Task<IActionResult> Details(int? id, int? version)
        {
            if (id == null)
            {
                return NotFound();
            }

            var btu_DatabaseContext = _context.BatchTest.Include(bt => bt.Batch).ThenInclude(b => b.Sim).ThenInclude(s => s.Ecu).Include(bt => bt.Test).ThenInclude(t => t.TestProc).ThenInclude(tp => tp.Proc).Include(bt => bt.Test).ThenInclude(t => t.Requirement);
            var results = from info in btu_DatabaseContext select info;
            results = results.Where(s => (s.BatchId == id));
            results = results.Where(s => (s.BatchVersion == version));

            return View(await results.ToListAsync());

            //var batchTest = await _context.BatchTest
            //    .Include(b => b.Batch)
            //    .Include(b => b.Test)
            //    .SingleOrDefaultAsync(m => m.BatchId == id);
            //if (batchTest == null)
            //{
            //    return NotFound();
            //}

            //return View(batchTest);
        }

        // GET: BatchTests/Create
        public IActionResult Create()
        {
            ViewData["BatchId"] = new SelectList(_context.Batch, "BatchId", "Status");
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId");
            return View();
        }

        // POST: BatchTests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BatchId,BatchVersion,TestId,TestVersion,Passed")] BatchTest batchTest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(batchTest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BatchId"] = new SelectList(_context.Batch, "BatchId", "Status", batchTest.BatchId);
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId", batchTest.TestId);
            return View(batchTest);
        }

        // GET: BatchTests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batchTest = await _context.BatchTest.SingleOrDefaultAsync(m => m.BatchId == id);
            if (batchTest == null)
            {
                return NotFound();
            }
            ViewData["BatchId"] = new SelectList(_context.Batch, "BatchId", "Status", batchTest.BatchId);
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId", batchTest.TestId);
            return View(batchTest);
        }

        // POST: BatchTests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BatchId,BatchVersion,TestId,TestVersion,Passed")] BatchTest batchTest)
        {
            if (id != batchTest.BatchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(batchTest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BatchTestExists(batchTest.BatchId))
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
            ViewData["BatchId"] = new SelectList(_context.Batch, "BatchId", "Status", batchTest.BatchId);
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId", batchTest.TestId);
            return View(batchTest);
        }

        // GET: BatchTests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batchTest = await _context.BatchTest
                .Include(b => b.Batch)
                .Include(b => b.Test)
                .SingleOrDefaultAsync(m => m.BatchId == id);
            if (batchTest == null)
            {
                return NotFound();
            }

            return View(batchTest);
        }

        // POST: BatchTests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var batchTest = await _context.BatchTest.SingleOrDefaultAsync(m => m.BatchId == id);
            _context.BatchTest.Remove(batchTest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BatchTestExists(int id)
        {
            return _context.BatchTest.Any(e => e.BatchId == id);
        }
    }
}
