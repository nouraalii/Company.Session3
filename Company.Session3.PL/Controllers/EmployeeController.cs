using AutoMapper;
using Company.Session3.BLL.Interfaces;
using Company.Session3.DAL.Data.Contexts;
using Company.Session3.DAL.Models;
using Company.Session3.PL.Dtos;
using Company.Session3.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Session3.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(
            //IEmployeeRepository employeeRepository ,
            //IDepartmentRepository departmentRepository1,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository1;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                 employees =await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                 employees =await _unitOfWork.EmployeeRepository.GetNameAsync(SearchInput);
            }

            ////View's Dictionary : there are 3 properties  that can  use to access the dictionary
            ////1.ViewData : Transfer Extra Info from the controller to the view 
            ViewData["Message01 "] = "Hello from ViewData"; //SET


            ////2.ViewBag : Transfer Extra Info from the controller to the view 
            ////ViewBag.Message02 = "Hello from ViewBag";
            //ViewBag.Message02 = new { Message = "Hello from ViewBag" };


            //3.TempData



            return View(employees);
        }


        public async Task<IActionResult> Search(string SearchInput)
        {
            var employees = await _unitOfWork.EmployeeRepository.GetNameAsync(SearchInput);
            return PartialView("EmployeePartialView/EmployeesTablePartialView", employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create() 
        {
            var departments =await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid) 
            {
                if (model.Image is not null)
                {
                   model.ImageName =  DocumentSettings.UplodeFile(model.Image, "images");
                }

                var employee = _mapper.Map<Employee>(model);
                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "Employee is created !!";
                    return RedirectToAction("Index");
                }
            }
            
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id , string ViewName = "Details") 
        {
            if (id is null) return BadRequest();
            var employees =await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if(employees is null) return NotFound(new { StatusCode = 404, message = $"Employee With Id  : {id} is not found" });
            return View(ViewName,employees);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id) 
        {
            var departments =await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id"); //400

            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, message = $"Employee With Id  : {id} is not found" });
            var employeeDto = new CreateEmployeeDto
            {
                Name = employee.Name,
                Address = employee.Address,
                Age = employee.Age,
                CreateAt = employee.CreateAt,
                Email = employee.Email,
                HiringDate = employee.HiringDate,
                IsActive = employee.IsActive,
                IsDeleted = employee.IsDeleted,
                Phone = employee.Phone,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId, 
                ImageName = employee.ImageName
            };
            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto model)
        {
       
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;

            if (model.ImageName is not null && model.Image is not null)
            {
                DocumentSettings.DeleteFile(model.ImageName, "images");
            }

            if (model.Image is not null)
            {
                model.ImageName = DocumentSettings.UplodeFile(model.Image, "images");
            }

            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;

                _unitOfWork.EmployeeRepository.Update(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(model); 
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, Employee employee)
        {
            if (id != employee.Id) return BadRequest();

            var existingEmployee =await _unitOfWork.EmployeeRepository.GetAsync(id);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(existingEmployee.ImageName))
            {
                DocumentSettings.DeleteFile(existingEmployee.ImageName, "images");
            }

            _unitOfWork.EmployeeRepository.Delete(existingEmployee);
            var count =await _unitOfWork.CompleteAsync();

            if (count > 0)
            {
                return RedirectToAction("Index");
            }

            return View();
        }


    }
}
