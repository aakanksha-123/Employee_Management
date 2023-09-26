using Employee_Management.Models;
using Employee_Management.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
namespace Employee_Management.Controllers
{
    //[Route("Home")] //ATTRIBUTE ROUTING
    //ACTION TOKEN  ==> varti home he controller so instead of that we specify controller so controller get replaced with Home controller 
    [Route("[controller]/[action]")]   //ites madetory for that to pass action with home controller, if not it will throw 404 error
    [Authorize]
    public class HomeController : Controller
    {

        private readonly IEmployeeRepository _employeeRepository;
       
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ILogger logger;

        public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment,ILogger<HomeController> logger)
        {   
            
          


            _employeeRepository = employeeRepository;  //INSTANCE(OBJECT) ASSIGN TO PRIVATE MEMBER VARIABLE==> WHICH ALLOW THE _employeeRepository TO FETCH THE DATA
            //dependancy injection ==same instnace use multiple times 

            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }

        ////ATTRIBUTE ROUTING
        //[Route("")]
        //[Route("~/")] //to fixed error 404 when url is localhost:71789 only 
        //[Route("Index")]  
        //instead of specifying the index as directly name used here the token=[action] which get replaced bt the Index action name
        [Route("")]
        [Route("~/")]
        [Route("~/Home")] //to solve 404 error only for localhost:7234/home == we include [Route("~/Home")] =error gone
        //[Route("[action]")]
        [AllowAnonymous]
        public /*JsonResult*/ /*string*/ ViewResult Index() //json result to string
        {
            //return Json(new { id = 1, name = "aakanksha" });
            //lets return the name property
            //lets return the name property


            //1//here we use ===> public Employee Getemployee(int Id) method

            //return _employeeRepository.Getemployee(1).Name;

            //2 //here we use==>public IEnumerable<Employee> GetAllEmployee() method
            var model = _employeeRepository.GetAllEmployee();
            return View(model);





        }

        //    public IActionResult Index()
        //{
        //    return View();
        //}

        //}

        //public ViewResult Details()
        //{
        //created a employee object and assign it a 



        /* Employee model = _employeeRepository.Getemployee(1);*/          //we used that var in which everytime the data is going to stored 
        /* return View(model);*/ //chnages for content negotiation         //simply return the view i.e details


        //return the view ==>Views/Home/Test.cshtml
        //  Employee model = _employeeRepository.Getemployee(1); 
        //return View("Test");

        //use the Test view from==> MyView/Test.cshtml
        //return View("MyViews/Test.cshtml");

        //to use the view from==> View/Test/Update.cshtml
        //  Employee model = _employeeRepository.Getemployee(1); 
        //return View("../Test/Update");



        //ViewData
        //another way of passing a data to a viewform controller == using ViewData (in key and values)
        //  Employee model = _employeeRepository.Getemployee(1); 
        //ViewData["Employee"]= model; //we are passing the object of name == model from homecontroller to the Details.cshtml ==usimg the ViewData and that Viewdata uses the ==>key==Employee
        //ViewData["PageTitle"]= "Employee Details"; //here we simply returnen the string value
        //return View();






        //ViewBag


        //  Employee model = _employeeRepository.Getemployee(1); 
        //ViewBag.Employee = model; //we are passing the object of name == model from homecontroller to the Details.cshtml ==usimg the ViewData and that Viewdata uses the ==>key==Employee
        //ViewBag.PageTitle = "Employee Details"; //here we simply returnen the string value
        //return View();



        //Strongly typed view

        /* Employee model = _employeeRepository.Getemployee(1);*/
        //ViewBag.PageTitle = "Employee Details";
        //return View(model);


        /*********************************?
         *   ViewModel  ****/
        //Employee model = _employeeRepository.Getemployee(1);

        //[Route("Details/{id?}")]
        [Route("{id?}")]
        [AllowAnonymous]
        public ViewResult Details(int? id) {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information log");
            logger.LogWarning("Warning log");
            logger.LogError("Error log");
            logger.LogCritical("Crirtical log");
            //throw new Exception("Error in details view");
            Employee employee = _employeeRepository.Getemployee(id.Value);
            if (employee == null) {
                Response.StatusCode = 404;
                return View("EmployeeNotFound",id.Value);
            }
        
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee, //c# null collasion operator
                PageTitle = "Employee Details"

            };
            return View(homeDetailsViewModel);
            

        }
        
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }


        [HttpGet]
        
        //for Edit view
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.Getemployee(id);
            //get details from our underlying database
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.Photopat
            };
            return View(employeeEditViewModel);
        }


















        ///created that time for to add the new employee that time @model Employee is used so we pass Employee employee object as parameter
        //[HttpPost]
        //public /*RedirectToActionResult*/ IActionResult Create(Employee employee)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Employee newEmployee = _employeeRepository.Add(employee); //adding a new employee
        //        //return RedirectToAction("details", new { id = newEmployee.Id });
        //    }
        //    return View();
        //}


        //for upload a photo i used here the @employeeCreateViewModel 
        [HttpPost]
      
        public /*RedirectToActionResult*/ IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                if (model.Photos != null && model.Photos.Count>0)
                {
                    foreach (IFormFile photo in model.Photos)
                    {
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                        string filepath = Path.Combine(uploadsFolder, uniqueFileName);
                        photo.CopyTo(new FileStream(filepath,
                            FileMode.Create));
                    }
                }
                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    Photopat = uniqueFileName
                };
                _employeeRepository.Add(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }

        [HttpPost]
        public /*RedirectToActionResult*/ IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.Getemployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if(model.Photos!=null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                       string filePath= Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.Photopat = ProcessUploadedFile(model);
                }
               
                

                Employee Updatedemployee=_employeeRepository.Update(employee);
                return RedirectToAction("index") ;
            }
            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = "null";
            if (model.Photos != null && model.Photos.Count > 0)
            {
                foreach (IFormFile photo in model.Photos)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filepath = Path.Combine(uploadsFolder, uniqueFileName);
                    photo.CopyTo(new FileStream(filepath,
                        FileMode.Create));
                }
            }

            return uniqueFileName;
        }
    }
}
