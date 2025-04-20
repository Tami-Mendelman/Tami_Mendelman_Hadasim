using Microsoft.AspNetCore.Mvc;
using GroceryApp.API.Models;
using GroceryApp.API.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GroceryApp.API.Controllers
{
    // ���� ��� ����� �������
    public class LoginRequest
    {
        public string PhoneNumber { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")] // �����: api/Suppliers
    public class SuppliersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SuppliersController(AppDbContext context)
        {
            _context = context; // ����� ���� ���� �������
        }

        // ����� ��� ���
        [HttpPost("register")]
        public IActionResult RegisterSupplier([FromBody] Supplier supplier)
        {
            // ��� ����� ����� ����� �� ����� ��� ��� �������
            foreach (var product in supplier.Products)
            {
                product.Supplier = null; // ���� �� ���� ��� �-EF �� ����� ������ ���� ���
            }

            _context.Suppliers.Add(supplier); // ����� �� ���� ����
            _context.SaveChanges(); // ���� �� ��������

            return Ok(supplier); // ����� �� ���� �� ����� 200 OK
        }

        // ����� ��� ��� ����
        [HttpDelete("{id}")]
        public IActionResult DeleteSupplier(int id)
        {
            var supplier = _context.Suppliers
                .Include(s => s.Products) // ���� �� �� ������� �� ����
                .FirstOrDefault(s => s.Id == id); // ���� �� ���� ��� ID

            if (supplier == null)
                return NotFound(); // �� �� ���� - ����� 404

            // ���� �� �� ������� �� ���� ���� ����� ����
            _context.Products.RemoveRange(supplier.Products);
            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();

            return Ok(new { message = "��� ���� ������" });
        }

        // ������� ��� ��� ���� �����
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var supplier = _context.Suppliers
                .FirstOrDefault(s => s.PhoneNumber == request.PhoneNumber); // ����� ��� ��� �����

            if (supplier == null)
                return Unauthorized("Supplier not found"); // �� �� ���� - ����� 401

            return Ok(supplier); // �� ���� - ����� �� ����
        }

        // ����� �� ������
        [HttpGet]
        public IActionResult GetAllSuppliers()
        {
            var suppliers = _context.Suppliers.ToList(); // ����� ����� �� �� ������
            return Ok(suppliers);
        }

        // ����� ��� ��� ����
        [HttpGet("{id}")]
        public IActionResult GetSupplier(int id)
        {
            var supplier = _context.Suppliers
                .FirstOrDefault(s => s.Id == id); // ����� ��� ��� ID

            if (supplier == null)
                return NotFound(); // �� �� ���� - ����� 404

            return Ok(supplier); // ����� �� ����
        }
    }
}
