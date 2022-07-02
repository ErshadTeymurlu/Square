using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Square.DAL;
using Square.Models;
using Square.ViewModels;
using System.Diagnostics;

namespace Square.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _db { get; }
        public HomeController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            HomeViewModel hvm = new HomeViewModel()
            {
                Workers = await _db.Workers.ToListAsync()
            };
            return View(hvm);
        }
    }
}