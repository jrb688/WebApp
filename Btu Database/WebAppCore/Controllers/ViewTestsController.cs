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
                .SingleOrDefaultAsync(m => m.TestId == id); ;
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

        // GET: ViewTests/AddProc/5
        public async Task<IActionResult> AddProcedurePartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proc = await _context.Test
                .Include(t => t.Ecu)
                .Include(t => t.User)
                .Include(t => t.TestProc)
                .ThenInclude(tp => tp.Proc)
                .Include(tr => tr.Requirement)
                .SingleOrDefaultAsync(m => m.TestId == id); ;
            if (proc == null)
            {
                return NotFound();
            }
            ViewData["EcuId"] = new SelectList(_context.Ecu, "EcuId", "EcuModel", proc.EcuId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", proc.UserId);
            return View(proc);
        }

        // POST: ViewTests/AddProccedurePartial/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProcedurePartial(int id, [Bind("TestId,TestVersion,ProcId,BatchId,BatchVersion,Order,Parameters,Passed,ReqId")] TestProc Procedure)
        {
            if (id != Procedure.TestId)
            {
                return NotFound();
            }

            var btu_DatabaseContext = _context.TestProc.Include(tp => tp.Proc).Include(tp => tp.Req);
            var results = from info in btu_DatabaseContext select info;
            results = results.Where(s => (s.TestId == id));
            Procedure.Order = results.Count();
            Procedure.BatchId = 0;
            Procedure.BatchVersion = 0;
            Procedure.TestId = 1;
            Procedure.TestVersion = 1;
            Procedure.ReqId = 0;
            Procedure.Parameters = "";

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(Procedure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestExists(Procedure.ProcId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
            //}
            return RedirectToAction("Edit", "ViewTests", new { id = 1 });
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
    }
}
