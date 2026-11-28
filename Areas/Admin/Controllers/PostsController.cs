using BlogMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BlogMVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
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