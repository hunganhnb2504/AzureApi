using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TodoContext")));
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
async Task<List<ToDo>> GetAllToDo(DataContext context)=> await context.ToDo.ToListAsync();
app.MapGet("/Todo", async (DataContext context) => await context.ToDo.ToListAsync());
app.MapGet("/Todo/{id}", async (DataContext context, int id) => await context.ToDo.FindAsync(id) is ToDo Item ? Results.Ok(Item) : Results.NotFound("Item not found"));
app.MapPost("Add/ToDo", async (DataContext context, ToDo Item) =>
{
    context.ToDo.Add(Item);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllToDo(context));
});

app.MapPut("/Todo/{id}", async (DataContext context, ToDo Item,int id) =>
{
    var toDoItem = await context.ToDo.FindAsync(id);
    if (toDoItem != null) return Results.NotFound("Item not found");
    toDoItem.Description =  Item.Description;
    toDoItem.itemName = Item.itemName;
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllToDo(context));
});

app.MapDelete("/Delete/{id}", async (DataContext context, int id) =>
{
    var toDoItem = await context.ToDo.FindAsync(id);
    if (toDoItem != null) return Results.NotFound("Item not found");
    context.ToDo.Remove(toDoItem);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllToDo(context));
});

app.Run();
