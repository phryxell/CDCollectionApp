using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CDCollectionApp.Data;
using CDCollectionApp.Models;

namespace CDCollectionApp.Controllers
{
    public class RentsController : Controller
    {
        private readonly CDContext _context;

        public RentsController(CDContext context)
        {
            _context = context;
        }

        // GET: Rents
        public async Task<IActionResult> Index()
        {
            var CDContext = _context.Rents.Include(r => r.CD);
            return View(await CDContext.ToListAsync());
        }

        // GET: Rents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rents
                .Include(r => r.CD)
                .FirstOrDefaultAsync(m => m.RentId == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }


        // GET: Rents/Create
        public IActionResult Create()
        {

            var idAndName = _context.CDs
               .Where(x => x.Available == true)
               .Select(x => new { x.CDId, x.Name });

            ViewBag.CDId = new SelectList(idAndName, "CDId", "Name");

            return View();
        }

        // POST: Rents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RentId,Name,RentDate,CDId")] Rent rent)
        {


            if (ModelState.IsValid)
            {
                CD result = (from p in _context.CDs
                             where p.CDId == rent.CDId
                             select p).SingleOrDefault();
                result.Available = false;

                _context.SaveChanges();

                _context.Add(rent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CDId = new SelectList(_context.CDs, "CDId", "Name");

            return View(rent);
        }

        // GET: Rents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rents.FindAsync(id);
            if (rent == null)
            {
                return NotFound();
            }
            var idAndName = _context.CDs
          .Where(x => x.Available == true)
          .Select(x => new { x.CDId, x.Name });



            ViewBag.CDId = new SelectList(idAndName, "CDId", "Name");
            return View(rent);
        }

        // POST: Rents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RentId,Name,RentDate,CDId")] Rent rent)
        {
            if (id != rent.RentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentExists(rent.RentId))
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
            ViewData["CDId"] = new SelectList(_context.CDs, "CDId", "CDId", rent.CDId);
            return View(rent);
        }

        // GET: Rents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }




            var rent = await _context.Rents
                .Include(r => r.CD)
                .FirstOrDefaultAsync(m => m.RentId == id);

            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // POST: Rents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var rent = await _context.Rents.FindAsync(id);

            _context.Rents.Remove(rent);

            CD results = (from p in _context.CDs
                          where p.CDId == rent.CDId
                          select p).SingleOrDefault();
            results.Available = true;

            _context.SaveChanges();



            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentExists(int id)
        {
            return _context.Rents.Any(e => e.RentId == id);
        }
    }
}
