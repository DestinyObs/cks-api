

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Models;
using cks_kaas.Data;


var builder = WebApplication.CreateBuilder(args);


// Add DbContext and Identity
// Register UserService, TenantService, and AuthService for DI
builder.Services.AddScoped<Services.UserService>();
builder.Services.AddScoped<Services.TenantService>();
builder.Services.AddScoped<Services.AuthService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add Controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});



var app = builder.Build();


// Seed Provider Tenant and Provider Admin
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    var providerTenantName = "Provider";
    var providerTenantEmail = "provider@kaas.local";
    var providerAdminEmail = "provideradmin@kaas.local";
    var providerAdminPassword = "ChangeMe123!";
    var providerAdminRole = "ProviderAdmin";

    // Ensure Provider Tenant exists
    var providerTenant = db.Tenants.FirstOrDefault(t => t.Name == providerTenantName);
    if (providerTenant == null)
    {
        providerTenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = providerTenantName,
            AdminEmail = providerTenantEmail,
            CreatedAt = DateTime.UtcNow
        };
        db.Tenants.Add(providerTenant);
        db.SaveChanges();
    }

    // Ensure ProviderAdmin role exists
    if (!await roleManager.RoleExistsAsync(providerAdminRole))
    {
        await roleManager.CreateAsync(new IdentityRole<Guid>(providerAdminRole));
    }

    // Ensure ProviderAdmin user exists
    var adminUser = await userManager.FindByEmailAsync(providerAdminEmail);
    if (adminUser == null)
    {
        adminUser = new User
        {
            UserName = providerAdminEmail,
            Email = providerAdminEmail,
            EmailConfirmed = true,
            Role = providerAdminRole,
            Name = "Provider Admin",
            TenantId = providerTenant.Id,
            JoinDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var result = await userManager.CreateAsync(adminUser, providerAdminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, providerAdminRole);
        }
    }
}



// Enable Swagger in all environments
app.UseSwagger();
app.UseSwaggerUI();

// Enable routing and controllers
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
