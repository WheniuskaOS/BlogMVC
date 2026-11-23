using BlogMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class PostsController : Controller
{
    private readonly BlogDbContext _context;

    public PostsController(BlogDbContext context)
    {
        _context = context;
    }

    // GET: /Admin/Posts
    public async Task<IActionResult> Index()
    {
        var posts = await _context.BlogPosts
            .AsNoTracking()
            .OrderByDescending(p => p.PublishedAt)
            .ThenByDescending(p => p.Id)
            .ToListAsync();
        
        return View(posts);
    }
}