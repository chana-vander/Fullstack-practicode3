using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
//ההזרקה
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
    new MySqlServerVersion(new Version(8,0,33))));

var app = builder.Build();

app.MapGet("/tasks", async (ToDoDbContext db) =>{
  var items=await db.Items.ToListAsync();
  return Results.Ok(items);
});

app.Run();




// using Microsoft.EntityFrameworkCore;
// using TodoApi;

// var builder = WebApplication.CreateBuilder(args);

// // הוספת ToDoDbContext לשירותים עם MySQL
// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
//         new MySqlServerVersion(new Version(8, 0, 21)))); // ודא שהגרסה מתאימה לגרסה של MySQL שלך

// var app = builder.Build();

// // הגדרת הנתיב לקבלת משימות
// app.MapGet("/tasks", async (ToDoDbContext db) =>
// {
//     var items = await db.Items.ToListAsync();
//     return Results.Ok(items);
// });

// app.Run();
