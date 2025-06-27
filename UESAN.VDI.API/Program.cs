using Microsoft.EntityFrameworkCore;
using UESAN.VDI.CORE.Core.Interfaces;
using UESAN.VDI.CORE.Core.Services;
using UESAN.VDI.CORE.Infrastructure.data;
using UESAN.VDI.CORE.Infrastructure.Repositories;
using UESAN.VDI.CORE.Infrastructure.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var _configuration = builder.Configuration;
var _connectionString = _configuration
                        .GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<VdiDbContext>(options =>
    options.UseSqlServer(_connectionString));
//Add interfaces
builder.Services.AddTransient<ILineasInvestigacionService, LineasInvestigacionService>();
builder.Services.AddTransient<ILineasInvestigacionRepository, LineasInvestigacionRepository>();



builder.Services.AddSharedInfrastructure(_configuration);
//Add cors policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        var frontendUrl = _configuration.GetValue<string>("FrontendUrl");
        builder//WithOrigins(frontendUrl)
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });

});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.UseCors(); // Use CORS policy

app.MapControllers();

app.Run();
