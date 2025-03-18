using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Session3.DAL.Models;

namespace Company.Session3.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        //IEnumerable<Employee> GetAll();

        //Employee? Get(int id);

        //int Add(Employee model);
        //int Update(Employee model);
        //int Delete(Employee model);

        List<Employee> GetName (string name);
    }
}
