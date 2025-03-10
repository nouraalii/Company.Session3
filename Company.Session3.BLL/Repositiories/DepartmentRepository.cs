using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Session3.BLL.Interfaces;
using Company.Session3.DAL.Data.Contexts;
using Company.Session3.DAL.Models;

namespace Company.Session3.BLL.Repositiories
{
    public class DepartmentRepository : GenericRepository<Department>,IDepartmentRepository
    {
        public DepartmentRepository(CompanyDbContext context) : base(context) //ASK CLR to create object from CompanyDbContext
        {
            
        }

        //private readonly CompanyDbContext _context; //Null  

        ////ASK CLR To create object from 
        //public DepartmentRepository(CompanyDbContext context)
        //{
        //    _context = context;
        //}

        //public IEnumerable<Department> GetAll()
        //{
        //    return _context.Departments.ToList();
        //}

        //public Department? Get(int id)
        //{
        //    return _context.Departments.Find(id);
        //}

        //public int Add(Department model)
        //{
        //    _context.Departments.Add(model);
        //    return _context.SaveChanges();
        //}

        //public int Update(Department model)
        //{
        //    _context.Departments.Update(model);
        //    return _context.SaveChanges();
        //}

        //public int Delete(Department model)
        //{
        //    _context.Departments.Remove(model);
        //    return _context.SaveChanges();
        //}
    }
}
