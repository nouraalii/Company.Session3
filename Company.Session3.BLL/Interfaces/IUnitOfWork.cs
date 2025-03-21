using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Session3.BLL.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
         IDepartmentRepository DepartmentRepository { get; } //Null
         IEmployeeRepository EmployeeRepository { get; }

        Task<int> CompleteAsync();

    }
}
