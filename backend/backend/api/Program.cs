using api.Dtos;
using api.Dtos.Mappers;
using api.Dtos.model;
using Domain.model;
using Microsoft.EntityFrameworkCore;

namespace api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddDbContext<KanjiDbContext>();
        builder.Services.AddScoped<IGenericMapper<UserDto, User>, UserMapper>();
        builder.Services.AddScoped<IGenericMapper<CardDto, Card>, CardMapper>();
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}