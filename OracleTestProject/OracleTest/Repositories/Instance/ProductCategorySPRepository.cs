using Dapper;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using OracleTest.Models.Data;
using OracleTest.Models.Response;
using OracleTest.Repositories.Interface;
using OracleTest.Utility;
using System.Data;

namespace OracleTest.Repositories.Instance
{
    public class ProductCategorySPRepository : IProductCategoryRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProductCategorySPRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public GetProductCategoryRP GetProductCategory(long id)
        {
            try
            {
                var dynamicParam = new OracleDynamicParameters();
                dynamicParam.Add("i_categoryId", id, OracleDbType.Int64, ParameterDirection.Input);
                dynamicParam.Add("o_result", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

                var res = _dbConnection.QueryFirstOrDefault<GetProductCategoryRP>(
                    "SP_GETPRODUCTCATEGORY",
                    dynamicParam,
                    commandType: CommandType.StoredProcedure
                );

                return res;
            }
            catch
            {
                throw;
            }
        }

        public bool InsertProductCategory(ProductCategory objProductCategory)
        {
            try
            {
                var dynamicParam = new OracleDynamicParameters();
                dynamicParam.Add("i_categoryName", objProductCategory.CategoryName, OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("o_categoryId", dbType: OracleDbType.Int64, direction: ParameterDirection.Output);

                _dbConnection.Execute(
                    "SP_INSERTPRODUCTCATEGORY",
                    dynamicParam,
                    commandType: CommandType.StoredProcedure
                );
                objProductCategory.CategoryId = (long)(dynamicParam.Get<OracleDecimal?>("o_categoryId") ?? 0);

                return objProductCategory.CategoryId > 0;
            }
            catch
            {
                throw;
            }
        }
    }
}
