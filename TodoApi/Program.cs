using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
//ההזרקה
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
    new MySqlServerVersion(new Version(8,0,33))));

var app = builder.Build();
app.UseRouting();

//functions:
app.MapGet("/tasks", async (ToDoDbContext db) =>{
  var items=await db.Items.ToListAsync();
  return Results.Ok(items);
});

app.MapPost("/tasks", async (ToDoDbContext db, Item newItem) =>
{
  db.Items.Add(newItem);
  await db.SaveChangesAsync();
  return Results.Created($"/tasks/{newItem.Id}", newItem);
});

app.MapPut("/tasks/{id}", async (int id, ToDoDbContext db,Item updateItem) =>
{
  var existItem = await db.Items.FindAsync(id);
  if (existItem is null)
    return Results.NotFound();
  
  existItem.Name=updateItem.Name;
  existItem.IsComplete=updateItem.IsComplete;

  await db.SaveChangesAsync();
  return Results.NoContent();
});

app.MapDelete("/tasks/{id}",async(int id,ToDoDbContext db)=>{
  var deleteItem=await db.Items.FindAsync(id);
  if(deleteItem is null)
    return Results.NotFound();
  
  db.Items.Remove(deleteItem);
  await db.SaveChangesAsync();
  return Results.NoContent();
});

app.Run();
