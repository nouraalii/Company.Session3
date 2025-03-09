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

        [HttpGet]
        public IActionResult Details(int? id , string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id"); //400

            var department = _departmentrepository.Get(id.Value);
            if (department is null) return NotFound(new {StatusCode=404,message=$"Department With Id  : {id} is not found"});
            return View(ViewName , department);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id"); //400

            //var department = _departmentrepository.Get(id.Value);
            //if (department is null) return NotFound(new { StatusCode = 404, message = $"Department With Id  : {id} is not found" });
           
            return Details(id,"Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Department department)
        {
            if (ModelState.IsValid)
            {
                if (id != department.Id) return BadRequest(); //400
                var count = _departmentrepository.Update(department);
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(department);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, UpdateDepartmentDto model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var department = new Department()
        //        {
        //            Id = id, 
        //            Code = model.Code,
        //            Name = model.Name,
        //            CreateAt=model.CreateAt
        //        };
        //        var count = _departmentrepository.Update(department);
        //        if (count > 0)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    return View(model);
        //}


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id"); //400

            //var department = _departmentrepository.Get(id.Value);
            //if (department is null) return NotFound(new { StatusCode = 404, message = $"Department With Id  : {id} is not found" });
            
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Department department)
        {
            if (ModelState.IsValid)
            {
                if (id != department.Id) return BadRequest(); //400
                var count = _departmentrepository.Delete(department);
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(department);
        }
    }
}
