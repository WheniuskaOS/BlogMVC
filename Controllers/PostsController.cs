using Microsoft.AspNetCore.Mvc;
using BlogMVC.Models;

namespace BlogMVC.Controllers;

public class PostsController : Controller
{
    // Örnek blog yazılarımız
    private static readonly List<BlogPost> Posts = new()
    {
        new BlogPost
        {
            Id = 1,
            Title = "C# Programlama Dili",
            Summary = "C# dilinin temellerini öğreniyoruz.",
            Content = "C# modern ve nesne yönelimli bir programlama dilidir. Microsoft tarafından geliştirilmiştir, Web, masaüstü ve farklı uygulama türlerinde kullanılır.",
            PublishedAt = new DateTime(2026, 11, 22)
        },

        new BlogPost
        {
            Id = 2,
            Title = "ASP.NET Core MVC Nedir?",
            Summary = "MVC mimarisini yakından tanıyalım.",
            Content = "ASP.NET Core MVC, web uygulamaları geliştirmek için kullanılan bir framework yapısıdır. Model, View ve Controller bileşenlerinden oluşur.",
            PublishedAt = new DateTime(2026, 11, 21)
        },

        new BlogPost
        {
            Id = 3,
            Title = "SQL Server'a Giriş",
            Summary = "Veritabanı dünyasına ilk adım.",
            Content = "SQL Server, Microsoft tarafından geliştirilen ilişkisel bir veritabanı yönetim sistemidir. Verileri saklamak, sorgulamak ve yönetmek için kullanılır.",
            PublishedAt = new DateTime(2026, 11, 20)
        }
    };

    // GET: /Posts
    public IActionResult Index()
    {
        return View(Posts);
    }

    // GET: /Posts/Details/1
    public IActionResult Details(int id)
    {
        // ID değerine göre blog yazısını bul
        var post = Posts.FirstOrDefault(p => p.Id == id);

        // Yazı bulunamazsa HTTP 404 döndür
        if (post == null)
        {
            return NotFound();
        }

        // Bulunan yazıyı Details sayfasına gönder
        return View(post);
    }
}