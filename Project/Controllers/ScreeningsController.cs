using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using S.Data.Entities;
using X.PagedList;
using X.PagedList.EF;

namespace Project.Controllers
{
    public class ScreeningsController : Controller
    {
        private readonly ScreeningDbContext _context;

        public ScreeningsController(ScreeningDbContext context)
        {
            _context = context;
        }

        // GET: Screenings
        public async Task<IActionResult> Index( string sortOrder, int? page, int? searchMovieId, int? searchCinemaId)
        {
            int pageCurrent = page ?? 1; //page == null ? 1 : page
            int pageMaxSize = 10;

            var screeningDbContext = _context.Screenings.Include(s => s.Cinema).Include(s => s.Movie).AsQueryable();

            ViewBag.SortOrder = sortOrder;
            ViewBag.TicketPriceParam = sortOrder == "price-desc" ? "price-asc" : "price-desc";
            ViewBag.ScreeningDateParam = sortOrder == "sdate-desc" ? "sdate-asc" : "sdate-desc";

            ViewBag.Movies = new SelectList(_context.Movies, "Id", "Title");
            ViewBag.Cinemas = new SelectList(_context.Cinemas, "Id", "Name");

            ViewBag.MoviesIdSearch = searchMovieId.ToString();
            if (searchMovieId.HasValue)
                screeningDbContext = screeningDbContext.Where(x => x.MovieId == searchMovieId);

            ViewBag.CinemaIdSearch = searchCinemaId.ToString();
            if (searchCinemaId.HasValue)
                screeningDbContext = screeningDbContext.Where(x => x.CinemaId == searchCinemaId);



            screeningDbContext = sortOrder switch
            {
                "price-desc" => screeningDbContext.OrderByDescending(x => x.TicketPrice),
                "price-asc" => screeningDbContext.OrderBy(x => x.TicketPrice),
                "sdate-asc" => screeningDbContext.OrderBy(x => x.ScreeningDate),
                "sdate-desc" => screeningDbContext.OrderByDescending(x => x.ScreeningDate),
                _ => screeningDbContext.OrderBy(x => x.TicketPrice),
            };


            return View(await screeningDbContext.ToPagedListAsync(pageCurrent, pageMaxSize));
            //  return View(await screeningDbContext.ToListAsync());
        }

        // GET: Screenings/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = await _context.Screenings
                .Include(s => s.Cinema)
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.ScreeningId == id);
            if (screening == null)
            {
                return NotFound();
            }

            return View(screening);
        }

        // GET: Screenings/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name");
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: Screenings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScreeningId,MovieId,CinemaId,ScreeningDate,NumberOfSeats,TicketPrice")] Screening screening)
        {
            if (ModelState.IsValid)
            {
                _context.Add(screening);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Set<Movie>(), "Id", "Title");
            ViewData["CinemaId"] = new SelectList(_context.Set<Cinema>(), "Id", "Name");
            return View(screening);
        }

        // GET: Screenings/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = await _context.Screenings.FindAsync(id);
            if (screening == null)
            {
                return NotFound();
            }
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name", screening.CinemaId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", screening.MovieId);
            return View(screening);
        }

        [Authorize(Roles = "Admin")]
        // POST: Screenings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScreeningId,MovieId,CinemaId,ScreeningDate,NumberOfSeats,TicketPrice")] Screening screening)
        {
            if (id != screening.ScreeningId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(screening);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScreeningExists(screening.ScreeningId))
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
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Title", screening.CinemaId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Name", screening.MovieId);
            return View(screening);
        }

        [Authorize(Roles = "Admin")]
        // GET: Screenings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = await _context.Screenings
                .Include(s => s.Cinema)
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.ScreeningId == id);
            if (screening == null)
            {
                return NotFound();
            }

            return View(screening);
        }

        [Authorize(Roles = "Admin")]
        // POST: Screenings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var screening = await _context.Screenings.FindAsync(id);
            if (screening != null)
            {
                _context.Screenings.Remove(screening);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScreeningExists(int id)
        {
            return _context.Screenings.Any(e => e.ScreeningId == id);
        }
    }
}
