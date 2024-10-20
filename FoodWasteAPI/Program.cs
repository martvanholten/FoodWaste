using FoodWasteAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICantineRepo, CantineRepo>();
builder.Services.AddScoped<IPakkageRepo, PakkageRepo>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IStudentRepo, StudentRepo>();
builder.Services.AddScoped<IEmployRepo, EmployRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();

builder.Services.AddScoped<ICantineService, CantineService>();
builder.Services.AddScoped<IEmployService, EmployService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPakkageService, PakkageService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//NEEDED WHEN IN AZURE
//var connection = String.Empty;
//if (builder.Environment.IsDevelopment())
//{
//    connection = builder.Configuration.GetConnectionString("TOO_GOOD_TO_GO_CONNSTR");
//}
//else
//{
//    connection = Environment.GetEnvironmentVariable("TOO_GOOD_TO_GO_CONNSTR");
//}

//builder.Services.AddDbContext<TgtgDbContext>(options =>
//    options.UseSqlServer(connection));

builder.Services.AddDbContext<FoodWasteContext>(builderOptions =>
{
    builderOptions.UseSqlServer(builder.Configuration.GetConnectionString("DataBaseFoodWasteConection"));
    builderOptions.EnableSensitiveDataLogging(true);
});

builder.Services.
    AddGraphQLServer()
    .AddQueryType<QueryDb>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .RegisterDbContext<FoodWasteContext>()
    .RegisterService<PakkageService>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGraphQL();

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
