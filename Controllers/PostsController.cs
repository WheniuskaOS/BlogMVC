using BlogMVC.Data;
using BlogMVC.Models;
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
    // Bütün blog yazılarını listele
    public async Task<IActionResult> Index()
    {
        var posts = await _context.BlogPosts
            .AsNoTracking()
            .OrderByDescending(p => p.PublishedAt)
            .ToListAsync();
        
        return View(posts);
    }

    // GET: /Posts/Details/1
    // Seçilen blog yazısının detaylarını göster
    public async Task<IActionResult> Details(int id)
    {
        var post = await _context.BlogPosts
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    // GET: /Posts/Create
    // Yeni yazı ekleme formunu aç
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreatePostViewModel());
    }

    // POST: /Posts/Create
    // Formdan gelen verileri veritabanına kaydet
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreatePostViewModel model)
    {
        // Form verileri geçerli mi?
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Yeni blog yazısı oluştur
        var post = new BlogPost
        {
            Title = model.Title.Trim(),
            Summary = model.Summary.Trim(),
            Content = model.Content.Trim(),
            PublishedAt = DateTime.Now
        };

        // Veritabanına ekle
        _context.BlogPosts.Add(post);

        // SQL Server'a kaydet
        await _context.SaveChangesAsync();

        // Başarı mesajı
        TempData["SuccessMessage"] =
            "Blog yazısı başarıyla eklendi!";
        
        // Yeni oluşturulan yazının detayına git
        return RedirectToAction(
            nameof(Details),
            new { id = post.Id }
        );
    }
}

// GET ve POST arasındaki fark
// GET Create() Kullanıcıya boş formu gösterir.
// POST Create() Formdan gelen bilgileri alır ve kaydeder.
// ModelState.IsValid Kullanıcının girdiği verileri doğrular.
// _context.BlogPosts.Add(post) Yeni yazıyı EF Core'a ekler.
// SaveChangesAsync() Kaydı SQL Server'a yazar.