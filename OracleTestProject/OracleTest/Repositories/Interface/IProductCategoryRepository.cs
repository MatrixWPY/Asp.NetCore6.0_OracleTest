using OracleTest.Models.Data;
using OracleTest.Models.Response;

namespace OracleTest.Repositories.Interface
{
    public interface IProductCategoryRepository
    {
        GetProductCategoryRP GetProductCategory(long id);

        bool InsertProductCategory(ProductCategory objProductCategory);
    }
}
