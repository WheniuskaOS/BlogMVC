using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogMVC.Data;

public static class IdentitySeeder
{
    public static async Task SeedAdminAsync(
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        // Admin bilgilerini User Secrets üzerinden al
        var adminEmail =
            configuration["AdminAccount:Email"]?.Trim();
        
        var adminPassword =
            configuration["AdminAccount:Password"];
        
        // Bilgiler eksikse uygulamayı başlatma
        if (string.IsNullOrWhiteSpace(adminEmail) ||
            string.IsNullOrWhiteSpace(adminPassword))
        {
            throw new InvalidOperationException(
                "Admin e-posta ve parola bilgileri ayarlanmamış."
            );
        }

        // Identity servisleri
        var roleManager =
            serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        var userManager =
            serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        
        const string roleName = "Admin";

        // ADMIN ROLÜNÜ OLUŞTUR
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var roleResult = await roleManager.CreateAsync(
                new IdentityRole(roleName)
            );

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Admin rolü oluşturulamadı: " +
                    string.Join("; ",
                        roleResult.Errors.Select(e => e.Description))
                );
            }
        }

        // ADMIN KULLANICISINI BUL
        var adminUser =
            await userManager.FindByEmailAsync(adminEmail);
        
        // Kullanıcı yoksa oluştur
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail
            };

            var userResult = await userManager.CreateAsync(
                adminUser,
                adminPassword
            );

            if (!userResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Admin hesabı oluşturulamadı: " +
                    string.Join("; ",
                        userResult.Errors.Select(e => e.Description))
                );
            }
        }

        // ADMIN ROLÜNE ATA
        if (!await userManager.IsInRoleAsync(
            adminUser, roleName))
        {
            var assignmentResult =
                await userManager.AddToRoleAsync(
                    adminUser, roleName
                );
            
            if (!assignmentResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Admin rolü kullanıcıya atanamadı: " +
                    string.Join("; ",
                        assignmentResult.Errors.Select(e => e.Description))
                );
            }
        }

        Console.WriteLine(
            "Admin hesabı ve Admin rolü hazır."
        );
    }
}