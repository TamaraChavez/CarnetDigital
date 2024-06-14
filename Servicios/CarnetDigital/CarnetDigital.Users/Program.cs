using CarnetDigital.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using CarnetDigital.Users;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Inyeccion de dependencias
builder.Services.AddDbContext<CarnetDigitalDbContext>(options =>
{
    options.UseSqlServer("name=ConnectionStrings:DefaultConnection");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapUsuarioEndpoints();


app.Run();

}
