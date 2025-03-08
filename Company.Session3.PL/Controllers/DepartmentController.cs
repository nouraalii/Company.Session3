using Company.Session3.BLL.Interfaces;
using Company.Session3.BLL.Repositiories;
using Company.Session3.DAL.Models;
using Company.Session3.PL.Dtos;
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

        [HttpGet] 
        public IActionResult Index()
        {
            var departments= _departmentrepository.GetAll();

            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) //Server Side Validation 
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt=model.CreateAt
                };
               var count = _departmentrepository.Add(department);
                if (count > 0) 
                {
                    return RedirectToAction("Index");
                }
            }

            return View();
        }


    }
}
