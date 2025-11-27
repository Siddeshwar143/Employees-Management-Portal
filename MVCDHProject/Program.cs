using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCDHProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<MVCCoreDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConStr")));


// Configure Authentication
builder.Services.AddAuthentication(options =>
***REMOVED***
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
***REMOVED***)
.AddCookie(options =>
***REMOVED***
    // Cookie settings
    options.Cookie.Name = "YourAppAuthCookie";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Requires HTTPS
    options.Cookie.SameSite = SameSiteMode.Strict;

    // Expiration settings
    options.ExpireTimeSpan = TimeSpan.FromDays(30); // Cookie lasts for 30 days
    options.SlidingExpiration = true; // Renews expiration on each request

    // Login path
    options.LoginPath = "/Account/Login";

    // Important for Remember Me functionality
    options.Events = new CookieAuthenticationEvents
    ***REMOVED***
        OnValidatePrincipal = context =>
        ***REMOVED***
            if (context.Properties.IsPersistent)
            ***REMOVED***
                // Extend expiration for persistent cookies (Remember Me)
                context.ShouldRenew = true;
          ***REMOVED***
            return Task.CompletedTask;
      ***REMOVED***
  ***REMOVED***;
***REMOVED***);


builder.Services.AddScoped<ICustomerDAL, CustomerSqlDAL>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
***REMOVED***
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = false;
***REMOVED***).AddEntityFrameworkStores<MVCCoreDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication()
   .AddGoogle(options =>
   ***REMOVED***
       options.ClientId = "880735235587-35f0lf5mssdggphk1jmnh990i3ga9a7q.apps.googleusercontent.com";
       options.ClientSecret = "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";
 ***REMOVED***);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
***REMOVED***
    //app.UseStatusCodePagesWithRedirects("/ClientError/***REMOVED***0***REMOVED***");
    app.UseStatusCodePagesWithReExecute("/ClientError/***REMOVED***0***REMOVED***");
    app.UseExceptionHandler("/ServerError");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
***REMOVED***

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "***REMOVED***controller=Account***REMOVED***/***REMOVED***action=Login***REMOVED***/***REMOVED***id?***REMOVED***")
    .WithStaticAssets();


app.Run();
