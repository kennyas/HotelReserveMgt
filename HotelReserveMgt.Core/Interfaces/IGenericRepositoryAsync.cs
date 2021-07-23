using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelReserveMgt.Core.Interfaces
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        //Task<T> GetByIdAsync(int id);
        //Task<IReadOnlyList<T>> GetAllAsync();
        //Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize);
        //Task<T> AddAsync(T entity);
        //Task UpdateAsync(T entity);
        //Task DeleteAsync(T entity);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task<T> CreateAsync(T obj);
        Task UpdateAsync(string id, T entity);
        Task DeleteAsync(string id);
    }
}
