using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsManagementDotNetCoreApp.Data;
using NewsManagementDotNetCoreApp.Models;
using NewsManagementDotNetCoreApp.Models.SubCategoryViewModels;
using NewsManagementDotNetCoreApp.StaticFolder;

namespace NewsManagementDotNetCoreApp.Controllers
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class SubCategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;

        [TempData]
        public string StatusMessage { get; set; }

        public SubCategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var SubCategories = _db.SubCategories.Include(m => m.Category);

            return View(await SubCategories.ToListAsync());
        }

        // GET Create Method
        public IActionResult Create()
        {
            var model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = _db.Categories.ToList(),
                SubCategory = new SubCategory(),
                SubCategoryList = _db.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExists = _db.SubCategories.Where(s => s.Name == model.SubCategory.Name).Count();
                var doesSubCatAndCatExists = _db.SubCategories.Where(s => s.Name == model.SubCategory.Name && s.CategoryId == model.SubCategory.CategoryId).Count();


                if (doesSubCategoryExists > 0)
                {
                    //error
                    StatusMessage = "Error : Sub Category Name already Exists";
                }
                else
                {
                    _db.Add(model.SubCategory);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = _db.Categories.ToList(),
                SubCategory = model.SubCategory,
                SubCategoryList = _db.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).ToList(),
                StatusMessage = StatusMessage
            };
            return View(modelVM);

        }


        //GET Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _db.SubCategories.SingleOrDefaultAsync(m => m.ID == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = _db.Categories.ToList(),
                SubCategory = subCategory,
                SubCategoryList = _db.SubCategories.Select(p => p.Name).Distinct().ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExists = _db.SubCategories.Where(s => s.Name == model.SubCategory.Name).Count();
                var doesSubCatAndCatExists = _db.SubCategories.Where(s => s.Name == model.SubCategory.Name && s.CategoryId == model.SubCategory.CategoryId).Count();

                if (doesSubCategoryExists > 0)
                {
                    StatusMessage = "Error : Category and Sub Category combination already exists.";
                }
                else
                {
                    var subCatFromDb = _db.SubCategories.Find(id);
                    subCatFromDb.Name = model.SubCategory.Name;
                    subCatFromDb.CategoryId = model.SubCategory.CategoryId;
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = _db.Categories.ToList(),
                SubCategory = model.SubCategory,
                SubCategoryList = _db.SubCategories.Select(p => p.Name).Distinct().ToList(),
                StatusMessage = StatusMessage
            };
            return View(modelVM);
        }


        //GET Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subCategory = await _db.SubCategories.Include(s => s.Category).SingleOrDefaultAsync(m => m.ID == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }


        //GET Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subCategory = await _db.SubCategories.Include(s => s.Category).SingleOrDefaultAsync(m => m.ID == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        //POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCategory = await _db.SubCategories.SingleOrDefaultAsync(m => m.ID == id);
            _db.SubCategories.Remove(subCategory);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


    }
}