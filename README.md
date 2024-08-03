MVC CRUD Application >>>
search MVC Templeate Give project Name >> Visual Studio already has Model, View, Controller


To able to talk to Data Base>>>> SQL Server (MSSQL)_Entity Framework Core
go to Dependencies > Right Click > click on Manage NuGet Packages 
 
- Search this in the package
	Microsoft.EntityFrameworkcore.sqlServer
- Install this sqlServer package (must be 6.0.6) same as .Net version

To do Data Migration with SQL need to install 2nd Package 
- Search 2nd Package 
	Microsoft.EntityframeworkCore.Tools 
- Install this package (must be 6.0.6)


- create Employee Model 
(Model file > add new folder (Domain)) then add class Employee. 
Add few properties for this Employee.
Sample code >>>>
	namespace ASPNETMVCCRUD.Models.Doman
	{
    		public class Employee
   	 {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public double Salary { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Department { get; set; }
    	}
	}

Create Database >>>>>

To create New Database - Create DBContext Class
- add File in project then create class called DBContext.cs 
that will be inherit from base class called DBContext
	using Microsoft.EntityFrameworkCore; 
(add this as Library in DBContext.cs)
- create Constructor (click on class name and press Ctl+.) 
	generate constructor with (option) 

- create Properties (shoutcut > prop then press 2 times) 
	change property Dbtype of DbSet<>, then create Employee Model

sample code >>>>
	using ASPNETMVCCRUD.Models.Doman;
	using Microsoft.EntityFrameworkCore;

	namespace ASPNETMVCCRUD.Data
	{
   	 public class MVCDemoDbContext : DbContext
    	{
        public MVCDemoDbContext(DbContextOptions options) : 
            base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }

   	 }
	}

- Use this Employee Model into (Data) Domain Model of MVCDbContext.cs
	DbSet<Emoloyee> (property must be employee cos we want to use employee model.
code sample >>>> 	
	public DbSet<Employee> Employees { get; set; }
	Call Ctl+. then use using ASPNETMVCCRUD.Models.Domain;

- then need to inject into Program.cs file if not application will not know about DBContext.
	and what property we want to create. 
code sample >>>> 
	// Add services to the container.
	builder.Services.AddControllersWithViews();
	builder.Services.AddDbContext<MVCDemoDbContext>(options =>
    	options.UseSqlServer(builder.Configuration.
	GetConnectionString("MvcDemoConnectionString")));


- appsetting.jason (need to add ConnectionString Property)
	"MvcDemoConnectionString": "server=;database=;Trusted_connection=true"
sample code >>>
	"ConnectionStrings": {
    	"MvcDemoConnectionString": "Server=localhost;Database=MvcDemoDb;Trusted_Connection=True;"
  	}
Database name = MvcDemoDb will create inside MSSQL server. 

- to create Database inside MSSQL server.
Tools > Nuget package manager > Console 

1.	Add-Migration "name something" 

2.	Update-Database


-Create frew pages to do CRUD. 
	Add new Controller (MVC Empty Controller) 
	Create Add Mehtod (that is HttpGet)
sample code >> 
	using Microsoft.AspNetCore.Mvc;
	namespace ASPNETMVCCRUD.Controllers
	{
   	 public class EmployeesController : Controller
    	{
       		 [HttpGet]
       		 public IActionResult Add()
        {
            return View();
       	 }
    	}
  }

-Add View for AddEmployee > can click return action view to create cshtml
- to navigate this view page add this nav list into layout.cshtml 
Add new employee will be under Employees (Controller) action will be (Add) 
then nav bar name is Add Employee 
sample code >>> 
 <li class="nav-item">
  <a class="nav-link text-dark" asp-area="" asp-controller="Employees" asp-action="Add">Add Employee</a>
 </li>


- Create AddEmployeeViewModel.cs (to tigger form action) that will be same property as Employee model.
but no Id ( when user add new employee DB will auto define ID) 

-Inside Views > Create Employees Folder > then create Add.cshtml

-to create Add new employee form use Bootstrap. 
	https://getbootstrap.com/docs/4.1/components/forms/
inside the form structure add method ="post" action="Add" (this add will trigger Models)
when user key in input box, we need to tigger and tie to Employee moedel. 
so need to define asp-for="property"
sample code >> 
	
@model ASPNETMVCCRUD.Models.AddEmployeeViewModel

@{

}

<form method="post" action="Add">
    <div class="mb-3">
        <label for="" class=" = " form-label">Name</label>
        <input type="text" class="form-control" asp-for="Name">
    </div>

    <div class="mb-3">
        <label for="" class=" = " form-label">Email Address</label>
        <input type="email" class="form-control" asp-for="Email">
    </div>

    <div class="mb-3">
        <label for="" class=" = " form-label">Salary</label>
        <input type="number" class="form-control" asp-for="Salary">
    </div>

    <div class="mb-3">
        <label for="" class=" = " form-label">Date of Birth</label>
        <input type="date" class="form-control" asp-for="DateOfBirth">
    </div>

    <div class="mb-3">
        <label for="" class=" = " form-label">Department</label>
        <input type="text" class="form-control" asp-for="Department">
    </div>

    <button type="submit" class="btn btn-primary">Submit</button>
</form>




-  Create Post Method in EmployeesController (Add action)
	Add method require one parameter of Model (AddEmployeeViewModel ddEmployeeRequest)
	create new employee based on what user input
	ID will auto generate ( Guid.NewGuid())
	the rest will be addEmployeeRequest.Name .........
Sample Code >>>
	[HttpPost]
        public async Task<IActionResult> Add(AddEmpiloyeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department,
                DateOfBirth = addEmployeeRequest.DateOfBirth
            };

- Need to inject DbContext , which is injected in sercvices (program.cs)
	inside EmployeeController on top of Add action. create constructor 
	short key = ctor
	then call MVCDemoDbContext > give name of that model 
	then create and assign field..(this readonly will talk to database.)
Sample code >>>
	private readonly MVCDemoDbContext mvcDemoDbContext;

        public EmployeesController(MVCDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }
	

-In add action (post method) 
	call above readonly property.Employees.Add(employee);
	readonly property.SaveChanges(); > that will save into Database. 


- Asyncoronous Method
	add async in Add action then IActionResult is Task > 
		then add Asyncinside action ,
		 Await in front of action to update database. 
Sample code >>> 

	 public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department,
                DateOfBirth = addEmployeeRequest.DateOfBirth
            };

                await mvcDemoDbContext.Employees.AddAsync(employee);
                await mvcDemoDbContext.SaveChangesAsync();
             return RedirectToAction("Add");
        }


