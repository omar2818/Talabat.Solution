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
        public ProductWithBrandAndCategorySpecifications()
            :base()
        {
            AddIncludes();

		}

        public ProductWithBrandAndCategorySpecifications(Expression<Func<Product, bool>> CriteriaExpression)
            : base(CriteriaExpression)
        {
            AddIncludes();

		}

        private void AddIncludes()
        {
			Includes.Add(P => P.Brand);
			Includes.Add(P => P.Category);
		}
    }
}
