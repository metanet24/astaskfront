namespace FrontEndProject_Connect_BackEnd.Models
{
    public class Product : BaseEntity
    {
        public List<ProductImages> ProductImages { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
