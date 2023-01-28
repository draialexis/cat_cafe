using Microsoft.EntityFrameworkCore;
using cat_cafe.Repositories;
using Serilog;
using Serilog.Sinks.File;

var builder = WebApplication.CreateBuilder(args);

/*
ILoggerFactory loggerFactory=new LoggerFactory();
loggerFactory.AddFile($@"{Directory.GetCurrentDirectory}\Logs\logs.txt");
*/

Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.File("log.txt").CreateLogger();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<CatContext>(opt =>
opt.UseInMemoryDatabase("CatCafe"));
builder.Services.AddDbContext<BarContext>(opt =>
opt.UseSqlite("CatCafe"));
builder.Services.AddDbContext<CustomerContext>(opt =>
opt.UseInMemoryDatabase("CatCafe"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllersWithViews();

var app = builder.Build();

/*var loggerFactory = app.Services.GetService<ILoggerFactory>();
loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());

app.Services.GetService<ILoggerFactory>().AddFile(builder.Configuration["Logging:LogFilePath"].ToString());*/

app.UseHttpLogging();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

Log.Information("program start");
app.Run();
