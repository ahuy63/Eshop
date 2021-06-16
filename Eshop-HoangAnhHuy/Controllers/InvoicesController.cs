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
    public class InvoicesController : Controller
    {
        private readonly EshopContext _context;

        public InvoicesController(EshopContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var eshopContext = _context.Invoices.Include(i => i.Account);
            return View(await eshopContext.ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create(int id)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewData["Account"] = _context.Accounts.Where(acc => acc.Id == id).FirstOrDefault();
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            ViewBag.Total = _context.Carts.Where(c => c.AccountId == id).Select(c => c.Product.Price * c.Quantity).Sum();
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Code,AccountId,IssuedDate,ShippingAddress,ShippingPhone,Total,Status")] Invoice invoice)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewData["Account"] = _context.Accounts.Where(acc => acc.Id == invoice.AccountId);
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password", invoice.AccountId);
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            ViewData["Cart"] = _context.Carts.Include(c => c.Account).Include(c => c.Product).Where(c => c.AccountId == invoice.AccountId);
            if (ModelState.IsValid)
            {
                invoice.Total = _context.Carts.Where(c => c.AccountId == invoice.AccountId).Select(c => c.Product.Price * c.Quantity).Sum();
                invoice.IssuedDate = DateTime.Now;
                invoice.Code = invoice.IssuedDate.ToString().Trim();
                invoice.Status = true;
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                var ctrl = new InvoiceDetailsController(_context);
                foreach (Cart item in ViewBag.Cart)
                {
                   ctrl.CreateAsync(item, invoice.Id);
                   _context.Carts.Remove(item);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Products");

            }
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password", invoice.AccountId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,AccountId,IssuedDate,ShippingAddress,ShippingPhone,Total,Status")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password", invoice.AccountId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
