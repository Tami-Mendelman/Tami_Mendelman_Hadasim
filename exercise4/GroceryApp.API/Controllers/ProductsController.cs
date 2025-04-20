using Microsoft.AspNetCore.Mvc;
using GroceryApp.API.Models;
using GroceryApp.API.Data;
using Microsoft.EntityFrameworkCore;

namespace GroceryApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // קובע שהכתובת תהיה: api/Products
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context; // משתנה לשמירת הקשר למסד הנתונים

        public ProductsController(AppDbContext context)
        {
            _context = context; // הזרקת תלות בקונסטרקטור - מאפשר גישה ל-DbContext
        }

        // שליפת כל המוצרים של ספק מסוים
        [HttpGet("supplier/{supplierId}")] // הנתיב: api/Products/supplier/123
        public IActionResult GetProductsBySupplier(int supplierId)
        {
            var products = _context.Products
                .Where(p => p.SupplierId == supplierId) // סינון מוצרים לפי מזהה ספק
                .ToList(); // המרה לרשימה

            return Ok(products); // החזרת הרשימה בתגובה מוצלחת (200 OK)
        }
    }
}
