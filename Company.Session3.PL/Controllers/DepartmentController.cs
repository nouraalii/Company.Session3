using Company.Session3.BLL.Interfaces;
using Company.Session3.BLL.Repositiories;
using Company.Session3.DAL.Models;
using Company.Session3.PL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Session3.PL.Controllers
{
    //MVC Controller
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentRepository _departmentrepository;

        //ASK CLR To create object from DepartmentRepository
        public DepartmentController(/*IDepartmentRepository departmentRepository*/ IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            //_departmentrepository = departmentRepository;
        }

        [HttpGet] 
        public async Task<IActionResult> Index()
        {
            var departments=await _unitOfWork.DepartmentRepository.GetAllAsync();

            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) //Server Side Validation 
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt=model.CreateAt
                };
                await _unitOfWork.DepartmentRepository.AddAsync(department);
                var count =await _unitOfWork.CompleteAsync();
                if (count > 0) 
                {
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id , string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id"); //400

            var department =await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (department is null) return NotFound(new {StatusCode=404,message=$"Department With Id  : {id} is not found"});
            return View(ViewName , department);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id"); //400

            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (department is null) return NotFound(new { StatusCode = 404, message = $"Department With Id  : {id} is not found" });
            var departmentDto = new CreateDepartmentDto()
            {
                Code = department.Code,
                Name = department.Name,
                CreateAt = department.CreateAt
            };

            return View(departmentDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id != department.Id) return BadRequest(); //400
                var department = new Department()
                {
                    Id = id,
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                _unitOfWork.DepartmentRepository.Update(department);
                var count =await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
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
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id"); //400

            //var department = _departmentrepository.Get(id.Value);
            //if (department is null) return NotFound(new { StatusCode = 404, message = $"Department With Id  : {id} is not found" });
            
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, Department department)
        {
            if (id != department.Id) return BadRequest(); // 400

            var existingDepartment =await _unitOfWork.DepartmentRepository.GetAsync(id);

            if (existingDepartment == null)
            {
                return NotFound(); 
            }

            _unitOfWork.DepartmentRepository.Delete(existingDepartment);
            var count =await _unitOfWork.CompleteAsync();
            if (count > 0)
            {
                return RedirectToAction("Index");
            }

            return View(existingDepartment); 
        }

    }
}
