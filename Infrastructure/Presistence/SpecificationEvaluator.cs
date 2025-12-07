using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    static class SpecificationEvaluator
    {
        // Create Query
        // _dbContext.Products.Where(P => P.id == id).Include(P => P.ProductBrand).Include(P => P.ProductType)
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> InputQuery, ISpecification<TEntity, TKey> specification) where TEntity : BaseEntity<TKey>
        {
            // _dbContext.Products
            var Query = InputQuery;

            // .Where(P => P.id == id)
            if(specification.Criteria is not null)
            {
                Query = Query.Where(specification.Criteria);
            }

            if(specification.OrderBy is not null)
            {
                Query = Query.OrderBy(specification.OrderBy);
            }

            if(specification.OrderByDescending is not null)
            {
                Query = Query.OrderByDescending(specification.OrderByDescending);
            }

            // .Include(P => P.ProductBrand).Include(P => P.ProductType)
            if(specification.IncludeExpressions is not null && specification.IncludeExpressions.Any())
            {
                //foreach(var expression in specification.IncludeExpressions)
                //{
                //    Query = Query.Include(expression);
                //}

                Query = specification.IncludeExpressions.Aggregate(Query, (CurrentQuery, IncludeExp) => CurrentQuery.Include(IncludeExp));
            }

            if (specification.IsPaginated)
            {
                Query = Query.Skip(specification.Skip);
                Query = Query.Take(specification.Take);
            }

            return Query;
        }
    }
}
