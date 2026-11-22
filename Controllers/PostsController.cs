using BlogMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.Controllers;

public class PostsController : Controller
{
    private readonly BlogDbContext _context;

    // Constructor - veritabanı bağlantısı
    public PostsController(BlogDbContext context)
    {
        _context = context;
    }

    // GET: /Posts
    public async Task<IActionResult> Index()
    {
        // Tüm yazıları en yeniden eskiye sırala
        var posts = await _context.BlogPosts
            .AsNoTracking()
            .OrderByDescending(p => p.PublishedAt)
            .ToListAsync();
        
        return View(posts);
    }

    // GET: /Posts/Details/1
    public async Task<IActionResult> Details(int id)
    {
        // ID'ye göre yazıyı getir
        var post = await _context.BlogPosts
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }
}