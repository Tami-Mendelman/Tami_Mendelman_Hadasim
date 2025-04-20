using Microsoft.AspNetCore.Mvc;
using GroceryApp.API.Models;
using GroceryApp.API.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace GroceryApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // ����� ����: api/Orders
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context; // ���� ���� �������

        public OrdersController(AppDbContext context)
        {
            _context = context; // ����� ����� �����������
        }

        // ����� ����� ���� �"� ��� ������
        [HttpPost]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            order.Status = "���"; // ����� ����� ���� ������ ����
            order.CreatedAt = DateTime.Now; // ����� ����� ����� ������
            _context.Orders.Add(order); // ����� ������ ���� �������
            _context.SaveChanges(); // ����� ��������
            return Ok(order); // ����� ������ ������
        }

        // ����� �� �������
        [HttpGet]
        public IActionResult GetAll()
        {
            var orders = _context.Orders
                .Include(o => o.Items) // ��� �� �� ���� ������� �������
                .ThenInclude(i => i.Product) // ��� �� �� �� ����� ���� ���� ������
                .Include(o => o.Supplier) // ��� �� ���� �� ������
                .ToList();

            return Ok(orders); // ����� �� �������
        }

        // ����� ������ ��� ���
        [HttpGet("supplier/{supplierId}")]
        public IActionResult GetBySupplier(int supplierId)
        {
            var orders = _context.Orders
                .Where(o => o.SupplierId == supplierId) // ����� ��� ���� ���
                .Include(o => o.Items) // ��� �� ���� �������
                .ThenInclude(i => i.Product) // ���� ����� ��� ����
                .Include(o => o.Supplier) // ���� ���� ����
                .ToList();

            return Ok(orders); // ����� ������� �� ����
        }

        // ����� �"� ���
        [HttpPost("{id}/approve")]
        public IActionResult ApproveOrder(int id)
        {
            var order = _context.Orders.Find(id); // ����� ������ ��� ����
            if (order == null) return NotFound(); // �� �� ����� � ���� 404

            order.Status = "������"; // ����� ����� ������ �"� ���
            _context.SaveChanges(); // ����� ������ ���� �������

            return Ok(order); // ����� ������ �������
        }

        // ����� ���� ����� �"� ��� ������
        [HttpPost("{id}/complete")]
        public IActionResult CompleteOrder(int id)
        {
            var order = _context.Orders.Find(id); // ����� ������ ��� ����
            if (order == null) return NotFound(); // �� �� ����� � ���� 404

            order.Status = "������"; // ����� ����� ������ �������
            _context.SaveChanges(); // ����� ������

            return Ok(order); // ����� ������
        }
    }
}
