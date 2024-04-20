using OracleTest.Models.Request;
using OracleTest.Models.Response;

namespace OracleTest.Services.Interface
{
    public interface IProductService
    {
        GetProductRP GetProduct(long id);

        PageDataRP<IEnumerable<QueryProductRP>> QueryProduct(QueryProductRQ request, string version);

        GetProductRP InsertProduct(InsertProductRQ request);

        GetProductRP UpdateProduct(long id, UpdateProductRQ request);

        bool DeleteProduct(IEnumerable<long> ids);

        GetProductRP InsertProductAndCategoryV1(InsertProductAndCategoryRQ request);

        GetProductRP InsertProductAndCategoryV2(InsertProductAndCategoryRQ request);
    }
}
