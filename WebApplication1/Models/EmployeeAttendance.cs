namespace WebApplication1.Models
{
    public class EmployeeAttendance
    {
        public int Id { get; set; }
        public Employee Employee { get; set; }
       
        public DateTime attendanceDate { get; set; }
        public bool isPresent { get; set; }
        public bool isAbsent { get; set; }
        public bool isOffday { get; set; }
    }
}
