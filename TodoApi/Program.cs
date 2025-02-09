// using Microsoft.EntityFrameworkCore;
// using Microsoft.OpenApi.Models;
// using TodoApi;

// var builder = WebApplication.CreateBuilder(args);

// //ההזרקה
// // builder.Services.AddDbContext<ToDoDbContext>(options =>
// //     options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
// //     new MySqlServerVersion(new Version(8,0,33))));

// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(builder.Configuration.GetConnectionString("server=buiercsasy7ihrkvjqvi-mysql.services.clever-cloud.com;port=3306;user=uazkdpcc8m7o3nrk;password=Dm8ZwOuwj3RmqJp34ZV2;database=buiercsasy7ihrkvjqvi"),
//     new MySqlServerVersion(new Version(8,0,33))));

// //cors:
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowClient", policy =>
//         policy.WithOrigins("https://authclient-f5q9.onrender.com") // התאימי ל-URL של הקליינט שלך!
//               .AllowAnyMethod()
//               .AllowAnyHeader());
// });

// //swagger:
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Title = "ToDo API",
//         Version = "v1"
//     });
// });

// var app = builder.Build();

// //הפעלת Swagger במצב Development
// // if (app.Environment.IsDevelopment())
// // {
//     app.UseSwagger();
//     app.UseSwaggerUI(c =>
//     {
//         c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API v1");
//     });
// //}

// app.UseCors("AllowAll");
// app.UseRouting();

// //functions:
// app.MapGet("/tasks", async (ToDoDbContext db) =>{
//   var tasks=await db.Items.ToListAsync();
//   return Results.Ok(tasks);
// });

// app.MapGet("/tasks/{id}", async (int id, ToDoDbContext db) => {
//     var task = await db.Items.FindAsync(id);
//     return task != null ? Results.Ok(task) : Results.NotFound();
// });

// app.MapPost("/tasks", async (ToDoDbContext db, Item newItem) =>
// {
//   db.Items.Add(newItem);
//   await db.SaveChangesAsync();
//   return Results.Created($"/tasks/{newItem.Id}", newItem);
// });

// app.MapPut("/tasks/{id}", async (int id, ToDoDbContext db,Item updateItem) =>
// {
//   var existItem = await db.Items.FindAsync(id);
//   if (existItem is null)
//     return Results.NotFound();
  
//   existItem.Name=updateItem.Name;
//   existItem.IsComplete=updateItem.IsComplete;

//   await db.SaveChangesAsync();
//   return Results.NoContent();
// });

// app.MapDelete("/tasks/{id}",async(int id,ToDoDbContext db)=>{
//   var deleteItem=await db.Items.FindAsync(id);
//   if(deleteItem is null)
//     return Results.NotFound();
  
//   db.Items.Remove(deleteItem);
//   await db.SaveChangesAsync();
//   return Results.NoContent();
// });

// app.MapGet("/",()=>"AuthServer Api is running!");

// app.Run();
using Microsoft.EntityFrameworkCore;
using TodoApi;
using Swashbuckle.AspNetCore.SwaggerGen;
var builder = WebApplication.CreateBuilder(args);

// חיבור ל-DB דרך appsettings.json
var connectionString = builder.Configuration.GetConnectionString("ToDoDB");

builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 41))));

// הגדרת CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "https://todolistreact-4nk8.onrender.com") // כאן תכתבי את ה-URL של ה-Client שלך, למשל אם זה React או Vue
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// הוספת Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// בדיקה אם מסד הנתונים קיים, ואם לא – יצירה אוטומטית
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
    db.Database.EnsureCreated();
}

// הוספת נתיבים (Routes) לטיפול במשימות
app.MapGet("/items", async (ToDoDbContext db) =>
    await db.Items.ToListAsync());

app.MapPost("/items", async (ToDoDbContext db, Item item) =>
{
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/items/{item.Id}", item);
});

app.MapPut("/items/{id}", async (ToDoDbContext db, int id, Item inputItem) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();

    item.Name = inputItem.Name;
    item.IsComplete = inputItem.IsComplete;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/items/{id}", async (ToDoDbContext db, int id) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();

    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// הפעלת השרת עם הגדרת CORS
app.UseCors("AllowSpecificOrigin"); // משתמשים בהגדרה "AllowSpecificOrigin"

// הפעלת Swagger
app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/",()=>"helloruti&rivki");
app.Run();