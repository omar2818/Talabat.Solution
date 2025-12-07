using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.OrderModule;
using DomainLayer.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _dbContext,
                             UserManager<ApplicationUser> _userManager,
                             RoleManager<IdentityRole> _roleManager) : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            try
            {
                if ((await _dbContext.Database.GetPendingMigrationsAsync()).Any())
                {
                    await _dbContext.Database.MigrateAsync();
                }

                if (!_dbContext.ProductBrands.Any())
                {
                    //var productBrandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Presistence\Data\DataSeeding\brands.json");
                    var productBrandsData = File.OpenRead(@"..\Infrastructure\Presistence\Data\DataSeeding\brands.json");
                    var productBrands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(productBrandsData);
                    if (productBrands is not null && productBrands.Any())
                    {
                        await _dbContext.ProductBrands.AddRangeAsync(productBrands);
                    }
                }
                if (!_dbContext.ProductTypes.Any())
                {
                    var productTypesData = File.OpenRead(@"..\Infrastructure\Presistence\Data\DataSeeding\types.json");
                    var productTypes = await JsonSerializer.DeserializeAsync<List<ProductType>>(productTypesData);
                    if (productTypes is not null && productTypes.Any())
                    {
                        await _dbContext.ProductTypes.AddRangeAsync(productTypes);
                    }
                }
                if (!_dbContext.Products.Any())
                {
                    var productsData = File.OpenRead(@"..\Infrastructure\Presistence\Data\DataSeeding\products.json");
                    var products = await JsonSerializer.DeserializeAsync<List<Product>>(productsData);
                    if (products is not null && products.Any())
                    {
                        await _dbContext.Products.AddRangeAsync(products);
                    }
                }
                if (!_dbContext.Set<DeliveryMethod>().Any())
                {
                    var DeliveryMethodsData = File.OpenRead(@"..\Infrastructure\Presistence\Data\DataSeeding\delivery.json");
                    var DeliveryMethods = await JsonSerializer.DeserializeAsync<List<DeliveryMethod>>(DeliveryMethodsData);
                    if (DeliveryMethods is not null && DeliveryMethods.Any())
                    {
                        await _dbContext.Set<DeliveryMethod>().AddRangeAsync(DeliveryMethods);
                    }
                }

                await _dbContext.SaveChangesAsync();
            }catch(Exception ex)
            {
                // To Do
            }
        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    var role01 = new IdentityRole("Admin");
                    var role02 = new IdentityRole("SuperAdmin");

                    await _roleManager.CreateAsync(role01);
                    await _roleManager.CreateAsync(role02);
                }

                if (!_userManager.Users.Any())
                {
                    var user01 = new ApplicationUser()
                    {
                        Email = "Mohamed@gmail.com",
                        DisplayName = "Mohamed Aly",
                        PhoneNumber = "01234567891",
                        UserName = "MohamedAly"
                    };
                    var user02 = new ApplicationUser()
                    {
                        Email = "Salma@Route.com",
                        DisplayName = "Salma Mohamed",
                        PhoneNumber = "01298765431",
                        UserName = "SalmaMohamed"
                    };

                    await _userManager.CreateAsync(user01, "P@ssw0rd");
                    await _userManager.CreateAsync(user02, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(user01, "Admin");
                    await _userManager.AddToRoleAsync(user02, "SuperAdmin");
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
