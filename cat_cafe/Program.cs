using Microsoft.EntityFrameworkCore;
using cat_cafe.Repositories;
using Serilog;
using Serilog.Sinks.File;
using System.Diagnostics;
using cat_cafe.Entities;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.File("log.txt").CreateLogger();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<CatCafeDbContext>();
//builder.Services.AddDbContext<CatContext>(opt =>
//opt.UseInMemoryDatabase("CatCafe"));
//builder.Services.AddDbContext<BarContext>(opt =>
//opt.UseSqlite("CatCafe"));
//builder.Services.AddDbContext<CatCafeDbContext>(opt =>
//opt.UseSqlite("$Data Source ={CatCafe}"));

try
{
    // DB stuff when the app opens
    using (CatCafeDbContext db = new())
    {
        
    }
}
catch (Exception ex) { Console.WriteLine($"{ex.Message}\n... Couldn't use the database"); }



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllersWithViews();

var app = builder.Build();

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
