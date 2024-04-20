using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Specs
{
	public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
	{
        public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams)
            :base(P =>
			    (string.IsNullOrEmpty(specParams.Search) || P.NormalizedName.Contains(specParams.Search)) &&
				(!specParams.BrandId.HasValue || specParams.BrandId.Value == P.BrandId) &&
				(!specParams.CategoryId.HasValue || specParams.CategoryId.Value == P.CategoryId)
			)
        {
            AddIncludes();

			AddSort(specParams.Sort);

			ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
		}

		public ProductWithBrandAndCategorySpecifications(int id)
            : base(P => P.Id == id)
        {
            AddIncludes();
		}

        private void AddSort(string? sort)
        {

			if (!string.IsNullOrEmpty(sort))
			{
				switch (sort)
				{
					case "priceAsc":
						AddOrderBy(P => P.Price);
						break;
					case "priceDesc":
						AddOrderByDesc(P => P.Price);
						break;
					default:
						AddOrderBy(P => P.Name);
						break;

				}
			}
			else
			{
				AddOrderBy(P => P.Name);
			}

		}

        private void AddIncludes()
        {

			Includes.Add(P => P.Brand);
			Includes.Add(P => P.Category);

		}
    }
}
