using BlogMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogMVC.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    // Constructor
    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // GET: /Account/Login
    // Giriş sayfasını aç
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        // Kullanıcı zaten giriş yaptıysa Admin paneline git
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction(
                "Index",
                "Dashboard",
                new { area = "Admin" }
            );
        }

        var model = new LoginViewModel
        {
            ReturnUrl = returnUrl
        };

        return View(model);
    }

    // POST: /Account/Login
    // Kullanıcı giriş işlemi
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        // Form doğrulaması
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // E-posta adresine göre kullanıcıyı bul
        var user = await _userManager.FindByEmailAsync(
            model.Email.Trim()
        );

        // Kullanıcı bulunamadıysa hata göster
        if (user == null)
        {
            ModelState.AddModelError(
                string.Empty,
                "E-posta adresi veya parola hatalı."
            );

            return View(model);
        }

        // Identity ile giriş işlemi
        var result = await _signInManager.PasswordSignInAsync(
            user,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: true
        );

        // Giriş başarılı
        if (result.Succeeded)
        {
            // Güvenli yerel adrese yönlendir
            if (!string.IsNullOrWhiteSpace(model.ReturnUrl) &&
            Url.IsLocalUrl(model.ReturnUrl))
            {
                return LocalRedirect(model.ReturnUrl);
            }

            // Varsayılan olarak Admin paneline git
            return RedirectToAction(
                "Index",
                "Dashboard",
                new { area = "Admin" }
            );
        }

        // Hesap kilitlenmiş mi?
        if (result.IsLockedOut)
        {
            ModelState.AddModelError(
                string.Empty,
                "Hesabınız çok fazla başarısız giriş nedeniyle geçici olarak kilitlendi."
            );
        }
        else
        {
            ModelState.AddModelError(
                string.Empty,
                "E-posta adresi veya parola hatalı."
            );
        }

        return View(model);
    }

    // POST: /Account/Logout
    // Kullanıcı çıkış işlemi
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        // Kullanıcının oturumunu kapat
        await _signInManager.SignOutAsync();

        // Ana sayfaya dön
        return RedirectToAction(
            "Index",
            "Home"
        );
    }

    // GET: /Account/AccessDenied
    // Yetkisiz erişim sayfası
    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }
}