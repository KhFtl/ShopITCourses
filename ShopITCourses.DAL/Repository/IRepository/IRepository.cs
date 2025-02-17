using System.Linq.Expressions;

namespace ShopITCourses.DAL.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Find(int id);
        IEnumerable<T> GetAll(
                Expression<Func<T, bool>>? filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                string? includeProperties = null,
                bool isTracking = false
            );
        T FirstOrDefault(
                Expression<Func<T, bool>>? filter = null,
                string? includePreporties = null,
                bool isTracking = false
            );

        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
        void Save();
    }
}
