using GroceryApp.API.Data;
using Microsoft.EntityFrameworkCore; 

var builder = WebApplication.CreateBuilder(args); // ���� �� ������� ������ ��-Services ���������

builder.Services.AddCors(options => // ����� ����� CORS ����� ������ �� �-Frontend
{
    options.AddPolicy("AllowReact", // ����� ������� ��� AllowReact
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // ����� ����� �� ��-React ��� �� ���� 3000
                  .AllowAnyHeader() // ���� �� Header ������
                  .AllowAnyMethod(); // ���� �� ����� HTTP (GET, POST, PUT, DELETE ���')
        });
});

builder.Services.AddDbContext<AppDbContext>(options => // ����� AppDbContext ������ �-Entity Framework
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); // ����� ���� SQLite �� ������ ����� �-appsettings

builder.Services.AddControllers(); // ? �� ����� ������: ����� Controllers ��� ��-API�� �����

builder.Services.AddEndpointsApiExplorer(); // ����� �-Swagger ����� �� �-API��
builder.Services.AddSwaggerGen(); // ���� ���������� �������� �-API ����� Swagger

var app = builder.Build(); // ���� �� ��������� �� ���� ������� ���������

app.UseCors("AllowReact"); // ����� �� ������� �-CORS ������� ���� ��� �-React ���� ����� �����

if (app.Environment.IsDevelopment()) // �� ����� ������ �����
{
    app.UseSwagger(); // ����� �� ����� �-Swagger (OpenAPI)
    app.UseSwaggerUI(); // ���� ���� ����� ������ �-API ������
}

app.UseHttpsRedirection(); // ���� ����� �������� ������ HTTP �-HTTPS

app.MapControllers(); // ? �� �� ������: ���� �� �-Controllers ������� (Routes) ���������

app.Run(); // ����� �� ��������� ������ ������
