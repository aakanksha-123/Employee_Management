using Microsoft.EntityFrameworkCore;

namespace Employee_Management.Models
{  

    //class has to be static
    public static class ModelBuilderExtensions
    {

        //make it static
        public static void seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Mark",
                    Department = Dept.IT,
                    Email = "mary@pragimtech.com",

                },
                new Employee
                {
                    Id = 2,
                    Name = "John",
                    Department = Dept.IT,
                    Email = "john@pragimtech.com"
                }
                );

        }
    }
}
