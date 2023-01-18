using EtteplanMORE.ServiceManual.ApplicationCore;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.ApplicationCore.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IFactoryDeviceService, FactoryDeviceService>();
builder.Services.AddScoped<IMaintenanceTaskService, MaintenanceTaskService>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();