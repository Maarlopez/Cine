using Application.IMappers;
using Application.Interfaces;
using Application.Mappers;
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
builder.Services.AddScoped<IFuncionMapper, FuncionMapper>();


builder.Services.AddScoped<IPeliculasService, PeliculasService>();
builder.Services.AddScoped<IPeliculasQuery, PeliculasQuery>();
builder.Services.AddScoped<IPeliculasCommand, PeliculasCommand>();
builder.Services.AddScoped<IPeliculaMapper, PeliculaMapper>();

builder.Services.AddScoped<ISalasService, SalasService>();
builder.Services.AddScoped<ISalasQuery, SalasQuery>();
builder.Services.AddScoped<ISalaMapper, SalaMapper>();

builder.Services.AddScoped<ITicketsService, TicketsService>();
builder.Services.AddScoped<ITicketQuery, TicketQuery>();
builder.Services.AddScoped<ITicketCommand, TicketsCommand>();
builder.Services.AddScoped<ITicketMapper, TicketMapper>();

builder.Services.AddScoped<IGenerosService, GenerosService>();
builder.Services.AddScoped<IGenerosQuery, GenerosQuery>();
builder.Services.AddScoped<IGeneroMapper, GeneroMapper>();


//CORS deshabilitar
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
