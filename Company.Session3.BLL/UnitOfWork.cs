using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Session3.BLL.Interfaces;
using Company.Session3.BLL.Repositiories;
using Company.Session3.DAL.Data.Contexts;

namespace Company.Session3.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext _context;

        public IDepartmentRepository DepartmentRepository { get; } //Null

        public IEmployeeRepository EmployeeRepository { get; } //Null

        public UnitOfWork(CompanyDbContext context)
        {
            _context = context;
            DepartmentRepository = new DepartmentRepository(_context);
            EmployeeRepository = new EmployeeRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
           return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

    }
}
