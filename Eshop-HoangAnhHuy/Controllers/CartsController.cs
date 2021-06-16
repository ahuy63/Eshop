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

namespace Eshop_HoangAnhHuy.Controllers
{
    public class CartsController : Controller
    {
        private readonly EshopContext _context;

        public CartsController(EshopContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            string username = HttpContext.Session.GetString("Username");
            int accountId = _context.Accounts.Where(acc => acc.Username == username).FirstOrDefault().Id;
            ViewBag.accountID = accountId;
            var eshopContext = _context.Carts.Include(c => c.Account).Include(c => c.Product).Where(c => c.AccountId == accountId);
            ViewBag.CartCount = eshopContext.Count();
            return View(await eshopContext.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Account)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,ProductId,Quantity")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cart.ProductId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,ProductId,Quantity")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                { 
                    if (!CartExists(cart.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var cart = await _context.Carts
        //        .Include(c => c.Account)
        //        .Include(c => c.Product)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (cart == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(cart);
        //}

        //// POST: Carts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            var cart = await _context.Carts.FindAsync(id);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult DeleteCart()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }
        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }

        public IActionResult Add(int id)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            int quant = Convert.ToInt32(Request.Form["quantity"]);
            if (quant == 0)
            {
                quant = 1;
            }
            string username = HttpContext.Session.GetString("Username");
            int accountId = _context.Accounts.Where(acc => acc.Username == username).FirstOrDefault().Id;
            Cart c = _context.Carts.Where(c => c.AccountId == accountId && c.ProductId == id).FirstOrDefault();
            if(c != null)
            {
                c.Quantity += quant;
                _context.Carts.Update(c);
            }
            else
            {
                _context.Carts.Add(new Cart { AccountId = accountId, ProductId = id, Quantity = quant });
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            string username = HttpContext.Session.GetString("Username");
            int accountId = _context.Accounts.Where(acc => acc.Username == username).FirstOrDefault().Id;
            Cart c = _context.Carts.Where(c => c.AccountId == accountId && c.ProductId == id).FirstOrDefault();
            c.Quantity = quantity;
            _context.Carts.Update(c);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
