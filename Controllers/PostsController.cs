using BlogMVC.Data;
using BlogMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View(new CreatePostViewModel());
    }

    // POST: /Posts/Create
    // Formdan gelen verileri veritabanına kaydet
    [HttpPost]
    [Authorize(Roles = "Admin")]
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

    // GET: /Posts/Edit/4
    // Düzenleme formunu aç
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        // Düzenlenecek yazıyı veritabanında bul
        var post = await _context.BlogPosts
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (post == null)
        {
            return NotFound();
        }

        // Verileri düzenleme modeline aktar
        var model = new EditPostViewModel
        {
            Id = post.Id,
            Title = post.Title,
            Summary = post.Summary,
            Content = post.Content
        };

        return View(model);
    }

    // POST: /Posts/Edit/4
    // Düzenlenen bilgileri kaydet
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        [FromRoute] int id,
        EditPostViewModel model)
    {
        // URL'deki ID ile formdaki ID uyuşuyor mu?
        if (id != model.Id)
        {
            return BadRequest();
        }

        // Form doğrulaması
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Mevcut yazıyı SQL Server'dan getir
        var post = await _context.BlogPosts.FindAsync(id);

        if (post == null)
        {
            return NotFound();
        }

        // Düzenlenen alanları güncelle
        post.Title = model.Title.Trim();
        post.Summary = model.Summary.Trim();
        post.Content = model.Content.Trim();

        // Değişiklikleri veritabanına kaydet
        await _context.SaveChangesAsync();

        // Başarı mesajı
        TempData["SuccessMessage"] =
            "Blog yazısı başarıyla güncellendi!";
        
        // Güncellenen yazının detayına dön
        return RedirectToAction(
            nameof(Details),
            new { id = post.Id }
        );
    }

    // GET: /Posts/Delete/4
    // Silinecek yazıyı ve onay sayfasını göster
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        // SQL Server'dan yazıyı getir
        var post = await _context.BlogPosts
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
        
        // Yazı bulunamadıysa 404 döndür
        if (post == null)
        {
            return NotFound();
        }

        // Silme onay sayfasını göster
        return View(post);
    }

    // POST: /Posts/Delete/4
    // Kullanıcı onay verirse yazıyı sil
    [HttpPost]
    [ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Silinecek yazıyı bul
        var post = await _context.BlogPosts.FindAsync(id);

        // Yazı bulunamadıysa 404 döndür
        if (post == null)
        {
            return NotFound();
        }

        // Yazıyı silinmek üzere işaretle
        _context.BlogPosts.Remove(post);

        // SQL Server'a değişikliği kaydet
        await _context.SaveChangesAsync();

        // Başarı mesajı
        TempData["SuccessMessage"] =
            "Blog yazısı başarıyla silindi!";
        
        // Blog yazıları listesine dön
        return RedirectToAction(nameof(Index));
    }
}

// GET ve POST arasındaki fark
// GET Create() Kullanıcıya boş formu gösterir.
// POST Create() Formdan gelen bilgileri alır ve kaydeder.
// ModelState.IsValid Kullanıcının girdiği verileri doğrular.
// _context.BlogPosts.Add(post) Yeni yazıyı EF Core'a ekler.
// SaveChangesAsync() Kaydı SQL Server'a yazar.

// GET Edit ve POST Edit arasındaki fark
// GET Edit(int id) Mevcut blog yazısını bulup formda gösterir.
// POST Edit(...) Düzenlenen bilgileri alır ve veritabanında günceller.
// ModelState.IsValid Form verilerinin geçerli olduğunu kontrol eder.
// FindAsync(id) Güncellenecek mevcut kaydı bulur.
// SaveChangesAsync() Değişiklikleri SQL Server'a kaydeder.

// Kodların görevleri
// [HttpGet] Silme onay sayfasını açar.
// [HttpPost] Onaylanmış silme isteğini işler.
// [ActionName("Delete")] DeleteConfirmed metodunu Delete eylemiyle eşleştirir.
// [ValidateAntiForgeryToken] CSRF korumasını uygular.
// FindAsync(id) SQL Server'dan ilgili yazıyı getirir.
// Remove(post) Kaydı silinmek üzere işaretler.
// SaveChangesAsync() Silme işlemini veritabanına uygular.