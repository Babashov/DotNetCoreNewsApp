using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsManagementDotNetCoreApp.Data;
using NewsManagementDotNetCoreApp.Models;
using NewsManagementDotNetCoreApp.StaticFolder;

namespace NewsManagementDotNetCoreApp.Controllers
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class PostPlacesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostPlacesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PostPlaces
        public async Task<IActionResult> Index()
        {
            return View(await _context.PostPlaces.ToListAsync());
        }

        // GET: PostPlaces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postPlace = await _context.PostPlaces
                .SingleOrDefaultAsync(m => m.ID == id);
            if (postPlace == null)
            {
                return NotFound();
            }

            return View(postPlace);
        }

        // GET: PostPlaces/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PostPlaces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] PostPlace postPlace)
        {
            if (ModelState.IsValid)
            {
                _context.Add(postPlace);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(postPlace);
        }

        // GET: PostPlaces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postPlace = await _context.PostPlaces.SingleOrDefaultAsync(m => m.ID == id);
            if (postPlace == null)
            {
                return NotFound();
            }
            return View(postPlace);
        }

        // POST: PostPlaces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] PostPlace postPlace)
        {
            if (id != postPlace.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postPlace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostPlaceExists(postPlace.ID))
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
            return View(postPlace);
        }

        // GET: PostPlaces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postPlace = await _context.PostPlaces
                .SingleOrDefaultAsync(m => m.ID == id);
            if (postPlace == null)
            {
                return NotFound();
            }

            return View(postPlace);
        }

        // POST: PostPlaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var postPlace = await _context.PostPlaces.SingleOrDefaultAsync(m => m.ID == id);
            _context.PostPlaces.Remove(postPlace);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostPlaceExists(int id)
        {
            return _context.PostPlaces.Any(e => e.ID == id);
        }
    }
}
