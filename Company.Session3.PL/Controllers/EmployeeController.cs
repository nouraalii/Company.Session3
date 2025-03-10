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

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employees = _employeeRepository.GetAll();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid) 
            {
                var employee = new Employee
                {
                    Name = model.Name,
                    Address = model.Address,
                    Age = model.Age,
                    CreateAt = model.CreateAt,
                    Email = model.Email,
                    HiringDate = model.HiringDate,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    Phone = model.Phone,
                    Salary= model.Salary,
                };
                var count = _employeeRepository.Add(employee);
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
            if (id is null) return BadRequest();
            var employees = _employeeRepository.Get(id.Value);
            if(employees is null) return NotFound(new { StatusCode = 404, message = $"Employee With Id  : {id} is not found" });
            return View(ViewName,employees);
        }

        [HttpGet]
        public IActionResult Edit(int? id) 
        {
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
            if (ModelState.IsValid)
            {
                if (id != employee.Id) return BadRequest(); // 400 
                var count = _employeeRepository.Delete(employee);
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }

            return View();
        }
    }
}
