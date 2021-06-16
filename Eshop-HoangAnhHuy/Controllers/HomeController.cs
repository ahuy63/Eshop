using Eshop_HoangAnhHuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Eshop_HoangAnhHuy.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Eshop_HoangAnhHuy.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly EshopContext _context;

        public HomeController(EshopContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            //ViewBag.Username = HttpContext.Session.GetString("Username") ?? "Guest";
            var eshopContext = _context.Products.Include(p => p.ProductType).OrderByDescending(p => p.Id).Take(3);
            return View(await eshopContext.ToListAsync());
            //return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
