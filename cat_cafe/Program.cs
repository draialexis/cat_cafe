using Microsoft.EntityFrameworkCore;
using cat_cafe.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<CatContext>(opt =>
opt.UseInMemoryDatabase("CatCafe"));
builder.Services.AddDbContext<BarContext>(opt =>
opt.UseInMemoryDatabase("CatCafe"));
builder.Services.AddDbContext<CustomerContext>(opt =>
opt.UseInMemoryDatabase("CatCafe"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
