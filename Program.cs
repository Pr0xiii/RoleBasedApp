using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RoleBasedApp.Context;
using RoleBasedApp.Data;
using RoleBasedApp.Models;
using RoleBasedApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Permissions.ViewOrders, p => p.RequireClaim(Permissions.ViewOrders, "true"));
    options.AddPolicy(Permissions.CreateOrders, p => p.RequireClaim(Permissions.CreateOrders, "true"));
    options.AddPolicy(Permissions.DeleteOrders, p => p.RequireClaim(Permissions.DeleteOrders, "true"));

    options.AddPolicy(Permissions.ViewProducts, p => p.RequireClaim(Permissions.ViewProducts, "true"));
    options.AddPolicy(Permissions.CreateProducts, p => p.RequireClaim(Permissions.CreateProducts, "true"));
    options.AddPolicy(Permissions.DeleteProducts, p => p.RequireClaim(Permissions.DeleteProducts, "true"));

    options.AddPolicy(Permissions.ViewClients, p => p.RequireClaim(Permissions.ViewClients, "true"));
    options.AddPolicy(Permissions.CreateClients, p => p.RequireClaim(Permissions.CreateClients, "true"));
    options.AddPolicy(Permissions.DeleteClients, p => p.RequireClaim(Permissions.DeleteClients, "true"));

    options.AddPolicy(Permissions.ViewUsers, p => p.RequireClaim(Permissions.ViewUsers, "true"));
    options.AddPolicy(Permissions.CreateUsers, p => p.RequireClaim(Permissions.CreateUsers, "true"));
    options.AddPolicy(Permissions.DeleteUsers, p => p.RequireClaim(Permissions.DeleteUsers, "true"));
});

builder.Services.AddRazorPages();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>,
    AppClaimsPrincipalFactory>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantContext, TenantContext>();

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<TenantInvitationService>();

builder.Services.AddScoped<IApplicationService, ApplicationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
