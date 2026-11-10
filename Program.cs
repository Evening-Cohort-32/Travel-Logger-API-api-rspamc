using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using TravelLogger;
using TravelLogger.Models;
using TravelLogger.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<TravelLoggerDbContext>(builder.Configuration["TravelLoggerDbConnectionString"]);

var app = builder.Build();

// Comment out HTTPS redirection for now to simplify testing
// app.UseHttpsRedirection();

// Configure Swagger for all environments during development
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});

//User Endpoints

//User Registration
app.MapPost("/api/users", (TravelLoggerDbContext db, User user) =>
{
    if (db.Users.Any(u => u.Email == user.Email))
    {
        return Results.BadRequest("A user with that email already exists");
    }

    db.Users.Add(user);
    db.SaveChanges();
    return Results.Created($"/api/users/{user.Id}", user);
});

//User Sign-in
app.MapGet("/api/users/signin/{email}", (TravelLoggerDbContext db, string email) =>
{
    User user = db.Users.SingleOrDefault(u => u.Email == email);
    if (user == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new UserDTO
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email
    });
});

//User by Id
app.MapGet("/api/users/{id}", (TravelLoggerDbContext db, int id) =>
{
    return db.Users
        .Select(u => new UserDTO
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Description = u.Description,
            ImageUrl = u.ImageUrl
        })
        .Single(u => u.Id == id);
});

//User Update
app.MapPut("/api/users/{id}", (TravelLoggerDbContext db, int id, int userId, User user) =>
{
    if (id != userId)
    {
        return Results.StatusCode(403);
    }

    User userToUpdate = db.Users.SingleOrDefault(u => u.Id == id);
    if (userToUpdate == null)
    {
        return Results.NotFound();
    }

    userToUpdate.Description = user.Description;
    userToUpdate.ImageUrl = user.ImageUrl;
    db.SaveChanges();
    return Results.NoContent();
});

//City Endpoints
app.MapGet("/api/cities", (TravelLoggerDbContext db) =>
{
    return db.Cities
    .Select(c => new CityDTO
    {
        Id = c.Id,
        Name = c.Name
    }).ToList();
});

//City by id
app.MapGet("/api/cities/{id}", (TravelLoggerDbContext db, int id) =>
{
    return db.Cities
        .Select(c => new CityDTO
        {
            Id = c.Id,
            Name = c.Name
        })
        .Single(c => c.Id == id);
});

app.Run();