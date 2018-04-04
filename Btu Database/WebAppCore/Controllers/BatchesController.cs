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
    public class BatchesController : Controller
    {
        private readonly Btu_DatabaseContext _context;

        public BatchesController(Btu_DatabaseContext context)
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

        // GET: Queues
        public async Task<IActionResult> Queues(int? BatchId, string BatchName, int? SimId, string Ecu, string Tester, bool Queued = false, bool Running = false, bool Complete = false, bool search = false)
        {
            var btu_DatabaseContext = _context.Batch.Include(b => b.AuthorUser).Include(b => b.Sim).Include(b => b.Sim.Ecu).Include(b => b.TesterUser);
            var results = from info in btu_DatabaseContext select info;
            results = results.Where(s => (s.Status.Equals("Queued") | s.Status.Equals("Complete") | s.Status.Equals("Running")));
            if (search)
            {
                if (BatchId != null)
                {
                    results = results.Where(s => s.BatchId == BatchId);
                }

                if (BatchName != null)
                {
                    results = results.Where(s => s.Name.Contains(BatchName));
                }
                if (SimId != null)
                {
                    results = results.Where(s => s.SimId == SimId);
                }
                if (Ecu != null)
                {
                    results = results.Where(s => s.Sim.Ecu.EcuModel.Contains(Ecu));
                }
                if (Tester != null)
                {
                    results = results.Where(s => (s.TesterUser.FirstName + ' ' + s.TesterUser.LastName).Contains(Tester));
                }
                if (!Queued)
                {
                    results = results.Where(s => s.Status.Equals("Complete") | s.Status.Equals("Running"));
                }
                if (!Complete)
                {
                    results = results.Where(s => s.Status.Equals("Queued") | s.Status.Equals("Running"));
                }
                if (!Running)
                {
                    results = results.Where(s => s.Status.Equals("Queued") | s.Status.Equals("Complete"));
                }
            }

            return View(await results.ToListAsync());
        }


        // POST: Queues/Delete/5
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var batch = await _context.Batch.SingleOrDefaultAsync(m => m.BatchId == id);
            _context.Update(batch);
            if (batch.Status.Equals("Complete"))
            {
                batch.Display = 0;
            }
            else if(batch.Status.Equals("Queued"))
            {
                batch.Status = "Made";
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Queues/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Queues/Create
        public IActionResult Create()
        {
            ViewData["AuthorUserId"] = new SelectList(_context.User, "UserId", "Email");
            ViewData["SimId"] = new SelectList(_context.Simulator, "SimId", "SimId");
            ViewData["TesterUserId"] = new SelectList(_context.User, "UserId", "Email");
            return View();
        }

        // POST: Queues/Create
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

        // POST: Queues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew([Bind("BatchId,BatchVersion,AuthorUserId,TesterUserId,SimId,Name,Status,DateCreated,DateRun,Display")] Batch batch)
        {
            batch.Status = "Made";
            batch.DateCreated = DateTime.Now;
            batch.Display = 1;
            batch.TesterUserId = null;

            if (ModelState.IsValid)
            {
                _context.Add(batch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = batch.BatchId, version = batch.BatchVersion });
            }
            ViewData["AuthorUserId"] = new SelectList(_context.User, "UserId", "Email", batch.AuthorUserId);
            ViewData["SimId"] = new SelectList(_context.Simulator, "SimId", "SimId", batch.SimId);
            ViewData["TesterUserId"] = new SelectList(_context.User, "UserId", "Email", batch.TesterUserId);
            return RedirectToAction(nameof(Create));
        }

        // GET: Queues/Edit/5
        public async Task<IActionResult> Edit(int? id, int? version, string message = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var btu_DatabaseContext = _context.Batch.Include(b => b.AuthorUser).Include(b => b.Sim).Include(b => b.TesterUser).Include(b => b.BatchTest);
            var batch = from info in btu_DatabaseContext
                        select info;
            batch = batch.Where(s => (s.BatchId == id && s.BatchVersion == version));
            var singleBatch = batch.FirstOrDefault();

            ViewData["AuthorUserId"] = new SelectList(_context.User, "UserId", "Email", singleBatch.AuthorUserId);
            ViewData["SimId"] = new SelectList(_context.Simulator, "SimId", "SimId", singleBatch.SimId);
            ViewData["TesterUserId"] = new SelectList(_context.User, "UserId", "Email", singleBatch.TesterUserId);
            ViewData["Message"] = message;
            return View(singleBatch);
        }

        // POST: Queues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNew([Bind("BatchId,BatchVersion,AuthorUserId,TesterUserId,SimId,Name,Status,DateCreated,DateRun,Display")] Batch batch)
        {
            var currentState = await _context.Batch.SingleOrDefaultAsync(m => m.BatchId == batch.BatchId && m.BatchVersion == batch.BatchVersion);
            currentState.AuthorUserId = batch.AuthorUserId;
            currentState.Name = batch.Name;
            currentState.SimId = batch.SimId;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(currentState);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BatchExists(currentState.BatchId))
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

        // GET: Queues/Delete/5
        public async Task<IActionResult> Delete(int? id, int? version)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _context.Batch
                .Include(b => b.AuthorUser)
                .Include(b => b.Sim)
                .Include(b => b.TesterUser)
                .SingleOrDefaultAsync(m => (m.BatchId == id && m.BatchVersion == version));
            if (batch == null)
            {
                return NotFound();
            }

            return View(batch);
        }

        // POST: Queues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int version)
        {
            var batch = _context.Batch.Include(b => b.BatchTest).Include(b => b.TestProc);
            var results = from info in batch select info;
            results = results.Where(m => (m.BatchId == id && m.BatchVersion == version));
            foreach(Batch b in results)
            {
                _context.Batch.Remove(b);
            }

            var testBatch = _context.BatchTest;
            var results2 = from info in testBatch select info;
            results2 = results2.Where(m => (m.BatchId == id && m.BatchVersion == version));
            foreach (BatchTest bt in results2)
            {
                _context.BatchTest.Remove(bt);
            }

            var testProc = _context.TestProc;
            var results3 = from info in testProc select info;
            results3 = results3.Where(m => (m.BatchId == id && m.BatchVersion == version));
            foreach (TestProc tp in results3)
            {
                _context.TestProc.Remove(tp);
            }


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BatchExists(int id)
        {
            return _context.Batch.Any(e => e.BatchId == id);
        }

        // GET: Queues/AddTests/5
        public async Task<IActionResult> AddTests(int? BatchId, int? BatchVersion)
        {
            var btu_DatabaseContext = _context.Test;
            var tests = from info in btu_DatabaseContext select info;
            var btu_DatabaseContext2 = _context.BatchTest;
            var batchTests = from info in btu_DatabaseContext2 select info;
            batchTests = batchTests.Where(s => (s.BatchId == BatchId && s.BatchVersion == BatchVersion));

            foreach(var batchTest in batchTests)
            {
                tests = tests.Where(s => s.BatchTest != batchTest);
            }

            //var retVal = tests.FirstOrDefault() ?? new NullTime();

            ViewData["BatchId"] = BatchId;
            ViewData["BatchVersion"] = BatchVersion;
            if(btu_DatabaseContext == null)
            {
                return View(tests);
            }
            if(tests != null && tests.Any())
            {
                return View(tests);
            }

            return RedirectToAction(nameof(Edit), new { id = BatchId, version = BatchVersion, message = "No Unused Tests" });
        }

        // Post: Queues/AddTests/5
        [HttpPost, ActionName("AddTests")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTests(int TestId, int BatchId, int BatchVersion)
        {
            string firstName = HttpContext.Request.Form["Batchid"];
            BatchTest batchTest = new BatchTest();
            batchTest.BatchId = BatchId;
            batchTest.BatchVersion = BatchVersion;
            batchTest.TestId = TestId;
            batchTest.TestVersion = 1;

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
    }
}
