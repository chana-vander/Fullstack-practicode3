using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

//ההזרקה
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
    new MySqlServerVersion(new Version(8,0,33))));

//cors:
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()    //מתיר קריאות מכל כתובת
              .AllowAnyMethod()    //מתיר GET, POST, PUT, DELETE וכו'
              .AllowAnyHeader());  //מתיר כל כותרות הבקשה
});

//swagger:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ToDo API",
        Version = "v1"
    });
});

var app = builder.Build();

//הפעלת Swagger במצב Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API v1");
    });
}
app.UseCors("AllowAll");
app.UseRouting();

//functions:
app.MapGet("/tasks", async (ToDoDbContext db) =>{
  var tasks=await db.Items.ToListAsync();
  return Results.Ok(tasks);
});

app.MapGet("/tasks/{id}", async (int id, ToDoDbContext db) => {
    var task = await db.Items.FindAsync(id);
    return task != null ? Results.Ok(task) : Results.NotFound();
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
