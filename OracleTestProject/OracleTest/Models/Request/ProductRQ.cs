using System.ComponentModel.DataAnnotations;

namespace OracleTest.Models.Request
{
    public class QueryProductRQ : PageInfoRQ
    {
        public long? CategoryId { get; set; }

        [RegularExpression("([0-9,]+)")]
        public string? CategoryIds { get; set; }

        public string? ProductName { get; set; }
    }

    public class InsertProductRQ
    {
        [Required(ErrorMessage = "{0} 為必填欄位。")]
        public string? ProductName { get; set; }

        public string? Description { get; set; }

        public decimal? StandardCost { get; set; }

        public decimal? ListPrice { get; set; }

        [Required(ErrorMessage = "{0} 為必填欄位。")]
        public long? CategoryId { get; set; }
    }

    public class UpdateProductRQ
    {
        public string? ProductName { get; set; }

        public string? Description { get; set; }

        public decimal? StandardCost { get; set; }

        public decimal? ListPrice { get; set; }

        public long? CategoryId { get; set; }
    }

    public class InsertProductAndCategoryRQ
    {
        [Required(ErrorMessage = "{0} 為必填欄位。")]
        public string? ProductName { get; set; }

        public string? Description { get; set; }

        public decimal? StandardCost { get; set; }

        public decimal? ListPrice { get; set; }

        [Required(ErrorMessage = "{0} 為必填欄位。")]
        public string? CategoryName { get; set; }
    }
}
