using Microsoft.EntityFrameworkCore;
using PersonAPI.Data;
using PersonAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDbConnection"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// -------------------------------------------------------------------------------------------------
app.MapGet("api/v1/people", async (AppDbContext dbContext) =>
{
    var people = await dbContext.People.ToListAsync();
    return Results.Ok(people);
});

// -------------------------------------------------------------------------------------------------
app.MapGet("api/v1/people/{id}", async (AppDbContext dbContext, int id) =>
{
    var person = await dbContext.People.FindAsync(id);
    if (person == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(person);
});

// -------------------------------------------------------------------------------------------------
app.MapPost("api/v1/people", async (AppDbContext dbContext, Person person) =>
{
    await dbContext.People.AddAsync(person);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/api/v1/people/{person.Id}", person);
});

// -------------------------------------------------------------------------------------------------
app.MapPut("api/v1/people/{id}", async (AppDbContext dbContext, int id, Person person) =>
{
    var existingPerson = await dbContext.People.FindAsync(id);
    if (existingPerson == null)
    {
        return Results.NotFound();
    }

    existingPerson.FullName = person.FullName;
    existingPerson.Telephone = person.Telephone;
    existingPerson.DoB = person.DoB;

    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});

// -------------------------------------------------------------------------------------------------
app.MapDelete("api/v1/people/{id}", async (AppDbContext dbContext, int id) =>
{
    var existingPerson = await dbContext.People.FindAsync(id);
    if (existingPerson == null)
    {
        return Results.NotFound();
    }

    dbContext.People.Remove(existingPerson);
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();