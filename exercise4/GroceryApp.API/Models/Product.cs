using System.Text.Json.Serialization;
namespace GroceryApp.API.Models
{


    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int MinOrderQuantity { get; set; }

        public int SupplierId { get; set; }

        [JsonIgnore]//  ← זה מונע בעיות במודל שלא יווצר הפניה מעגלית
        public Supplier? Supplier { get; set; }
    }
}