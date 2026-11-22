namespace BlogMVC.Models;

public class BlogPost
{
    // Blog yazısının benzersiz kimliği
    public int Id { get; set; }

    // Blog yazısının başlığı
    public string Title { get; set; } = string.Empty;

    // Kısa açıklama
    public string Summary { get; set; } = string.Empty;

    // Yazının tam içeriği
    public string Content { get; set; } = string.Empty;

    // Yayımlanma tarihi
    public DateTime PublishedAt { get; set; }
}