using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;

namespace GroceryApp.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }

        public Supplier? Supplier { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; } = "";

        public List<OrderItem> Items { get; set; } = new();
    }
}
