namespace Employee_Management.Models
{
    public interface IEmployeeRepository
    {
        Employee Getemployee(int Id);
        IEnumerable<Employee> GetAllEmployee();

        //Added new employee sathi method
        Employee Add(Employee employee);//employee model ch object pass kel

        Employee Update(Employee employeeChanges);

        Employee Delete(int id);
       
    }
}
