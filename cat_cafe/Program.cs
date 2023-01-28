using Microsoft.EntityFrameworkCore;
using cat_cafe.Repositories;
using Serilog;
using Serilog.Sinks.File;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.File("log.txt").CreateLogger();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<CatContext>(opt => opt.UseInMemoryDatabase("CatCafe"));
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
