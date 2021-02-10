using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MovieCatalog.Data;

namespace MovieCatalog.Models
{
    // инициализатор базы данных MoviesCatalog
    public class DataInitializer
    {
        public async static Task InitializeAsync(
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager, 
            MoviesContext context)
        {
            string userName = "admin";
            string password = "123";

            if(await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            if(await userManager.FindByNameAsync(userName) == null)
            {
                var user = new User() { UserName = userName };
                IdentityResult result = await userManager.CreateAsync(user, password);

                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");

                    context.Movies.AddRange(
                        new Movie {
                            Name = "Семь нянек", 
                            ProductionYear = 1962, 
                            User = user                    
                        }, 

                        new Movie {
                            Name = "Нечто", 
                            ProductionYear = 1982, 
                            User = user 
                        }, 

                        new Movie {
                            Name = "Отец солдата", 
                            ProductionYear = 1964, 
                            User = user 
                        }, 

                        new Movie {
                            Name = "Голый пистолет", 
                            ProductionYear = 1988, 
                            User = user 
                        }, 

                        new Movie {
                            Name = "Городской охотник", 
                            ProductionYear = 1992, 
                            User = user 
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}