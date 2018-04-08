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
    public class TestProcsController : Controller
    {
        private readonly Btu_DatabaseContext _context;

        public TestProcsController(Btu_DatabaseContext context)
        {
            _context = context;
        }

        // GET: TestProcs
        public async Task<IActionResult> Index(int id, int version)
        {
            
            var btu_DatabaseContext = _context.TestProc.Include(t => t.Batch).Include(t => t.Proc).Include(t => t.Req).Include(t => t.Test);
            var results = from info in btu_DatabaseContext
                          select info;
            results = results.Where(s => s.TestId.Equals(id));
            results = results.Where(t => t.TestVersion.Equals(version));

            return View(await results.ToListAsync());

        }

        // GET: TestProcs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testProc = await _context.TestProc
                .Include(t => t.Batch)
                .Include(t => t.Proc)
                .Include(t => t.Req)
                .Include(t => t.Test)
                .SingleOrDefaultAsync(m => m.TestId == id);
            if (testProc == null)
            {
                return NotFound();
            }

            return View(testProc);
        }

        // GET: TestProcs/Create
        public IActionResult Create(int TestId, int TestVersion)
        {

            ViewData["BatchId"] = new SelectList(_context.Batch, "BatchId", "Status");
            ViewData["ProcId"] = new SelectList(_context.Procedure, "ProcId", "Description");
            ViewData["ReqId"] = new SelectList(_context.Requirement, "ReqId", "Description");
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId");
            ViewData["Test"] = new SelectList(_context.Test, "TestVersion", "TestVersion");
            return View();
        }

        // POST: TestProcs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BatchId,BatchVersion,TestId,TestVersion,ProcId,ReqId,Parameters,Passed,Order")] TestProc testProc)
        {

            if (ModelState.IsValid)
            {
                _context.Add(testProc);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "ViewTests", new { area = "ViewTests" });
            }
            ViewData["BatchId"] = new SelectList(_context.Batch, "BatchId", "Status", testProc.BatchId);
            ViewData["ProcId"] = new SelectList(_context.Procedure, "ProcId", "Description", testProc.ProcId);
            ViewData["ReqId"] = new SelectList(_context.Requirement, "ReqId", "Description", testProc.ReqId);
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId", testProc.TestId);
            return View(testProc);
        }

        // GET: TestProcs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testProc = await _context.TestProc.SingleOrDefaultAsync(m => m.TestId == id);
            if (testProc == null)
            {
                return NotFound();
            }
            ViewData["BatchId"] = new SelectList(_context.Batch, "BatchId", "Status", testProc.BatchId);
            ViewData["ProcId"] = new SelectList(_context.Procedure, "ProcId", "Description", testProc.ProcId);
            ViewData["ReqId"] = new SelectList(_context.Requirement, "ReqId", "Description", testProc.ReqId);
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId", testProc.TestId);
            return View(testProc);
        }

        // POST: TestProcs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BatchId,BatchVersion,TestId,TestVersion,ProcId,ReqId,Parameters,Passed,Order")] TestProc testProc)
        {
            if (id != testProc.TestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testProc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestProcExists(testProc.TestId))
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
            ViewData["BatchId"] = new SelectList(_context.Batch, "BatchId", "Status", testProc.BatchId);
            ViewData["ProcId"] = new SelectList(_context.Procedure, "ProcId", "Description", testProc.ProcId);
            ViewData["ReqId"] = new SelectList(_context.Requirement, "ReqId", "Description", testProc.ReqId);
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId", testProc.TestId);
            return View(testProc);
        }

        // GET: TestProcs/Delete/5
        public async Task<IActionResult> Delete(int? procId, int testId)
        {
           
            var btu_DatabaseContext = _context.TestProc.Include(t => t.Batch).Include(t => t.Proc).Include(t => t.Req).Include(t => t.Test);
            var results = from info in btu_DatabaseContext
                          select info;
            results = results.Where(s => s.TestId.Equals(testId));
            var testProc = await results.FirstOrDefaultAsync(m => m.ProcId == procId);

            if (testProc == null)
            {
                return NotFound();
            }

            return View(testProc);
        }

        // POST: TestProcs/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int pId)
        {

            var testProc = await _context.TestProc.SingleOrDefaultAsync(m => m.TestId == id);
            _context.TestProc.Remove(testProc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestProcExists(int id)
        {
            return _context.TestProc.Any(e => e.TestId == id);
        }
    }
}
