using GroceryApp.API.Data;
using Microsoft.EntityFrameworkCore; 

var builder = WebApplication.CreateBuilder(args); // בונה את אובייקט התצורה וה-Services לאפליקציה

builder.Services.AddCors(options => // מוסיף שירות CORS לאפשר תקשורת עם ה-Frontend
{
    options.AddPolicy("AllowReact", // מגדיר מדיניות בשם AllowReact
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // מאפשר בקשות רק מה-React שרץ על פורט 3000
                  .AllowAnyHeader() // מתיר כל Header בבקשות
                  .AllowAnyMethod(); // מתיר כל מתודת HTTP (GET, POST, PUT, DELETE וכו')
        });
});

builder.Services.AddDbContext<AppDbContext>(options => // רישום AppDbContext לשימוש ב-Entity Framework
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); // שימוש במסד SQLite עם מחרוזת חיבור מ-appsettings

builder.Services.AddControllers(); // ? זו השורה החשובה: רישום Controllers כדי שה-APIים יעבדו

builder.Services.AddEndpointsApiExplorer(); // מאפשר ל-Swagger לזהות את ה-APIים
builder.Services.AddSwaggerGen(); // יוצר דוקומנטציה אוטומטית ל-API בעזרת Swagger

var app = builder.Build(); // בונה את האפליקציה על בסיס ההגדרות והשירותים

app.UseCors("AllowReact"); // מפעיל את מדיניות ה-CORS שהוגדרה קודם כדי ש-React יוכל לשלוח בקשות

if (app.Environment.IsDevelopment()) // אם מדובר בסביבת פיתוח
{
    app.UseSwagger(); // מייצר את מסמכי ה-Swagger (OpenAPI)
    app.UseSwaggerUI(); // מציג ממשק משתמש לבדיקת ה-API בדפדפן
}

app.UseHttpsRedirection(); // מבצע הפניה אוטומטית מבקשות HTTP ל-HTTPS

app.MapControllers(); // ? גם זו קריטית: ממפה את ה-Controllers לנתיבים (Routes) באפליקציה

app.Run(); // מפעיל את האפליקציה ומאזין לבקשות
