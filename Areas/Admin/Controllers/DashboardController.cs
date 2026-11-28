using BlogMVC.Areas.Admin.Models;
using BlogMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BlogMVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly BlogDbContext _context;

    // Veritabanı bağlantısı
    public DashboardController(BlogDbContext context)
    {
        _context = context;
    }

    // GET: /Admin
    public async Task<IActionResult> Index()
    {
        var model = new DashboardViewModel
        {
            // Toplam yazı sayısı
            TotalPosts = await _context.BlogPosts.CountAsync(),

            // Son 5 blog yazısı
            RecentPosts = await _context.BlogPosts
                .AsNoTracking()
                .OrderByDescending(p => p.PublishedAt)
                .ThenByDescending(p => p.Id)
                .Take(5)
                .ToListAsync()
        };

        return View(model);
    }
}