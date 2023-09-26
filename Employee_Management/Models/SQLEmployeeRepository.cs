namespace Employee_Management.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {  
        //created privete field
        private readonly AppDbContext context;
        private readonly ILogger<SQLEmployeeRepository> logger;

        //injected the AppDbcontext class by creating the constructor os SQLEmployeeReporitory
        public SQLEmployeeRepository(AppDbContext context, ILogger<SQLEmployeeRepository> logger)  //constructor injection
        {
            this.context=context;      //dependancy injection
            this.logger = logger;
        }
        public Employee Add(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
            return employee;

        }

        public Employee Delete(int id)
        {
            Employee employee = context.Employees.Find(id);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
          return context.Employees;
        }

        public Employee Getemployee(int Id)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information log");
            logger.LogWarning("Warning log");
            logger.LogError("Error log");
            logger.LogCritical("Crirtical log");
            return context.Employees.Find(Id);
        }

        

        public Employee Update(Employee employeeChanges)
        {
          var employee = context.Employees.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return employeeChanges;
        }
    }
}
