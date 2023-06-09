﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;
using WebApplication1.Database;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        private readonly ApplicationDBContext _context;

        public EmployeesController(ApplicationDBContext context)
        {
            _context = context;
        }


      
        [HttpPut("UpdateEmployeeCode")]
        public ActionResult UpdateEmployeeCode(string oldEmpCode, string NewEmpCode)
        {
            var employee = _context.tblEmployee.FirstOrDefault(e => e.employeeCode == oldEmpCode);
            if (employee == null)
            {
                return NotFound();
            }

            if (_context.tblEmployee.Any(e => e.employeeCode == NewEmpCode))
            {
                return Conflict("Another employee already has this Employee Code.");
            }

            employee.employeeCode = NewEmpCode;
            _context.tblEmployee.Update(employee);
            _context.SaveChanges();
            return Ok("Employees Code Updated Successfully");
        }

        [HttpGet("GetEmployeeBySalary")]
        public ActionResult<IEnumerable<Employee>> GetEmployeeBySalary() {
            return _context.tblEmployee.OrderByDescending(e => e.employeeSalary).ToList();
        }

        [HttpGet("GetAbsentEmployees")]
        public ActionResult<IEnumerable<Employee>> GetAbsentEmployees()
        {
            List<Employee> empList = new List<Employee>();
           var adsentEmpList= _context.tblEmployeeAttendance.Include(e=>e.Employee).Where(e=>e.isAbsent==true).ToList();
            foreach (var item in adsentEmpList)
            {
                var emp = _context.tblEmployee.Find(item.Employee.employeeId); 
                empList.Add(emp);
            }

            return Ok(empList);
        }

        [HttpGet("GetMonthlyAttendanceReport")]
        public ActionResult<IEnumerable<object>> GetMonthlyAttendanceReport()
        {
            var attendanceRecords = _context.tblEmployeeAttendance
                .Include(e => e.Employee)
                .ToList();

            var report = attendanceRecords
                .GroupBy(a => new { a.Employee.employeeName, Month = a.attendanceDate.ToString("MMMM") })
                .Select(g => new
                {
                    EmployeeName = g.Key.employeeName,
                    MonthName = g.Key.Month,
                    TotalPresent = g.Count(a => a.isPresent),
                    TotalAbsent = g.Count(a => a.isAbsent),
                    TotalOffday = g.Count(a => a.isOffday)
                })
                .ToList();

            return Ok(report);
        }
    }
}
