using InMemoryDemo.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InMemoryDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDbContext _context;
        private readonly ILogger<EmployeeController> _logger;
        public EmployeeController(ILogger<EmployeeController> logger, EmployeeDbContext context)
        {
            _context = context;
            this._logger = logger;
        }

       
        [HttpGet]
        public async Task<List<Employee>> GetEmployees()
        {
            var Empdata = await _context.Employees.ToListAsync();
            return Empdata;
            
        }

        [HttpGet("{Id}")]
        public async Task<Employee> GetEmployeeById(int Id)
        {
            var empdata = await _context.Employees.FirstOrDefaultAsync(m => m.Id == Id);
            return empdata;
            
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var emp = await _context.Employees.FirstOrDefaultAsync(m => m.Id == Id);
            if (emp == null)
            {
                return NotFound("Employee with the id " + Id + " does not exist");

            }
            _context.Employees.Remove(emp);
            _context.SaveChanges();
            return Ok("Employee with the Id " + Id + " deleted successfully");
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
             await _context.SaveChangesAsync();
            return Created($"api/employee/" + employee.Id, employee);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(int Id, Employee employee)
        {
            var emp = await _context.Employees.FirstOrDefaultAsync(m => m.Id == Id);
            if (emp == null)
            {
                return NotFound("Employee with the id " + Id + " does not exist");

            }

            if (emp.Name != null)
            {
                emp.Name = employee.Name;
            }
            if (emp.Gender != null)
            {
                emp.Gender = employee.Gender;
            }
            if (emp.age != 0)
            {
                emp.age = employee.age;
            }
            if (emp.salary != 0)
            {
                emp.salary = employee.salary;
            }
             _context.Update(emp);
             _context.SaveChanges();
            return Ok("Employee with the id " + Id + " updated successfully");
        }
    }
}
