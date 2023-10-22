using Application.Interfaces;
using Application.UseCases;
using Infrastructure.Command;
using Infrastructure.Persistence.Context;
using Infrastructure.Query;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Custom
var connectionString = builder.Configuration["ConnectionString"];
builder.Services.AddDbContext<CineContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IFuncionesService, FuncionesService>();
builder.Services.AddScoped<IFuncionesCommand, FuncionesCommand>();
builder.Services.AddScoped<IFuncionesQuery, FuncionesQuery>();

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
