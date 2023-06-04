namespace Common.Models
{
    public class OrderDetail
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; } // Quantity of this product in the order
        public float Price => Quantity * Product.Price;

        public OrderDetail()
        {
            Quantity = 1;
        }
    }
}