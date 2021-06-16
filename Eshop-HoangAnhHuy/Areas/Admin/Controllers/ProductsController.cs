using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop_HoangAnhHuy.Data;
using Eshop_HoangAnhHuy.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Eshop_HoangAnhHuy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly EshopContext _context;

        public ProductsController(EshopContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index(int? id)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id","Name");
            if (id != null)
            {
                var eshopContext = _context.Products.Include(p => p.ProductType).Where(p => p.ProductTypeId == id);
                return View(await eshopContext.ToListAsync());
            }
            else
            {
                var eshopContext = _context.Products.Include(p => p.ProductType);
                return View(await eshopContext.ToListAsync());
            }
        }

        // GET: Admin/Products/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.ProductType)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username") ?? "Guest";
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SKU,Name,Description,Price,Stock,ProductTypeId,Image,Status")] Product product, IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                product.Image = product.SKU + ".jpg";
                string filename = product.Image;
                string urlImage = "wwwroot/Images/product/" + filename;
                image.CopyTo(new FileStream(urlImage, FileMode.Create));
            }
            else
            {
                product.Image = "default.jpg";
            }
            ViewBag.Username = HttpContext.Session.GetString("Username") ?? "Guest";
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", product.ProductTypeId);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username") ?? "Guest";
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", product.ProductTypeId);
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SKU,Name,Description,Price,Stock,ProductTypeId,Image,Status")] Product product)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username") ?? "Guest";
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
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
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", product.ProductTypeId);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.ProductType)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //// POST: Admin/Products/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username") ?? "Guest";
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username") ?? "Guest";
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
