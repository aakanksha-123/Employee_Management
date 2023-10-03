//using Microsoft.EntityFrameworkCore;


//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Employee_Management.Models
//{
//    public class AppDbContext : IdentityDbContext
//    {
//        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
//        {

//        }
//        public DbSet<Employee> Employees { get; set; }


//        //to seed the initial data we need to oveeride the AppDbcontext.cs==ovveride
//        //type kel ki OnModelCreating class get overrided
//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.seed();
//            //move this code to the ModelBuilderExtensions.zs
//            //modelBuilder.Entity<Employee>().HasData(
//            //    new Employee
//            //    {
//            //        Id = 1,
//            //        Name= "Mark",
//            //        Department= Dept.IT,
//            //        Email="mary@pragimtech.com"
//            //    },
//            //    new Employee
//            //    {
//            //        Id=2,
//            //        Name="John",
//            //        Department= Dept.IT,
//            //        Email="john@pragimtech.com"
//            //    }
//            //    ) ;
//        }
//    }




//}








using Employee_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;





namespace Employee_Management.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser> //know them that they usin applicationuser not a identityuser how to know them
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }



        public DbSet<Employee> Employees { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.seed();
            


           
            foreach(var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e=>e.GetForeignKeys())) {
            
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }


        }
    }



}






