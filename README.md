Book Shop Shopping Cart MVC with User Role >>>>>>>>>>>>>>>>>>>>

1. Choose ASP.NET Core Web Appp (Model-View-Controller)
Choose .Net 7 
Authentication Type - Individual Accounts
Configure for HTTPS

2. Inside Area Folder >> already has migration file for User Log in 

3. To connect DB open MSSQL server set with Default Setting (such as Local)

inside appsettings.json >>> 

"ConnectionStrings": {
    "DefaultConnection": "Server=(local);Database=BookShoppingCartMvc;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  },


4. Inside Area Foler > Identity folder > Right click on it and add New Scaffolded Item

then click on Identity and Overrdie all files (or) use what we only need.. 
it will ask database context and choose DbContext. 

5. Create Databse 

open nuget packages >>  update-database 


6. Create Table >>>>>>> Models > in this case BookShop we create 
  [Table("Book")]
    public class Book
    {
        public int Id { get; set; }
        [Required, MaxLength(40)]
        public string? BookName { get; set; }
        [Required]
        public  double Price { get; set; }
        public string? Image { get; set; }
        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public List<OrderDetail> OrderDeatils{ get; set; }
        public List<CartDetail> CartDetails { get; set; }



    }

 [Table("CartDetail")]
    public class CartDetail
    {
        public int Id { get; set; }
        [Required]
        public int ShoppingCartId { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public Book Book { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }

    [Table("Genre")]
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string GenreName { get; set; }
        public List<Book>Books;
    }
}

   [Table("Order")]
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public  Guid UserId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        [Required]
        public int OrderStatus { get; set; }
        public bool IsDeleted { get; set; } = false;
        public OrderStatus orderStatusId{ get; set; }
        public List<OrderDetail> OrderDetails{ get; set; }

    }

    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int Id { get; set; }
        [Required]
        public  int OrderId  { get; set; }
        [Required]
        public  int BookId  { get; set; }
        [Required]
        public int Quantity { get; set; }
        public double UniquePrice { get; set; }
        public Order Order { get; set; }
        public  Book Book { get; set; }


    }

    [Table("OrderStatus")]
    public class OrderStatus
    {
        public int Id { get; set; }
        [Required]
        public int StatusId { get; set; }
      
        [Required, MaxLength(20)]
        public string? StatusName { get; set; }
    }

    [Table("ShoppingCart")]
    public class ShoppingCart 
    {
        public int Id { get; set; }

        [Required]
        public int CartId { get; set; }
        [Required]
        public int UserId { get; set;}

        public bool IsDeleted { get; set; } = false;

    }

7. Then Update Database with those tables >> 
Migrations > ApplicationDbContext.cs >> inside this class 
add all those Models 

  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<OrderStatus> OrderStatuses{ get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

8. Update Database >> inside Nuget Package Consle 
add-migration (give name for this migration) >> after migration done 
update-database 

USER ROLE CREATION >>>>>>>>>>>>>>>>>>>>>

10. Create folder called Constant > inside that add Roles.cs and update as follow 
	   public enum Roles
    {
        User=1,
        Admin
    }

11. Areas > Identity > Pages > Account > Manage > 
	Register.cshtml > Register.cshtml.cs (open this class) 
Find Post Method >>> OnPostAsync ( like when user successfully created we will add some roles to it) 
	CreateUser(); after success >>>>> 
	if (result.Succeeded)
                {
                    //assigning roles to user
                    await _userManager.AddToRoleAsync(user,Roles.User.ToString());

12. In the package Roles data are Empty > so before we run it we need to create Admin role in backend app our own 

	Data > add DbSeeder.cs >>>
		public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();

            //Adding some roles to DB
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            //Create Admin user
            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            };

            var userInDb = await userMgr.FindByEmailAsync(admin.Email);
            if(userInDb is null) {
                await userMgr.CreateAsync(admin, "Admin@123");
                await userMgr.AddToRoleAsync(admin,Roles.Admin.ToString());
            }
        }
    }

13. Inside Program.cs >>>> change this funciton > builder.service.AddDefaultIdentity 
	
    builder.Services.AddIdentity<IdentityUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    	.AddEntityFrameworkStores<ApplicationDbContext>()
   	.AddDefaultUI()
    	.AddDefaultTokenProviders();

14. change abit inside Login Layout to know Admin account
	Shared > LoginPartial.cshtml
	
	 <li class="nav-item">
        <a  class="nav-link text-dark" asp-area="Identity" asp-page="/Account/Manage/Index" title="Manage">Hello @User.Identity?.Name!
                @if (User.IsInRole("Admin"))
                {
                    <span>(Admin)</span>
                }
            </a>
    </li>
	
