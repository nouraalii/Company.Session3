using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Session3.DAL.Models;

namespace Company.Session3.BLL.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        //IEnumerable<Department> GetAll();

        //Department? Get(int id);

        //int Add(Department model);
        //int Update(Department model);
        //int Delete(Department model);
    }
}
