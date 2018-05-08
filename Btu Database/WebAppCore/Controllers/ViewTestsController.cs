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
    public class ViewTestsController : Controller
    {
        private readonly Btu_DatabaseContext _context;

        public ViewTestsController(Btu_DatabaseContext context)
        {
            _context = context;
        }

        // GET: ViewTests
        public async Task<IActionResult> Index(string searchString, string options)
        {
            var btu_DatabaseContext = _context.Test.Include(t => t.Ecu).Include(t => t.User);
            var results = from info in btu_DatabaseContext
                         select info;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (options.Equals("TestId"))
                {
                    results = results.Where(s => s.TestId.Equals(Int32.Parse(searchString)));
                }
                else if (options.Equals("TestName"))
                {
                    results = results.Where(s => s.Name.Contains(searchString));
                }
                else if (options.Equals("Ecu"))
                {
                    results = results.Where(s => s.Ecu.EcuModel.Contains(searchString));
                }
                else if (options.Equals("Tester"))
                {
                    results = results.Where(s => (s.User.FirstName + " " + s.User.LastName).Contains(searchString));
                }
            }

            return View(await results.ToListAsync());
        }

        // GET: ViewTests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Test
                .Include(t => t.Ecu)
                .Include(t => t.User)
                .Include(t => t.TestProc)
                .ThenInclude(tp => tp.Proc)
                .Include(tr => tr.Requirement)
                .SingleOrDefaultAsync(m => m.TestId == id);

            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // GET: ViewTests/Create
        public IActionResult Create()
        {
            ViewData["EcuId"] = new SelectList(_context.Ecu, "EcuId", "EcuModel");
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email");
            return View();
        }

        // POST: ViewTests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TestId,TestVersion,UserId,EcuId,Name,DateRun")] Test test)
        {
            if (ModelState.IsValid)
            {
                test.DateCreated = DateTime.Now;
                _context.Add(test);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EcuId"] = new SelectList(_context.Ecu, "EcuId", "EcuModel", test.EcuId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", test.UserId);
            return View(test);
        }

        // GET: ViewTests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Test
                .Include(t => t.Ecu)
                .Include(t => t.User)
                .Include(t => t.TestProc)
                .ThenInclude(tp => tp.Proc)
                .Include(tr => tr.Requirement)
                .SingleOrDefaultAsync(m => m.TestId == id);
            if (test == null)
            {
                return NotFound();
            }
            ViewData["EcuId"] = new SelectList(_context.Ecu, "EcuId", "EcuModel", test.EcuId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", test.UserId);
            return View(test);
        }

        // POST: ViewTests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TestId,TestVersion,UserId,EcuId,Name,DateCreated,DateRun")] Test test)
        {
            if (id != test.TestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(test);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestExists(test.TestId))
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
            ViewData["EcuId"] = new SelectList(_context.Ecu, "EcuId", "EcuModel", test.EcuId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", test.UserId);
            return View(test);
        }

        
        // GET: ViewTests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Test
                .Include(t => t.Ecu)
                .Include(t => t.User)
                .SingleOrDefaultAsync(m => m.TestId == id);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // POST: ViewTests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var test = await _context.Test.SingleOrDefaultAsync(m => m.TestId == id);
            _context.Test.Remove(test);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestExists(int id)
        {
            return _context.Test.Any(e => e.TestId == id);
        }

        // GET: ViewTests/CreateProc
        public IActionResult CreateProc(int TestId, int TestVersion)
        {

            ViewData["BatchId"] = new SelectList(_context.Batch, "BatchId", "Status");
            ViewData["ProcId"] = new SelectList(_context.Procedure, "ProcId", "Name");
            ViewData["ReqId"] = new SelectList(_context.Requirement, "ReqId", "Description");
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId");
            ViewData["Test"] = new SelectList(_context.Test, "TestVersion", "TestVersion");
            return View();
        }

        // POST: ViewTests/CreateProcConfirmed
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProcConfirmed([Bind("BatchId,BatchVersion,TestId,TestVersion,ProcId,ReqId,Parameters,Passed,Order")] TestProc testProc)
        {

            if (ModelState.IsValid)
            {
                _context.Add(testProc);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "ViewTests", new { id= testProc.TestId });
            }
            ViewData["BatchId"] = new SelectList(_context.Batch, "BatchId", "Status", testProc.BatchId);
            ViewData["ProcId"] = new SelectList(_context.Procedure, "ProcId", "Description", testProc.ProcId);
            ViewData["ReqId"] = new SelectList(_context.Requirement, "ReqId", "Description", testProc.ReqId);
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId", testProc.TestId);
            return View(testProc);
        }

        // GET: ViewTests/DeleteProc
        public async Task<IActionResult> DeleteProc(int procId, int testId)
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

        // POST: ViewTests/DeleteProc/5
        [HttpPost, ActionName("DeleteProcConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProcConfirmed(int procId, int testId)
        {

            var btu_DatabaseContext = _context.TestProc.Include(t => t.Batch).Include(t => t.Proc).Include(t => t.Req).Include(t => t.Test);
            var results = from info in btu_DatabaseContext
                          select info;
            results = results.Where(s => s.TestId.Equals(testId));
            var testProc = await results.FirstOrDefaultAsync(m => m.ProcId == procId);
            _context.TestProc.Remove(testProc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestProcExists(int id)
        {
            return _context.TestProc.Any(e => e.TestId == id);
        }


        // GET: ViewTests/CreateReq
        public IActionResult CreateReq(int TestId, int TestVersion)
        {
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId");
            return View();
        }

        // POST: ViewTests/CreateReqConfirmed
        [HttpPost, ActionName("CreateReqConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReqConfirmed([Bind("ReqId,TestId,TestVersion,Description")] Requirement requirement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requirement);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "ViewTests", new { id = requirement.TestId });
            }
            ViewData["TestId"] = new SelectList(_context.Test, "TestId", "TestId", requirement.TestId);
            return View(requirement);
        }

        // GET: ViewTests/DeleteReq
        public async Task<IActionResult> DeleteReq(int reqId, int testId)
        {

            var btu_DatabaseContext = _context.Requirement.Include(t => t.Test);
            var results = from info in btu_DatabaseContext
                          select info;
            results = results.Where(s => s.TestId.Equals(testId));
            var requirement = await results.FirstOrDefaultAsync(m => m.ReqId == reqId);
            if (requirement == null)
            {
                return NotFound();
            }

            return View(requirement);
        }

        // POST: ViewTests/DeleteReqConfirmed/5
        [HttpPost, ActionName("DeleteReqConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReqConfirmed(int reqId, int testId)
        {
            var btu_DatabaseContext = _context.Requirement.Include(t => t.Test);
            var results = from info in btu_DatabaseContext
                          select info;
            results = results.Where(s => s.TestId.Equals(testId));
            var requirement = await results.FirstOrDefaultAsync(m => m.ReqId == reqId);
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
