using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopITCourses.DAL.Repository.IRepository;
using ShopITCourses.Data;
using ShopITCourses.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ShopITCourses.Controllers
{
    [Authorize(Policy = "AdminManager")]//Доступ на основі політики доступу
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductRepository _prodRepo;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IProductRepository prodRepo)
        {
            _db = context;
            _webHostEnvironment = webHostEnvironment;
            _prodRepo = prodRepo;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var products = await _db.Product.Include(p => p.Category).ToListAsync();
            return View(products);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _db.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            //ViewData["CategoryId"] = new SelectList(_db.Category, "Id", "CategoryName");
            //ViewData["CategoryId"] = _prodRepo.GetAllDropDownList("Category");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Image,CategoryId")] Product product)
        {
            if (HttpContext.Request.Form.Files.Count == 0)
            {
                ModelState.AddModelError("Image", "Оберіть файл з картинкою");
            }
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;

                //var path = _webHostEnvironment.WebRootPath + WC.ImagePath;

                var path = Path.Combine(_webHostEnvironment.WebRootPath,"images","product");

                var fileName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(files[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(path, fileName + extension), FileMode.Create))
                {
                    await files[0].CopyToAsync(fileStream);
                }
                product.Image = fileName + extension;
                _db.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_db.Category, "Id", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _db.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_db.Category, "Id", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Image,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Product oldProduct = await _db.Product.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                    var files = HttpContext.Request.Form.Files;
                    product.Image = oldProduct.Image;
                    if (files.Count > 0)
                    { 
                        string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
                        if (oldProduct != null && oldProduct.Image != null)
                        { 
                            var oldFile = Path.Combine(upload, oldProduct.Image);
                            if(System.IO.File.Exists(oldFile))
                            {
                                System.IO.File.Delete(oldFile);
                            }
                        }
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            await files[0].CopyToAsync(fileStream);
                        }
                        product.Image = fileName + extension;
                    }
                    _db.Update(product);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_db.Category, "Id", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _db.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _db.Product.FindAsync(id);
            if (product != null)
            {
                var file = Path.Combine(_webHostEnvironment.WebRootPath + WC.ImagePath , product.Image);
                try
                {
                    System.IO.File.Delete(file);
                }
                catch { }
                _db.Product.Remove(product);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _db.Product.Any(e => e.Id == id);
        }
    }
}
