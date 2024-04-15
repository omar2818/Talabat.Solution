using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data
{
	public static class StoreContextSeed
	{
		public async static Task SeedAsync(StoreContext context)
		{
			if (!context.ProductBrands.Any())
			{
				var brandsData = await File.ReadAllTextAsync("../Taabat.Repository/Data/DataSeed/brands.json");
				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
				if (brands?.Count > 0)
				{
					foreach (var brand in brands)
					{
						context.Set<ProductBrand>().Add(brand);
					}
				} 
			}

			if (!context.ProductCategories.Any())
			{
				var categoriesData = await File.ReadAllTextAsync("../Taabat.Repository/Data/DataSeed/categories.json");
				var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);
				if (categories?.Count > 0)
				{
					foreach (var category in categories)
					{
						context.Set<ProductCategory>().Add(category);
					}
				}
			}

			if (!context.Products.Any())
			{
				var productsData = await File.ReadAllTextAsync("../Taabat.Repository/Data/DataSeed/products.json");
				var products = JsonSerializer.Deserialize<List<Product>>(productsData);
				if (products?.Count > 0)
				{
					foreach (var product in products)
					{
						context.Set<Product>().Add(product);
					}
				}
			}

			await context.SaveChangesAsync();
		}
	}
}
