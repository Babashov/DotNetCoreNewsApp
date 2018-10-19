using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsManagementDotNetCoreApp.Data;
using NewsManagementDotNetCoreApp.Models;
using NewsManagementDotNetCoreApp.Models.HomeViewModels;

namespace NewsManagementDotNetCoreApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel IndexVM = new IndexViewModel()
            {
                Post = await _db.Posts.Include(m => m.Category).Include(m => m.SubCategory).Include(m => m.PostPlace).ToListAsync(),
                Category = _db.Categories.OrderBy(c=>c.CatPlace),
                PostPlace = _db.PostPlaces.OrderBy(p=>p.ID)

            };

            return View(IndexVM);
        }

        public async Task<IActionResult> ReadPost(int ? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            IndexViewModel IndexVM = new IndexViewModel()
            {
                Post = await _db.Posts.Include(m => m.Category).Include(m => m.SubCategory).Include(m => m.PostPlace).ToListAsync(),
                PostPlace = _db.PostPlaces.OrderBy(p=>p.ID),
                Category = _db.Categories.OrderBy(c=>c.CatPlace)
            };

            return View(IndexVM);
        }

        public async Task<IActionResult> Category(int ? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            IndexViewModel IndexVM = new IndexViewModel()
            {
                Post = await _db.Posts.Include(m => m.Category).Include(m => m.SubCategory).Include(m => m.PostPlace).ToListAsync(),
                PostPlace = _db.PostPlaces.OrderBy(p=>p.ID),
                Category = _db.Categories.OrderBy(c=>c.ID)
            };

            return View(IndexVM);
        }

       

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
