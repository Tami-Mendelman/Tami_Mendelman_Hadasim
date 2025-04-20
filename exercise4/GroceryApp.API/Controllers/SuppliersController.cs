using Microsoft.AspNetCore.Mvc;
using GroceryApp.API.Models;
using GroceryApp.API.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GroceryApp.API.Controllers
{
    // מודל עזר לבקשת התחברות
    public class LoginRequest
    {
        public string PhoneNumber { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")] // הנתיב: api/Suppliers
    public class SuppliersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SuppliersController(AppDbContext context)
        {
            _context = context; // הזרקת הקשר למסד הנתונים
        }

        // רישום ספק חדש
        [HttpPost("register")]
        public IActionResult RegisterSupplier([FromBody] Supplier supplier)
        {
            // כדי למנוע יצירה כפולה של קשרים בין ספק למוצרים
            foreach (var product in supplier.Products)
            {
                product.Supplier = null; // מנקה את השדה כדי ש-EF לא יחשוב שמדובר בספק חדש
            }

            _context.Suppliers.Add(supplier); // מוסיף את הספק למסד
            _context.SaveChanges(); // שומר את השינויים

            return Ok(supplier); // מחזיר את הספק עם סטטוס 200 OK
        }

        // מחיקת ספק לפי מזהה
        [HttpDelete("{id}")]
        public IActionResult DeleteSupplier(int id)
        {
            var supplier = _context.Suppliers
                .Include(s => s.Products) // טוען גם את המוצרים של הספק
                .FirstOrDefault(s => s.Id == id); // מחפש את הספק לפי ID

            if (supplier == null)
                return NotFound(); // אם לא נמצא - מחזיר 404

            // מוחק את כל המוצרים של הספק לפני שמוחק אותו
            _context.Products.RemoveRange(supplier.Products);
            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();

            return Ok(new { message = "ספק נמחק בהצלחה" });
        }

        // התחברות ספק לפי מספר טלפון
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var supplier = _context.Suppliers
                .FirstOrDefault(s => s.PhoneNumber == request.PhoneNumber); // חיפוש ספק לפי טלפון

            if (supplier == null)
                return Unauthorized("Supplier not found"); // אם לא נמצא - מחזיר 401

            return Ok(supplier); // אם נמצא - מחזיר את הספק
        }

        // שליפת כל הספקים
        [HttpGet]
        public IActionResult GetAllSuppliers()
        {
            var suppliers = _context.Suppliers.ToList(); // מחזיר רשימה של כל הספקים
            return Ok(suppliers);
        }

        // שליפת ספק לפי מזהה
        [HttpGet("{id}")]
        public IActionResult GetSupplier(int id)
        {
            var supplier = _context.Suppliers
                .FirstOrDefault(s => s.Id == id); // חיפוש ספק לפי ID

            if (supplier == null)
                return NotFound(); // אם לא נמצא - מחזיר 404

            return Ok(supplier); // מחזיר את הספק
        }
    }
}
