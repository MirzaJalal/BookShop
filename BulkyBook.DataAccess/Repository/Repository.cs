using BookShop.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;

        internal DbSet<T> dbSet; //to use DbContext db set
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity); //(dbSet) simplifies (_db.Categories).add()
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet; //as we will make query
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filer)
        {
            IQueryable<T> query = dbSet;

            query = query.Where(filer);

            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
