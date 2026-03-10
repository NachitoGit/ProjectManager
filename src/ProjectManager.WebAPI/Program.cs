using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Infrastructure.Persistence.Repositories;
using ProjectManager.Infrastructure.Persistence;
using ProjectManager.Application.Features.Projects.Commands.CreateProject;
using ProjectManager.Application.Mappings;
using FluentValidation;
using ProjectManager.Application;
using MediatR;
using ProjectManager.Application.Pipelines;
using ProjectManager.Application.Services;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Filters;
using ProjectManager.Infrastructure;
using ProjectManager.WebAPI.Middlewares;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Serilog;

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        Description = "Ingresa 'Bearer ' [espacio] y luego tu token. Ejemplo: Bearer eyJhbGciOi..."
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});



var app = builder.Build();

app.UseSerilogRequestLogging();




// **SEED DATA**
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await ApplicationDbContextSeed.SeedEssentialsAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
// **SEED DATA**


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();

try
{
    Log.Information("Iniciando la Web API de ProjectManager...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al iniciar");
}
finally
{
    Log.CloseAndFlush();
}

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();
app.Run();
