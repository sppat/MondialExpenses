using Microsoft.AspNetCore.Identity;

namespace MondialExpenses.Data
{
    public class IdentitySeeder
    {
        public static async void Seed(IServiceProvider serviceProvider)
        {
            serviceProvider = serviceProvider.CreateScope().ServiceProvider;

            UserManager<IdentityUser> userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = new string[]
            {
                "Admin",
                "User",
            };

            foreach (string role in roles)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            if (await userManager.FindByNameAsync("spyros") == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "spyros",
                    Email = "spyros@mondial.com"
                };

                IdentityResult result = await userManager.CreateAsync(user, "P@ssw0rd");
            
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roles[0]);
                }
            }
        }
    }
}
