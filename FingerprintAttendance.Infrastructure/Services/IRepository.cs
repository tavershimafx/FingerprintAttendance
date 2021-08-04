using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FingerprintAttendance.Infrastructure.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    public interface IRepository<TKey, TModel> where TModel : class
    {
        IQueryable<TModel> AsQueryable();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        void Insert(TModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        void Delete(TKey key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="model"></param>
        void Update(TKey key, TModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<TModel> Get(TKey key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<TModel> Get(Expression<Func<TModel, bool>> expression);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TModel>> GetAll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<IEnumerable<TModel>> GetAll(Expression<Func<TModel, bool>> expression);

        /// <summary>
        /// Saves all pending changes in the tracking collection in an asynchronous operation
        /// </summary>
        /// <returns></returns>
        Task<(bool, string)> SaveChangesAsync();

        /// <summary>
        /// Saves all pending changes in the tracking collection
        /// </summary>
        (bool, string) SaveChanges();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IEnumerable<DbValidationError> GetValidationErrors(TModel model);
    }
}
