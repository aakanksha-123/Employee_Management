using Microsoft.AspNetCore.Mvc;

namespace Employee_Management.Controllers
{
    public class DepartmentController 
    {

        public string List()
        {
            return "List() of Department";

        }
        public string Details()
        {
            return "Details() of Department";

        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
