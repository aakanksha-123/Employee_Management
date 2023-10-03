////video 17

//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.AspNetCore.Mvc;


//using Employee_Management.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using NLog.Extensions.Logging;
//using Microsoft.AspNetCore.Identity;
//using System.Configuration;

//var builder = WebApplication.CreateBuilder(args);

////add the dbContextPOOL
////builder.Services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer("EmployeeDBConnection"));

////OR
////builder.Services.AddDbContextPool<AppDbContext>((serviceProvider, options) =>
////{
////    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
////    var connectionString = configuration.GetConnectionString("EmployeeDBConnection");
////    options.UseSqlServer(connectionString);
////});

///*****/
//builder.Services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetValue<String>("EmployeeDBConnection")));
////builder.Services.AddEntityFrameworkStores<AppDbContext>();
////builder.Services.AddDbContext<AppDbContext>();

//////builder.Services.AddDbContext<AppDbContext>(options =>
//////options.UseSqlServer(
//////builder.Configuration.GetConnectionString("EmployeeDBConnection")
//////));
////builder.Services.AddDbContext<AppDbContext>(options => {
////    options.UseSqlServer(builder.Configuration.GetConnectionString(GetConnectionString.SqlServerExpress));
////});

///****/

//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
//// Configure services
///*builder.Services.AddControllers();*/ // Add support for controllers //no need
///*builder.Services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>();*///crete sinle instance and used that instnce for every http request
///*total count = 3,4,5,.....incremented  */                                                 //everytime we click create button the total count get incremented
///*builder.Services.AddScoped<IEmployeeRepository, MockEmployeeRepository>();*///for every request within scoped new instance is created, for diff scoped new instance created 
///*total count = 4      */
////everytime we click create button the total count not get incremented it wiill reinitialized and show the same vale
//builder.Services.AddMvc().AddXmlSerializerFormatters();
////add logger
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();
//builder.Logging.AddEventSourceLogger();
//builder.Logging.AddNLog();



///*builder.Services.AddTransient<IEmployeeRepository, MockEmployeeRepository>();*/ //new instance provided every time so 
//                                                                                  //click on button but it shows total count=3 only not get incremented



////here we used SQLEmployeeRepository class & AddScoped
//builder.Services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
////builder.Services.AddScoped<IEmployeeRepository, MockEmployeeRepository>();



///*builder.Services.AddMvcCore(option => option.EnableEndpointRouting = false);*/ //added the mvc service
///*builder.Services.AddScoped<IEmployeeRepository, MockEmployeeRepository>();*/ // Or your actual repository implementation
//                                                                               // You can also use AddMvc if needed 
//                                                                               //for additional configuration



//builder.Services.AddMvc(option => option.EnableEndpointRouting = false); //for view need to register this
//builder.Services.AddRazorPages();

//var app = builder.Build();
//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}
//else
//{
//    app.UseExceptionHandler("/Error");
//    app.UseStatusCodePagesWithReExecute("/Error/{0}");
//}

//app.UseStaticFiles();//middleware handle the request and request for stastic file the request is for the default file is not executed
///*app.UseMvcWithDefaultRoute();*/ //bcz of this default route the mapping is happened
//app.UseAuthentication();

////CONVENTIONAL ROUTING
////instead of using app.UseMvcWithDefaultRoute(); we use UseMvc ==>CAALED CONVENTIONAL ROTING
////app.UseMvc(routes =>
////{
////    routes.MapRoute("default", "pragim/{controller}/{action}/{id?}");
////});

///*app.UseMvc();*/

/////to handle the 404 status code error
////app.UseStatusCodePages();
///*app.UseStatusCodePagesWithRedirects("/Error/{0}");*/  //create ErrorController and view of name==NotFound.cshtml


////adding the logger






//app.UseMvc(routes =>
//{
//    routes.MapRoute(
//        name: "default",
//        template: "{controller=Home}/{action=Index}/{id?}" // By putting '?' after 'id', we make the 'id' parameter optional
//    );
//});



///*app.UseRouting();*/ //to use mvc with default route middleware which handle request and produce response



////We no longer need app.UseMvcWithDefaultRoute() as it has been replaced by the routing middleware and MapDefaultControllerRoute().
///*app.MapDefaultControllerRoute();*/ //app.UseMvcWithDefaultRoute() method get replaced by the app.MapDefaultControllerRoute();


////app.Run(async (context) =>
////{
////    await context.Response.WriteAsync("Hello world");
////});
//app.Run();



/******************/

using Microsoft.AspNetCore.Identity;
using Employee_Management.Models;
using NLog.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Employee_Management.Security;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add the DbContextPool
        builder.Services.AddDbContextPool<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDBConnection")));
        //builder.Services.AddDbContextPool<AppDbContext>(options =>
        //options.UseSqlServer(builder.Configuration.GetValue<String>("EmployeeDBConnection")));

        // Add Identity services==done using identity service
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 10;
            options.Password.RequiredUniqueChars = 3;
        })
            .AddEntityFrameworkStores<AppDbContext>();


        //done uing configure method
        //builder.Services.Configure<IdentityOptions>(options =>
        //{
        //    options.Password.RequiredLength = 10;
        //    options.Password.RequiredLength = 3;
        //});

        //AUTHORIZE
        // Configure services  /add the authorize policy and also add authorize filter
        builder.Services.AddMvc(options =>
        {
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build(); options.Filters.Add(new AuthorizeFilter(policy));
        }).AddXmlSerializerFormatters();


        builder.Services.AddAuthentication().AddGoogle(options =>
        {
            options.ClientId = "85889254143-l8l0cfd32ccdsg2b57joc0chp19vfjko.apps.googleusercontent.com";
            options.ClientSecret = "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";

        })
        .AddFacebook(Options =>
        {
            Options.AppId = "976428496779482";
            Options.AppSecret = "4e1a170c21b40653b597677d1fdd1efa";
        });




        // Add logger
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();
        builder.Logging.AddEventSourceLogger();
        builder.Logging.AddNLog();

        // Use SQL Employee Repository with Scoped lifetime
        builder.Services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();

        builder.Services.AddSingleton<IAuthorizationHandler,CanEditOnlyOtherAdminRolesAndClaimsHandler>();

        builder.Services.AddSingleton<IAuthorizationHandler,SuperAdminHandler>();

        // Add MVC
        builder.Services.AddMvc(option => option.EnableEndpointRouting = false);


        //change acessdenied path
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
        });

        //use this property to protect the controller actions or controller
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role"));

            //options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role", "true").RequireRole("Admin").RequireRole("Super Admin"));
            ////////////now use the fun type create our own customize policy
            ///
            //options.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion(context => 
            //context.User.IsInRole("Admin") && context.User.HasClaim(claim => 
            //claim.Type == "Edit Role" && claim.Value == "true")|| context.User.IsInRole("Super Admin")));

            options.AddPolicy("EditRolePolicy", policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

            //options.InvokeHandlersAfterFailure = false;

            options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin"));
        });

          

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
        }

        app.UseStaticFiles();
        app.UseAuthentication();

        // Define default routing
        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}"
            );
        });
        //running the app
        app.Run();
    }
}