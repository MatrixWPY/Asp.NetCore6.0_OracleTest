using OracleTest.Utility;

namespace OracleTest.Models.Response
{
    public class GetProductCategoryRP
    {
        [Column(Name = "CATEGORY_ID")]
        public long CategoryId { get; set; }

        [Column(Name = "CATEGORY_NAME")]
        public string CategoryName { get; set; }
    }
}
