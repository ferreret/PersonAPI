using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PersonAPI.Data;
using PersonAPI.Dtos;
using PersonAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDbConnection"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// -------------------------------------------------------------------------------------------------
app.MapGet("api/v1/people", async (AppDbContext dbContext, IMapper mapper) =>
{
    var people = await dbContext.People.ToListAsync();

    return Results.Ok(mapper.Map<IEnumerable<PersonReadDto>>(people));
});

// -------------------------------------------------------------------------------------------------
app.MapGet("api/v1/people/{id}", async (AppDbContext dbContext, int id, IMapper mapper) =>
{
    var personModel = await dbContext.People.FindAsync(id);

    if (personModel == null) return Results.NotFound();

    // var personDto = new PersonDto
    // {
    //     Id = personModel.Id,
    //     FullName = personModel.FullName,
    //     Telephone = personModel.Telephone
    // };

    var personDto = mapper.Map<PersonReadDto>(personModel);

    return Results.Ok(personDto);
});

// -------------------------------------------------------------------------------------------------
app.MapPost("api/v1/people", async (AppDbContext dbContext,
                                    PersonCreateDto personCreateDto,
                                    IMapper mapper) =>
{
    var personModel = mapper.Map<Person>(personCreateDto);
    await dbContext.People.AddAsync(personModel);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/api/v1/people/{personModel.Id}", mapper.Map<PersonReadDto>(personModel));
});

// -------------------------------------------------------------------------------------------------
app.MapPut("api/v1/people/{id}", async (AppDbContext dbContext,
                                        int id,
                                        PersonUpdateDto personUpdateDto,
                                        IMapper mapper) =>
{
    var existingPerson = await dbContext.People.FindAsync(id);
    if (existingPerson == null)
    {
        return Results.NotFound();
    }

    // existingPerson.FullName = person.FullName;
    // existingPerson.Telephone = person.Telephone;
    // existingPerson.DoB = person.DoB;

    mapper.Map(personUpdateDto, existingPerson);

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