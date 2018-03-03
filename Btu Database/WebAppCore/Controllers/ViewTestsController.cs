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
        public async Task<IActionResult> Index(string searchString)
        {
            var btu_DatabaseContext = _context.Test.Include(t => t.Ecu).Include(t => t.User);
            var test = from info in btu_DatabaseContext
                         select info;

            if (!String.IsNullOrEmpty(searchString))
            {
                test = test.Where(s => s.Name.Contains(searchString));
            }

            return View(await test.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("TestId,TestVersion,UserId,EcuId,Name,DateCreated,DateRun")] Test test)
        {
            if (ModelState.IsValid)
            {
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

            var test = await _context.Test.SingleOrDefaultAsync(m => m.TestId == id);
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
    }
}
