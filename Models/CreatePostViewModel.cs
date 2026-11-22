using System.ComponentModel.DataAnnotations;

namespace BlogMVC.Models;

public class CreatePostViewModel
{
    [Required(ErrorMessage = "Başlık zorunludur.")]
    [StringLength(150, ErrorMessage = "Başlık en fazla 150 karakter olabilir.")]
    [Display(Name = "Blog Başlığı")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kısa açıklama zorunludur.")]
    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
    [Display(Name = "Kısa Açıklama")]
    public string Summary { get; set; } = string.Empty;

    [Required(ErrorMessage = "Yazı içeriği zorunludur.")]
    [StringLength(10000, ErrorMessage = "İçerik en fazla 10.000 karakter olabilir.")]
    [Display(Name = "Yazı İçeriği")]
    public string Content { get; set; } = string.Empty;
}