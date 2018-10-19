using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsManagementDotNetCoreApp.Data;
using NewsManagementDotNetCoreApp.Models;
using NewsManagementDotNetCoreApp.Models.PostViewModels;
using NewsManagementDotNetCoreApp.StaticFolder;

namespace NewsManagementDotNetCoreApp.Controllers
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class PostsController : Controller
    {        
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnviroment;

        [BindProperty]
        public PostViewModel PostViewModelVM { get; set; }

        public PostsController(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;

            _hostingEnviroment = hostingEnvironment;

            PostViewModelVM = new PostViewModel()
            {
                Category = _db.Categories.ToList(),
                PostPlace = _db.PostPlaces.ToList(),
                Post = new Post()
            };
        }

        public async Task<IActionResult> Index()
        {
            var posts = _db.Posts.Include(m => m.Category).Include(m => m.SubCategory).Include(m => m.PostPlace);
            return View(await posts.ToListAsync());
        }






        // GET: Post Create
        public IActionResult Create() => View(PostViewModelVM);

        // POST : Post Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            PostViewModelVM.Post.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());
            PostViewModelVM.Post.Video = "default_video";

            if (!ModelState.IsValid)
            {
                return View(PostViewModelVM);
            }

            _db.Posts.Add(PostViewModelVM.Post);
            await _db.SaveChangesAsync();

            //Image Being Saved
            string webRootPath = _hostingEnviroment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var postFromDb = _db.Posts.Find(PostViewModelVM.Post.ID);

            if (files[0] != null && files[0].Length > 0)
            {
                //when user uploads an image
                var uploads = Path.Combine(webRootPath, "images");
                var extension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."), files[0].FileName.Length - files[0].FileName.LastIndexOf("."));

                using (var filestream = new FileStream(Path.Combine(uploads, PostViewModelVM.Post.ID + extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                postFromDb.Image = @"\images\" + PostViewModelVM.Post.ID + extension;
            }
            else
            {
                //when user does not upload image
                var uploads = Path.Combine(webRootPath, @"images\" + SD.DefaultNewsImage);
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + PostViewModelVM.Post.ID + ".png");
                postFromDb.Image = @"\images\" + PostViewModelVM.Post.ID + ".png";
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        // Get JSON Result For SubCategories
        public JsonResult GetSubCategory(int CategoryId)
        {
            List<SubCategory> subCategoryList = new List<SubCategory>();

            subCategoryList = (from subCategory in _db.SubCategories
                               where subCategory.CategoryId == CategoryId
                               select subCategory).ToList();

            return Json(new SelectList(subCategoryList, "ID", "Name"));
        }

        //GET : Edit Posts
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PostViewModelVM.Post = await _db.Posts.Include(m=>m.Category).Include(m => m.SubCategory).Include(m=>m.PostPlace).SingleOrDefaultAsync(m => m.ID == id);
            PostViewModelVM.SubCategory = _db.SubCategories.Where(s => s.CategoryId == PostViewModelVM.Post.CategoryId).ToList();

            if (PostViewModelVM.Post == null)
            {
                return NotFound();
            }

            return View(PostViewModelVM);
        }



        //POST : Edit Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            PostViewModelVM.Post.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (id != PostViewModelVM.Post.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string webRootPath = _hostingEnviroment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;
                    var postFromDb = _db.Posts.Where(m => m.ID == PostViewModelVM.Post.ID).FirstOrDefault();

                    if (files[0].Length > 0 && files[0] != null)
                    {
                        //if user uploads a new image
                        var uploads = Path.Combine(webRootPath, "images");

                        var extension_New = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."), files[0].FileName.Length - files[0].FileName.LastIndexOf("."));

                        var extension_Old = postFromDb.Image.Substring(postFromDb.Image.LastIndexOf("."), postFromDb.Image.Length - postFromDb.Image.LastIndexOf("."));

                        if (System.IO.File.Exists(Path.Combine(uploads, PostViewModelVM.Post.ID + extension_Old)))
                        {
                            System.IO.File.Delete(Path.Combine(uploads, PostViewModelVM.Post.ID + extension_Old));
                        }
                        using (var filestream = new FileStream(Path.Combine(uploads, PostViewModelVM.Post.ID + extension_New), FileMode.Create))
                        {
                            files[0].CopyTo(filestream);
                        }
                        PostViewModelVM.Post.Image = @"\images\" + PostViewModelVM.Post.ID + extension_New;
                    }

                    if (PostViewModelVM.Post.Image != null)
                    {
                        postFromDb.Image = PostViewModelVM.Post.Image;
                    }
                    postFromDb.Name = PostViewModelVM.Post.Name;
                    postFromDb.Desc = PostViewModelVM.Post.Desc;
                    postFromDb.PostPlaceId = PostViewModelVM.Post.PostPlaceId;
                    postFromDb.CategoryId = PostViewModelVM.Post.CategoryId;
                    postFromDb.SubCategoryId = PostViewModelVM.Post.SubCategoryId;
                    await _db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                }
                return RedirectToAction(nameof(Index));
            }
            PostViewModelVM.SubCategory = _db.SubCategories.Where(s => s.CategoryId == PostViewModelVM.Post.CategoryId).ToList();
            return View(PostViewModelVM);

        }

        //GET : Details Posts
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PostViewModelVM.Post = await _db.Posts.Include(m => m.Category).Include(m => m.SubCategory).Include(m=>m.PostPlace).SingleOrDefaultAsync(m => m.ID == id);

            if (PostViewModelVM.Post == null)
            {
                return NotFound();
            }

            return View(PostViewModelVM);
        }


        //GET : Delete Post
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PostViewModelVM.Post = await _db.Posts.Include(m => m.Category).Include(m => m.SubCategory).Include(m=>m.PostPlace).SingleOrDefaultAsync(m => m.ID == id);

            if (PostViewModelVM.Post == null)
            {
                return NotFound();
            }

            return View(PostViewModelVM);
        }

        //POST Delete MenuItem
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = _hostingEnviroment.WebRootPath;
            Post postItem = await _db.Posts.FindAsync(id);

            if (postItem != null)
            {
                var uploads = Path.Combine(webRootPath, "images");
                var extension = postItem.Image.Substring(postItem.Image.LastIndexOf("."), postItem.Image.Length - postItem.Image.LastIndexOf("."));

                var imagePath = Path.Combine(uploads, postItem.ID + extension);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _db.Posts.Remove(postItem);
                await _db.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }


    }
}