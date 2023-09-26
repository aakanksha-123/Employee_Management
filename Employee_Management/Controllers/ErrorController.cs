using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Management.Controllers
{
    public class ErrorController : Controller //create a view for the ErrorController of name==NotFound.cshtml
    {
        private ILogger<ErrorController> logger; //injecting the Logger interface into the errorcontrollerusing the constructor of the Errocontroller

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;

        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you required could not be found";
                    //ViewBag.Path = statusCodeResult.OriginalPath;

                    //ViewBag.QS = statusCodeResult.OriginalQueryString;
                    logger.LogWarning($"$04 Error Occured. Path={statusCodeResult.OriginalPath}" + $" and QuerString={statusCodeResult.OriginalQueryString}");


                    break;
            }
            return View("NotFound");
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            //ToString show all the details on the browser
            //if we display the details to the user is security risk
            //var exceptionDetails=HttpContext.Features.Get<IExceptionHandlerFeature> ();
            //ViewBag.ExceptionPath = exceptionDetails.Path;
            //ViewBag.ExceptionPath = exceptionDetails.Error.Message;
            //ViewBag.Stacktrace = exceptionDetails.Error.StackTrace;


            //logger information
            //if we display the details to the user is security risk ===USE LOGGER
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerFeature>();
            logger.LogError($"The path {exceptionDetails.Path} threw an exception" + $"{exceptionDetails.Error}");

            return View("Error");
        }

    }
}
//public IActionResult Index()
        //{
        //    return View();
        //}
    