- Showing the List of Employee 
	In EmployeeController create Index Method (HttpGet)
	That will be async await method.. 
sample code >>>> 

	 [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await mvcDemoDbContext.Employees.ToListAsync();
            return View(employees);
        }


-create Index page to Show Employee List (Table)
sample code >>>

	@model List<ASPNETMVCCRUD.Models.Domain.Employee>
@{
}

<h1>Employee List</h1>
<table class = "table">
    <thead>
        <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Email</th>
            <th>Salary</th>
            <th>Date of Birth</th>
            <th>Deaprtment</th>
        </tr>
    </thead>
    <tbody>
        @foreach(var employee in Model)
        {
            <tr>
                <td>@employee.Id</td>
                <td>@employee.Name</td>
                <td>@employee.Email</td>
                <td>@employee.Salary</td>
                <td>@employee.DateOfBirth.ToString("dd-MMM-yyyy")</td>
                <td>@employee.Department</td>
            </tr>
        }
    </tbody>
</table> 

- give directory route in Layout > 
	 <li class="nav-item">
              <a class="nav-link text-dark" asp-area="" asp-controller="Employees" asp-action="Index">Employees List</a>
          </li>

- Since already create view index for list of employee, change direction of 
Add Employee function > return RedirectToAction("Index");



- View/Update Employee Deatil
	inside table of employees (Index.html). add in empty head <td>
	then add <td><a href="Employees/View/@employee.Id">View</a></td>
that will show herf path of employees/view with respective employeeID

-trigger this action (View/Update)
	inside EmployeeController create function View to show Employee.
	to show this need to create another Model (UpdateEmployeeViewModel)
sample code >>>> that is exactly same as Employee Model 
	
	namespace ASPNETMVCCRUD.Models
	{
    	public class UpdateEmployeeViewModel
   	 {
        	public Guid Id { get; set; }
        	public string Name { get; set; }
        	public string Email { get; set; }
        	public double Salary { get; set; }
        	public DateTime DateOfBirth { get; set; }
        	public string Department { get; set; }
    	}
	}
- inside View Action (HttpGet) with await , async
samplecode >>> 
	
	[HttpGet]
        public async Task <IActionResult> View(Guid id)
        {
            var employee = await mvcDemoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth

                };
                return await Task.Run(()=> View("View", viewModel));
            }
                 return RedirectToAction("Index"); 
        }
	
- to show this View/Update Employee Create another Page View.cshtml
	This view html is using UpdateEmployeeViewModel in order to let update it. 
sample code >>> (ID not able to change)

		@model ASPNETMVCCRUD.Models.UpdateEmployeeViewModel
	@{
	}

		<h1>View Employee</h1>
		<form method="post" action="Add" class="mb-5">
    		<div class="mb-3">
        		<label for="" class=" = " form-label">ID</label>
       		 <input type="text" class="form-control" asp-for="Id" readonly>
    		</div>
    		<div class="mb-3">
        		<label for="" class=" = " form-label">Name</label>
        		<input type="text" class="form-control" asp-for="Name">
    		</div>
    		<div class="mb-3">
        		<label for="" class=" = " form-label">Email Address</label>
        		<input type="email" class="form-control" asp-for="Email">
    		</div>
    		<div class="mb-3">
        		<label for="" class=" = " form-label">Salary</label>
        		<input type="number" class="form-control" asp-for="Salary">
    		</div>
    		<div class="mb-3">
        		<label for="" class=" = " form-label">Date of Birth</label>
        		<input type="date" class="form-control" asp-for="DateOfBirth">
    		</div>
    		<div class="mb-3">
        		<label for="" class=" = " form-label">Department</label>
        		<input type="text" class="form-control" asp-for="Department">
    		</div>
    		<button type="submit" class="btn btn-primary">Update</button>
		</form>

- To trigger Update Function create [HttpPost] Method for View Action 
	always need savechanges
sample code >>> 
		[HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.Department = model.Department;
                employee.DateOfBirth = model.DateOfBirth;

                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }

- Delete Function >>> create one more button inside View html page 
	btn is danger and need to add asp-action , asp-controller
	 <button type="submit" class="btn btn-danger"
        asp-action="Delete" 
        asp-controller="Employees">Delete</button>
	
-for asp-action = "delete" > create [httpPost] Delete Action inside Employees COntroller
sample code >>>>
	
	[HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                mvcDemoDbContext.Employees.Remove(employee);
                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            
            return RedirectToAction("Index");
        }

	
			






	
