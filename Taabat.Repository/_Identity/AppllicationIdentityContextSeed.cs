using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository._Identity
{
    public static class AppllicationIdentityContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            if (! userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    DisplayName = "Omar Wael",
                    Email = "Omar.Wael@gmail.com",
                    UserName = "Omar.wael",
                    PhoneNumber = "01552115395"
                };

                var user2 = new ApplicationUser()
                {
                    DisplayName = "Ahmed Nasr",
                    Email = "Ahmed.nasr@linkdev.com",
                    UserName = "Ahmed.nasr",
                    PhoneNumber = "01122334455"
                }; 

                await userManager.CreateAsync(user, "P@ssw0rd");
                await userManager.CreateAsync(user2, "Pa$$w0rd");
            }
        }
    }
}
