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
    public class CDsController : Controller
    {
        private readonly CDContext _context;

        public CDsController(CDContext context)
        {
            _context = context;
        }

        // GET: CDs
        public async Task<IActionResult> Index(string searchString, int id)
        {

            var cdContext = _context.CDs.Include(c => c.Artist);

            var cds = from m in _context.CDs
                      select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                cds = cds.Where(s => s.Name.Contains(searchString));
                foreach (var t in cdContext)
                {
                    var x = t.ArtistId;
                    if (x == id)
                    {
                        ViewData["test"] = t.Name;

                    }
                }


                return View(await cds.ToListAsync());
            }

            cds = _context.CDs.Include(c => c.Artist);



            var newdate = _context.CDs.Include(x => x.ReleaseDate);

            ViewData["Dates"] = newdate;
            return View(await cdContext.ToListAsync());
        }
        /*Search */

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: CDs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var cd = await _context.CDs
                .Include(c => c.Artist).Include(c => c.Tracks)
                .FirstOrDefaultAsync(m => m.CDId == id);
            string date = cd.ReleaseDate.ToString("yyyy-MM-dd");
            ViewData["Date"] = date;


            if (cd == null)
            {
                return NotFound();
            }

            return View(cd);
        }

        // GET: CDs/Create
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "Name");
            return View();
        }

        // POST: CDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CDId,Name,ReleaseDate,Available,ArtistId")] CD cd)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "Name", cd.ArtistId);
            return View(cd);
        }

        // GET: CDs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cd = await _context.CDs.FindAsync(id);
            if (cd == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "Name", cd.ArtistId);
            return View(cd);
        }

        // POST: CDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CDId,Name,ReleaseDate,Available,ArtistId")] CD cd)
        {
            if (id != cd.CDId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CdExists(cd.CDId))
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
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "Name", cd.ArtistId);
            return View(cd);
        }

        // GET: CDs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cd = await _context.CDs
                .Include(c => c.Artist)
                .FirstOrDefaultAsync(m => m.CDId == id);
            string date = cd.ReleaseDate.ToString("yyyy-MM-dd");
            ViewData["Date"] = date;

            if (cd == null)
            {
                return NotFound();
            }

            return View(cd);
        }

        // POST: CDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cd = await _context.CDs.FindAsync(id);
            _context.CDs.Remove(cd);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CdExists(int id)
        {
            return _context.CDs.Any(e => e.CDId == id);
        }
    }
}
