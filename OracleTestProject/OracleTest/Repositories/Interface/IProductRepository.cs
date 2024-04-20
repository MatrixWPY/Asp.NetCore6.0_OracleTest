using OracleTest.Models.Data;
using OracleTest.Models.Response;

namespace OracleTest.Repositories.Interface
{
    public interface IProductRepository
    {
        GetProductRP GetProduct(long id);

        (int, IEnumerable<QueryProductRP>) QueryProductV1(Dictionary<string, object> dicParams);

        (int, IEnumerable<QueryProductRP>) QueryProductV2(Dictionary<string, object> dicParams);

        (int, IEnumerable<QueryProductRP>) QueryProductV3(Dictionary<string, object> dicParams);

        (int, IEnumerable<QueryProductRP>) QueryProductV4(Dictionary<string, object> dicParams);

        (int, IEnumerable<QueryProductRP>) QueryProductV5(Dictionary<string, object> dicParams);

        (int, IEnumerable<QueryProductRP>) QueryProductV6(Dictionary<string, object> dicParams);

        bool InsertProduct(Product objProduct);

        bool UpdateProduct(Product objProduct);

        bool DeleteProduct(IEnumerable<long> ids);

        bool InsertProductAndCategory(Product objProduct, ProductCategory objProductCategory);
    }
}
