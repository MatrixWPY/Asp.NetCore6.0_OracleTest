namespace OracleTest.Models.Data
{
    public class Product
    {
        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public string? Description { get; set; }

        public decimal? StandardCost { get; set; }

        public decimal? ListPrice { get; set; }

        public long CategoryId { get; set; }
    }
}
