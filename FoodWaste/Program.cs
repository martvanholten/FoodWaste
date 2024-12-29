var builder = WebApplication.CreateBuilder(args);

// Add repos to the container.
builder.Services.AddScoped<ICantineRepo, CantineRepo>();
builder.Services.AddScoped<IPakkageRepo, PakkageRepo>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IStudentRepo, StudentRepo>();
builder.Services.AddScoped<IEmployRepo, EmployRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();

// Add services to the container.
builder.Services.AddScoped<ICantineService, CantineService>();
builder.Services.AddScoped<IEmployService, EmployService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPakkageService, PakkageService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddControllersWithViews();

var connection = String.Empty;
if (builder.Environment.IsDevelopment())
{
    connection = builder.Configuration.GetConnectionString("DataBaseFoodWasteConection");
}
else
{
    connection = Environment.GetEnvironmentVariable("DataBaseFoodWasteConection");
}

builder.Services.AddDbContext<FoodWasteContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(config =>
{
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequireUppercase = false;
    config.Password.RequireDigit = false;
    config.Password.RequiredLength = 1;
    config.User.RequireUniqueEmail = false;
    config.SignIn.RequireConfirmedEmail = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FoodWasteContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "User";
    config.LoginPath = "/Home/Login";
});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}");

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Employ", "Student" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.Run();
