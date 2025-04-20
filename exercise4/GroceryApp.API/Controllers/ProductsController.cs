using Microsoft.AspNetCore.Mvc;
using GroceryApp.API.Models;
using GroceryApp.API.Data;
using Microsoft.EntityFrameworkCore;

namespace GroceryApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // ���� ������� ����: api/Products
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context; // ����� ������ ���� ���� �������

        public ProductsController(AppDbContext context)
        {
            _context = context; // ����� ���� ����������� - ����� ���� �-DbContext
        }

        // ����� �� ������� �� ��� �����
        [HttpGet("supplier/{supplierId}")] // �����: api/Products/supplier/123
        public IActionResult GetProductsBySupplier(int supplierId)
        {
            var products = _context.Products
                .Where(p => p.SupplierId == supplierId) // ����� ������ ��� ���� ���
                .ToList(); // ���� ������

            return Ok(products); // ����� ������ ������ ������ (200 OK)
        }
    }
}
