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

namespace Eshop_HoangAnhHuy.Controllers
{
    public class AccountsController : Controller
    {
        private readonly EshopContext _context;

        public AccountsController(EshopContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View(await _context.Accounts.ToListAsync());
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            string username = HttpContext.Session.GetString("Username");
            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Username == username);
            ViewData["Invoices"] = _context.Invoices.Where(i => i.AccountId == account.Id).ToList();
            //ViewData["InvoicesDetails"] = _context.InvoiceDetails.Where
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var account = await _context.Accounts
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (account == null)
            //{
            //    return NotFound();
            //}

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,Email,Phone,Address,FullName,IsAdmin,Avatar,Status")] Account account, IFormFile image)
        {
            if (image != null && image.Length >0)
            {
                account.Avatar = account.Username + ".jpg";
                string filename = account.Avatar;
                string urlImage = "wwwroot/Images/avatar/" + filename;
                image.CopyTo(new FileStream(urlImage, FileMode.Create));
            }
            else
            {
                account.Avatar = "default.jpg";
            }
            if (ModelState.IsValid)
            {
                account.IsAdmin = false;
                account.Status = true;
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Products");
            }
            return View(account);
            //avc
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Email,Phone,Address,FullName,Status")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    account.IsAdmin = false;
                    account.Status = true;
                    account.Avatar = account.Username + ".jpg";
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details));
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }

        public IActionResult Login()
        {
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            bool result = _context.Accounts.Any(acc => acc.Username == username && acc.Password == password);
            if (result)
            {
                //HttpContext.Response.Cookies.Append("Name", username, new CookieOptions{
                //    Expires = DateTime.Today.AddDays(15)
                //});
                bool IsAdmin = _context.Accounts.Where(acc => acc.Username == username).FirstOrDefault().IsAdmin;
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetInt32("IsAdmin", Convert.ToInt32(IsAdmin));
                if (!IsAdmin)
                {
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    return RedirectToAction("Index", "Products", new { area = "Admin" });
                }
            }
            else
            {
                ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
                ViewBag.ErrorMsg = "Login failed!";
                return View();
            }
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Index", "Products");
        }
        public IActionResult SignUp()
        {
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(string username, string password, string email, string phone, string address, string fullname, bool isadmin, string avatar, bool status)
        {
            //ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            return RedirectToAction("Index", "Products");
        }
    }
}
