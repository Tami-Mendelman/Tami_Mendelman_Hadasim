using Microsoft.EntityFrameworkCore; // ����� ����� �-Entity Framework Core ������ ��� �������
using GroceryApp.API.Models; // ���� �� ������� Supplier, Product, Order �-OrderItem

namespace GroceryApp.API.Data
{
    public class AppDbContext : DbContext // ���� �-DbContext, ����� ����� �� EF ������ ���� ���� ������
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { } // ���������� ������ �� ��������� ������ �����, ����� ����������� ��� ��� ��� �������

        public DbSet<Supplier> Suppliers { get; set; } // ����� ���� �� ����� ���� �������
        public DbSet<Product> Products { get; set; } // ����� ���� �� ������
        public DbSet<Order> Orders { get; set; } // ����� ���� �� ������
        public DbSet<OrderItem> OrderItems { get; set; } // ����� ���� �� ������ ��� �����
    }
}
