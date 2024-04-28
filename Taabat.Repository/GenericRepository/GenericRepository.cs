using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Repository.GenericRepository.Data;

namespace Talabat.Repository.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbcontext;

        public GenericRepository(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if(typeof(T) == typeof(Product))
            //{
            //	return (IEnumerable<T>) await _dbcontext.Products.Include(P => P.Brand).Include(P => P.Category).ToListAsync();
            //}
            return await _dbcontext.Set<T>().AsNoTracking().ToListAsync();
        }
        public async Task<T?> GetAsync(int id)
        {
            //if (typeof(T) == typeof(Product))
            //{
            //	return await _dbcontext.Products.Where(P => P.Id == id).Include(P => P.Brand).Include(P => P.Category).FirstOrDefaultAsync() as T;
            //}
            return await _dbcontext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).AsNoTracking().ToListAsync();
        }


        public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecifications<T> spec)
        {
            return SpecificationsEvaluator<T>.GetQuery(_dbcontext.Set<T>(), spec);
        }

    }
}
