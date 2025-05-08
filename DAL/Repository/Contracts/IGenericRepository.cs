using DAL.Entities;
using System.Linq.Expressions;


namespace DAL.Repository.Contracts
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<long> AddAsync(T entity);
        Task DeleteAsync(long id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(long id);
        Task<IEnumerable<T>> GetManyWithFilterAsync(Expression<Func<T, bool>> expression);
        Task<T> GetOneWithFilterAsync(Expression<Func<T, bool>> expression);
        Task UpdateAsync(T entity);
    }

}
