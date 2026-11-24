using BlogMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.Data;

public class BlogDbContext : IdentityDbContext<IdentityUser>
{
    // Veritabanı bağlantı ayarlarını alır
    public BlogDbContext(
        DbContextOptions<BlogDbContext> options)
        : base(options)
    {
        
    }

    // BlogPosts tablosu
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();

    // İlk örnek yazılar
    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BlogPost>().HasData(
            new BlogPost
            {
                Id = 1,
                Title = "C# Programlama Dili",
                Summary = "C# dilinin temellerini öğreniyoruz.",
                Content = "C# modern ve nesne yönelimli bir programlama dilidir. Microsoft tarafından geliştirilmiştir. Web ve masaüstü uygulamalarında kullanılabilir.",
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
        );
    }
}