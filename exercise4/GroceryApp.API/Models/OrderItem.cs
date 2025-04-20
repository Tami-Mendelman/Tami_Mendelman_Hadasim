using System.Text.Json.Serialization;

namespace GroceryApp.API.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        [JsonIgnore]//  זה מונע בעיות במודל שלא יווצר הפניה מעגלית
        public Order? Order { get; set; }

        public int ProductId { get; set; }

        public Product? Product { get; set; }

        public int Quantity { get; set; }
    }
}
