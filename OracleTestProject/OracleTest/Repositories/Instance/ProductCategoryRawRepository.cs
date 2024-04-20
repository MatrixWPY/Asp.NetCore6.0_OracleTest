using Dapper;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using OracleTest.Models.Data;
using OracleTest.Models.Response;
using OracleTest.Repositories.Interface;
using OracleTest.Utility;
using System.Data;
using System.Text;

namespace OracleTest.Repositories.Instance
{
    public class ProductCategoryRawRepository : IProductCategoryRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProductCategoryRawRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public GetProductCategoryRP GetProductCategory(long id)
        {
            try
            {
                var dynamicParam = new OracleDynamicParameters();
                var sbQuery = new StringBuilder();

                sbQuery.AppendLine(@"
                    SELECT
                        CATEGORY_ID,
                        CATEGORY_NAME
                    FROM
                        PRODUCT_CATEGORIES
                    WHERE
                        CATEGORY_ID = :CategoryId
                ");
                dynamicParam.Add("CategoryId", id, OracleDbType.Int64, ParameterDirection.Input);

                var res = _dbConnection.QueryFirstOrDefault<GetProductCategoryRP>(sbQuery.ToString(), dynamicParam);

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
                var sbSQL = new StringBuilder();
                sbSQL.AppendLine(@"
                    INSERT INTO PRODUCT_CATEGORIES
                        (CATEGORY_NAME)
                    VALUES
                        (:CategoryName)
                    RETURNING CATEGORY_ID INTO :CategoryId
                ");

                var dynamicParam = new OracleDynamicParameters();
                dynamicParam.Add("CategoryName", objProductCategory.CategoryName, OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("CategoryId", dbType: OracleDbType.Int64, direction: ParameterDirection.Output);

                _dbConnection.Execute(sbSQL.ToString(), dynamicParam);
                objProductCategory.CategoryId = (long)(dynamicParam.Get<OracleDecimal?>("CategoryId") ?? 0);

                return objProductCategory.CategoryId > 0;
            }
            catch
            {
                throw;
            }
        }
    }
}
