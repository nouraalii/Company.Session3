using AutoMapper;
using Company.Session3.BLL.Interfaces;
using Company.Session3.DAL.Data.Contexts;
using Company.Session3.DAL.Models;
using Company.Session3.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.Session3.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository employeeRepository ,
            IDepartmentRepository departmentRepository1,
            IMapper mapper
            )
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository1;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                 employees = _employeeRepository.GetAll();
            }
            else
            {
                 employees = _employeeRepository.GetName(SearchInput);
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

        [HttpGet]
        public IActionResult Create() 
        {
            var departments = _departmentRepository.GetAll();
            ViewData["departments"] = departments;

            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid) 
            {
                ////Manual Mapping 
                //var employee = new Employee
                //{
                //    Name = model.Name,
                //    Address = model.Address,
                //    Age = model.Age,
                //    CreateAt = model.CreateAt,
                //    Email = model.Email,
                //    HiringDate = model.HiringDate,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    Phone = model.Phone,
                //    Salary= model.Salary,
                //    DepartmentId=model.DepartmentId
                //};

                var employee = _mapper.Map<Employee>(model);
                var count = _employeeRepository.Add(employee);
                if (count > 0)
                {
                    TempData["Message"] = "Employee is created !!";
                    return RedirectToAction("Index");
                }
            }
            
            return View();
        }

        [HttpGet]
        public IActionResult Details(int? id , string ViewName = "Details") 
        {
            if (id is null) return BadRequest();
            var employees = _employeeRepository.Get(id.Value);
            if(employees is null) return NotFound(new { StatusCode = 404, message = $"Employee With Id  : {id} is not found" });
            return View(ViewName,employees);
        }

        [HttpGet]
        public IActionResult Edit(int? id) 
        {
            var departments = _departmentRepository.GetAll();
            ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id"); //400

            var employee = _employeeRepository.Get(id.Value);
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
            };
            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id != employee.Id) return BadRequest(); // 400 
                var employee = new Employee
                {
                    Id=id,
                    Name = model.Name,
                    Address = model.Address,
                    Age = model.Age,
                    CreateAt = model.CreateAt,
                    Email = model.Email,
                    HiringDate = model.HiringDate,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    Phone = model.Phone,
                    Salary = model.Salary,
                    DepartmentId=model.DepartmentId
                };
                var count = _employeeRepository.Update(employee);
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Employee employee)
        {
            if (id != employee.Id) return BadRequest(); 

            var existingEmployee = _employeeRepository.Get(id);

            if (existingEmployee == null)
            {
                return NotFound(); 
            }

            var count = _employeeRepository.Delete(existingEmployee);

            if (count > 0)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

    }
}
