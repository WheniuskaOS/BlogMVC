using System.ComponentModel.DataAnnotations;

namespace BlogMVC.Models;

public class LoginViewModel
{
    // E-posta adresi
    [Required(ErrorMessage = "E-posta adresi zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
    [Display(Name = "E-posta Adresi")]
    public string Email { get; set; } = string.Empty;

    // Parola
    [Required(ErrorMessage = "Parola zorunludur.")]
    [DataType(DataType.Password)]
    [Display(Name = "Parola")]
    public string Password { get; set; } = string.Empty;

    // Beni hatırla
    [Display(Name = "Beni Hatırla")]
    public bool RememberMe { get; set; }

    // Giriş sonrası yönlendirilecek adres
    public string? ReturnUrl { get; set; }
}