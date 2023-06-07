using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Database
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
        {
            
        }

        public DbSet<Employee> tblEmployee { get; set; }
        public DbSet<EmployeeAttendance> tblEmployeeAttendance{ get; set; }
    }
}
