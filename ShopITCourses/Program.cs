using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopITCourses;
using ShopITCourses.Data;
using ShopITCourses.Services;
using ShopITCourses.Services.IServices;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

//var connectionString = builder.Configuration.GetConnectionString("MSSQLConnection") ?? throw new InvalidOperationException("Not found ConnectionStrings");
// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySQL(connectionString));

//builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(connectionString));

#region Register Custom Services
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<IAccessToApi, AccessToApi>();
#endregion

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
                options.SignIn.RequireConfirmedAccount = false
                ).AddDefaultTokenProviders()
                 .AddDefaultUI()
                 .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Lockout settings.
    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.Cookie.Name = "ShopIT_Identity";
});

builder.Services.AddAuthorization(options =>
         {
             options.AddPolicy("AdminManager", policy => policy.RequireRole(WC.AdminRole, WC.ManagerRole));
             options.AddPolicy("Customer", policy => policy.RequireRole(WC.CustomerRole));
             options.AddPolicy("Admin", policy => policy.RequireRole(WC.AdminRole));
         });

#region Google Authentication
var clientId = builder.Configuration["GoogleKeys:ClientId"];
var clientSecret = builder.Configuration["GoogleKeys:ClientSecret"];

if (clientId == null || clientSecret == null)
{
    clientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
    clientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
}

if (clientId != null && clientSecret != null)
{
    builder.Services.AddAuthentication().AddGoogle(options =>
        {
            //options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
            //options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
            options.ClientId = clientId;
            options.ClientSecret = clientSecret;
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("email");

            options.ClaimActions.Clear();
            options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub"); 
            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name"); 
            options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email"); 
            options.ClaimActions.MapJsonKey("picture", "picture"); 
            options.SaveTokens = true;
            options.CallbackPath = "/signin-google";

            options.Events.OnRedirectToAuthorizationEndpoint = context =>
            {
                context.RedirectUri = context.RedirectUri.Replace("http://", "https://");
                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            };
        }
        );
}
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear(); // Дозвіл на всі мережі
    options.KnownProxies.Clear();  // Дозволяємо всі проксі
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Set Roles in DataBase
using (var scope = app.Services.CreateScope())
{ 
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { WC.AdminRole, WC.CustomerRole, WC.ManagerRole };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
#endregion

app.UseForwardedHeaders();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
