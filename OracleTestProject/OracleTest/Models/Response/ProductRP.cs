using OracleTest.Utility;

namespace OracleTest.Models.Response
{
    public class GetProductRP
    {
        [Column(Name = "PRODUCT_ID")]
        public long ProductId { get; set; }

        [Column(Name = "PRODUCT_NAME")]
        public string ProductName { get; set; }

        public string? Description { get; set; }

        [Column(Name = "STANDARD_COST")]
        public decimal? StandardCost { get; set; }

        [Column(Name = "LIST_PRICE")]
        public decimal? ListPrice { get; set; }

        [Column(Name = "CATEGORY_ID")]
        public long CategoryId { get; set; }

        [Column(Name = "CATEGORY_NAME")]
        public string CategoryName { get; set; }

        public int Quantity { get; set; }
    }

    public class QueryProductRP : GetProductRP
    {
        
    }

    public class QueryProductV1RP : QueryProductRP
    {

    }

    public class QueryProductV2RP : QueryProductRP
    {

    }

    public class QueryProductV3RP : QueryProductRP
    {
        public int TotalCnt { get; set; }
    }

    public class QueryProductV4RP : QueryProductV3RP
    {

    }

    public class QueryProductV5RP : QueryProductV3RP
    {

    }

    public class QueryProductV6RP : QueryProductRP
    {

    }
}
