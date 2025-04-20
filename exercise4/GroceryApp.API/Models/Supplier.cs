using System.Collections.Generic;


namespace GroceryApp.API.Models
{
	public class Supplier
	{
		public int Id { get; set; }
		public string CompanyName { get; set; }
		public string PhoneNumber { get; set; }
		public string RepresentativeName { get; set; }
        public List<Product> Products { get; set; } = new();
    }
}
