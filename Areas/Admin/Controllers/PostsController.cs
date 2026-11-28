using BlogMVC.Data;
using BlogMVC.Models;
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

    // GET: /Admin/Posts/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreatePostViewModel());
    }

    // POST: /Admin/Posts/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePostViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var post = new BlogPost
        {
            Title = model.Title.Trim(),
            Summary = model.Summary.Trim(),
            Content = model.Content.Trim(),
            PublishedAt = DateTime.Now
        };

        _context.BlogPosts.Add(post);

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Blog yazısı başarıyla eklendi.";

        return RedirectToAction(nameof(Index));
    }
}