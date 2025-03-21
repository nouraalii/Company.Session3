using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Session3.DAL.Models;

namespace Company.Session3.BLL.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetAsync(int id);

        Task AddAsync(T model);
        void Update(T model);
        void Delete(T model);
    }
}
