namespace PolysuranceTest.Models
{
    public class Model
    {
    }

    public class Product
    {
        public int Sku { get; set; }
        public decimal Price { get; set; }
    }

    // Discount.cs
    public class Discount
    {
        public string Key { get; set; }
        public decimal Value { get; set; }
    }

    // Order.cs
    public class Order
    {
        public int OrderId { get; set; }
        public string Discount { get; set; }
        public List<OrderItem> Items { get; set; }
    }

    // OrderItem.cs
    public class OrderItem
    {
        public int Sku { get; set; }
        public int Quantity { get; set; }
    }
}
