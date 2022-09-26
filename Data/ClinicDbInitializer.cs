
using ClinicApi.Data.Helpers;
using Microsoft.AspNetCore.Identity;

namespace ClinicApi.Data
{
    public class ClinicDbInitializer
    {
        public static async Task SeedRolesToDb(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                //check if role does not exist in database to generate new role

                //Doctor role exists
                if(!await roleManager.RoleExistsAsync(UserRoles.Doctor))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Doctor));
                }

                //Admin role exists
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                }

                //Patient role exists
                if (!await roleManager.RoleExistsAsync(UserRoles.Patient))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Patient));
                }
            }
        }
    }
}
