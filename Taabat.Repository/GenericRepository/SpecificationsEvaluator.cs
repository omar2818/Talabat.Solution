﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository.GenericRepository
{
    internal static class SpecificationsEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> spec)
        {
            var query = inputQuery;

            if (spec?.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec?.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec?.OrderByDesc is not null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            if (spec?.IsPaginationEnabled ?? false)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            query = spec?.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression))
                        ?? query;

            return query;
        }
    }
}
