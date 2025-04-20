using Microsoft.EntityFrameworkCore; // מאפשר שימוש ב-Entity Framework Core לניהול מסד הנתונים
using GroceryApp.API.Models; // כולל את המחלקות Supplier, Product, Order ו-OrderItem

namespace GroceryApp.API.Data
{
    public class AppDbContext : DbContext // יורש מ-DbContext, מחלקת הבסיס של EF לניהול גישה למסד נתונים
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { } // קונסטרקטור שמעביר את האפשרויות למחלקת הבסיס, מאפשר קונפיגורציה כמו סוג מסד הנתונים

        public DbSet<Supplier> Suppliers { get; set; } // מייצג טבלה של ספקים במסד הנתונים
        public DbSet<Product> Products { get; set; } // מייצג טבלה של מוצרים
        public DbSet<Order> Orders { get; set; } // מייצג טבלה של הזמנות
        public DbSet<OrderItem> OrderItems { get; set; } // מייצג טבלה של פריטים בכל הזמנה
    }
}
