using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Session3.BLL.Interfaces;
using Company.Session3.DAL.Data.Contexts;
using Company.Session3.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.Session3.BLL.Repositiories
{
    public class EmployeeRepository : GenericRepository<Employee>,IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepository(CompanyDbContext context) : base(context) //ASK CLR to create object from CompanyDbContext
        {
            _context = context;
        }

        public async Task<List<Employee>> GetNameAsync(string name)
        {
            return await _context.Employees.Include(E=>E.Department).Where(E=>E.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }

        //private readonly CompanyDbContext _context; 

        //public EmployeeRepository(CompanyDbContext context)
        //{
        //    _context = context;
        //}

        //IEnumerable<Employee> IEmployeeRepository.GetAll()
        //{
        //    return _context.Employees.ToList();
        //}

        //Employee? IEmployeeRepository.Get(int id)
        //{
        //    return _context.Employees.Find(id);
        //}

        //public int Add(Employee model)
        //{
        //    _context.Employees.Add(model);
        //    return _context.SaveChanges();
        //}

        //public int Update(Employee model)
        //{
        //    _context.Employees.Update(model);
        //    return _context.SaveChanges();
        //}

        //public int Delete(Employee model)
        //{
        //    _context.Employees.Remove(model);
        //    return _context.SaveChanges();
        //}
    }
}
