using BlogMVC.Models;

namespace BlogMVC.Areas.Admin.Models;

public class DashboardViewModel
{
    // Veritabanındaki toplam yazı sayısı
    public int TotalPosts { get; set; }

    // Son eklenen 5 blog yazısı
    public List<BlogPost> RecentPosts { get; set; } = new();
}