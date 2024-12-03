using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Infrastructure.Data;
using Domain.Entities;

namespace Application.Services
{
    public class SeedAdmin
    {
        
        public static async Task Initialize(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@plasmac.it";
            string adminPassword = "Plasmac@2017";
            string adminRole = "Admin";
            string name = "P";
            string surname = "M";
            string company = "Plasmac";
            string phonenumber = "0331341813";


            // Creazione del ruolo Admin se non esiste
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // Creazione dell'utente Admin se non esiste
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail, Name = name, Surname = surname, Company=company, PhoneNumber = phonenumber  };
                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }
        
    }
}
