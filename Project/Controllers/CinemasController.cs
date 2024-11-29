using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using S.Data.Entities;
using X.PagedList;
using X.PagedList.EF;

namespace Project.Controllers
{
    public class CinemasController : Controller
    {
        private readonly ScreeningDbContext _context;

        public CinemasController(ScreeningDbContext context)
        {
            _context = context;
        }

        // GET: Cinemas
        public async Task<IActionResult> Index(string sortOrder, int? page, string searchNameString, string searchAddressString)
        {
            int pageCurrent = page ?? 1; //page == null ? 1 : page
            int pageMaxSize = 10;

            var cinemas = _context.Cinemas.AsQueryable();


            ViewBag.SortOrder = sortOrder;
            ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "name-desc" : "";
            ViewBag.TotalSeatsSortParam = sortOrder == "seats-desc" ? "seats-asc" : "seats-desc";


            cinemas = sortOrder switch
            {
                "name-desc" => cinemas.OrderByDescending(x => x.Name),
                "seats-asc" => cinemas.OrderBy(x => x.TotalSeats), // Ascending order for TotalSeats
                "seats-desc" => cinemas.OrderByDescending(x => x.TotalSeats), // Descending order for TotalSeats
                _ => cinemas.OrderBy(x => x.Name),
            };

            if (!String.IsNullOrEmpty(searchNameString))
            {
                cinemas = cinemas.Where(m => m.Name.Contains(searchNameString));

            }

            if (!String.IsNullOrEmpty(searchAddressString))
            {
                cinemas = cinemas.Where(m => m.Address.Contains(searchAddressString));

            }



            return View(await cinemas.ToPagedListAsync(pageCurrent, pageMaxSize));
            //return View(await _context.Cinemas.ToListAsync());
        }

        // GET: Cinemas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinema = await _context.Cinemas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cinema == null)
            {
                return NotFound();
            }

            return View(cinema);
        }

        [Authorize(Roles = "Admin")]

        // GET: Cinemas/Create
        public IActionResult Create()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        // POST: Cinemas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,NumberOfHalls,Website,TotalSeats")] Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cinema);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cinema);
        }
        [Authorize(Roles = "Admin")]
        // GET: Cinemas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema == null)
            {
                return NotFound();
            }
            return View(cinema);
        }
        [Authorize(Roles = "Admin")]

        // POST: Cinemas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,NumberOfHalls,Website,TotalSeats")] Cinema cinema)
        {
            if (id != cinema.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cinema);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CinemaExists(cinema.Id))
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
            return View(cinema);
        }

        [Authorize(Roles = "Admin")]
        // GET: Cinemas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinema = await _context.Cinemas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cinema == null)
            {
                return NotFound();
            }

            return View(cinema);
        }

        [Authorize(Roles = "Admin")]
        // POST: Cinemas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema != null)
            {
                _context.Cinemas.Remove(cinema);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CinemaExists(int id)
        {
            return _context.Cinemas.Any(e => e.Id == id);
        }
    }
}
