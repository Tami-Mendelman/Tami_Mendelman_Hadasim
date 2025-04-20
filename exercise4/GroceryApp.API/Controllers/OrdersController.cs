using Microsoft.AspNetCore.Mvc;
using GroceryApp.API.Models;
using GroceryApp.API.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace GroceryApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // הנתיב יהיה: api/Orders
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context; // גישה למסד הנתונים

        public OrdersController(AppDbContext context)
        {
            _context = context; // הזרקת התלות בקונסטרקטור
        }

        // יצירת הזמנה חדשה ע"י בעל המכולת
        [HttpPost]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            order.Status = "חדש"; // סטטוס ברירת מחדל להזמנה חדשה
            order.CreatedAt = DateTime.Now; // שמירת תאריך יצירת ההזמנה
            _context.Orders.Add(order); // הוספת ההזמנה למסד הנתונים
            _context.SaveChanges(); // שמירת השינויים
            return Ok(order); // החזרת ההזמנה שנוצרה
        }

        // שליפת כל ההזמנות
        [HttpGet]
        public IActionResult GetAll()
        {
            var orders = _context.Orders
                .Include(o => o.Items) // טען גם את פרטי המוצרים שבהזמנה
                .ThenInclude(i => i.Product) // טען גם את שם המוצר מתוך פריט ההזמנה
                .Include(o => o.Supplier) // טען את הספק של ההזמנה
                .ToList();

            return Ok(orders); // החזרת כל ההזמנות
        }

        // שליפת הזמנות לפי ספק
        [HttpGet("supplier/{supplierId}")]
        public IActionResult GetBySupplier(int supplierId)
        {
            var orders = _context.Orders
                .Where(o => o.SupplierId == supplierId) // סינון לפי מזהה ספק
                .Include(o => o.Items) // טען את פרטי הפריטים
                .ThenInclude(i => i.Product) // כולל המוצר בכל פריט
                .Include(o => o.Supplier) // כולל פרטי הספק
                .ToList();

            return Ok(orders); // החזרת ההזמנות של הספק
        }

        // אישור ע"י ספק
        [HttpPost("{id}/approve")]
        public IActionResult ApproveOrder(int id)
        {
            var order = _context.Orders.Find(id); // חיפוש ההזמנה לפי מזהה
            if (order == null) return NotFound(); // אם לא נמצאה – החזר 404

            order.Status = "בתהליך"; // עדכון סטטוס לאישור ע"י ספק
            _context.SaveChanges(); // שמירת השינוי במסד הנתונים

            return Ok(order); // החזרת ההזמנה המאושרת
        }

        // אישור קבלת הזמנה ע"י בעל המכולת
        [HttpPost("{id}/complete")]
        public IActionResult CompleteOrder(int id)
        {
            var order = _context.Orders.Find(id); // חיפוש ההזמנה לפי מזהה
            if (order == null) return NotFound(); // אם לא נמצאה – החזר 404

            order.Status = "הושלמה"; // שינוי סטטוס להזמנה שהושלמה
            _context.SaveChanges(); // שמירת השינוי

            return Ok(order); // החזרת ההזמנה
        }
    }
}
