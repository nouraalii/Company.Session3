using Company.Session3.BLL.Interfaces;
using Company.Session3.BLL.Repositiories;
using Microsoft.AspNetCore.Mvc;

namespace Company.Session3.PL.Controllers
{
    //MVC Controller
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentrepository;

        //ASK CLR To create object from DepartmentRepository
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentrepository = departmentRepository;
        }

        public IActionResult Index()
        {
            var departments= _departmentrepository.GetAll();

            return View(departments);
        }
    }
}
