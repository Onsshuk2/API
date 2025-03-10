using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pd311_web_api.BLL.DTOs.Account;
using pd311_web_api.BLL.Services.Account;
using pd311_web_api.BLL.Services.Email;
using pd311_web_api.BLL.Services.Role;
using pd311_web_api.DAL;
using static pd311_web_api.DAL.Entities.IdentityEntities;

var builder = WebApplication.CreateBuilder(args);

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql("name=NpgsqlLocal");
});

// Add identity before registering services
builder.Services
    .AddIdentity<AppUser, AppRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllers();

// Add fluent validation
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

// Add Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed database roles and users
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

    await AppDbSeeder.SeedAsync(userManager, roleManager);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
