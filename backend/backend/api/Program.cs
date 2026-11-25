using api.Dtos;
using api.Dtos.Mappers;
using api.Dtos.model;
using Domain.model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<KanjiDbContext>();

        builder.Services.AddScoped<IGenericMapper<UserDto, User>, UserMapper>();
        builder.Services.AddScoped<IGenericMapper<CardDto, Card>, CardMapper>();
        builder.Services.AddScoped<IGenericMapper<ItemDto, Item>, ItemMapper>();
        builder.Services.AddScoped<IGenericMapper<ItemReadingDto, ItemReading>, ItemReadingMapper>();
        builder.Services.AddScoped<IGenericMapper<ReviewDto, Review>, ReviewMapper>();
        
        builder.Services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-XSRF-TOKEN";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Lax;
        });

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<
            IPasswordHasher<User>,
            PasswordHasher<User>>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Frontend", policy =>
            {
                policy
                    .WithOrigins(
                        "http://localhost:4200",
                        "https://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        builder.Services
            .AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "KanjiDojo.Auth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;

                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;

                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };

                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        var app = builder.Build();

        if (args.Contains("--seed"))
        {
            if (!app.Environment.IsDevelopment())
            {
                throw new InvalidOperationException(
                    "Database seeding is only allowed in development.");
            }

            using var scope = app.Services.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<KanjiDbContext>();

            await DatabaseSeeder.SeedAsync(db);

            return;
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors("Frontend");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}