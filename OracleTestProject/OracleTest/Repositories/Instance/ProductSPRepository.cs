using Dapper;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using OracleTest.Models.Data;
using OracleTest.Models.Data.Udt;
using OracleTest.Models.Response;
using OracleTest.Repositories.Interface;
using OracleTest.Utility;
using System.Data;

namespace OracleTest.Repositories.Instance
{
    public class ProductSPRepository : IProductRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProductSPRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public GetProductRP GetProduct(long id)
        {
            try
            {
                var dynamicParam = new OracleDynamicParameters();
                dynamicParam.Add("i_productId", id, OracleDbType.Int64, ParameterDirection.Input);
                dynamicParam.Add("o_result", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

                var res = _dbConnection.QueryFirstOrDefault<GetProductRP>(
                    "SP_GETPRODUCT",
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

        public (int, IEnumerable<QueryProductRP>) QueryProductV1(Dictionary<string, object> dicParams)
        {
            try
            {
                var dynamicParam = new OracleDynamicParameters();
                dynamicParam.Add("i_categoryId", dicParams.GetValueOrDefault("CategoryId"), OracleDbType.Int64, ParameterDirection.Input);
                dynamicParam.Add("i_categoryIds", dicParams.GetValueOrDefault("CategoryIds") == null ?
                                                  null : string.Join(',', (IEnumerable<long>)dicParams.GetValueOrDefault("CategoryIds")),
                                  OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("i_productName", dicParams.GetValueOrDefault("ProductName"), OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("i_rowStart", dicParams.GetValueOrDefault("RowStart"), OracleDbType.Int32, ParameterDirection.Input);
                dynamicParam.Add("i_rowLength", dicParams.GetValueOrDefault("RowLength"), OracleDbType.Int32, ParameterDirection.Input);
                dynamicParam.Add("o_cnt", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                dynamicParam.Add("o_result", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

                var res = _dbConnection.QueryMultiple(
                    "SP_QUERYPRODUCTV1",
                    dynamicParam,
                    commandType: CommandType.StoredProcedure
                );

                return (res.Read<int>().FirstOrDefault(), res.Read<QueryProductV1RP>());
            }
            catch
            {
                throw;
            }
        }

        public (int, IEnumerable<QueryProductRP>) QueryProductV2(Dictionary<string, object> dicParams)
        {
            try
            {
                var dynamicParam = new OracleDynamicParameters();
                dynamicParam.Add("i_categoryId", dicParams.GetValueOrDefault("CategoryId"), OracleDbType.Int64, ParameterDirection.Input);
                dynamicParam.Add("i_categoryIds", dicParams.GetValueOrDefault("CategoryIds") == null ?
                                                  null : string.Join(',', (IEnumerable<long>)dicParams.GetValueOrDefault("CategoryIds")),
                                  OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("i_productName", dicParams.GetValueOrDefault("ProductName"), OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("i_rowStart", dicParams.GetValueOrDefault("RowStart"), OracleDbType.Int32, ParameterDirection.Input);
                dynamicParam.Add("i_rowLength", dicParams.GetValueOrDefault("RowLength"), OracleDbType.Int32, ParameterDirection.Input);
                dynamicParam.Add("o_cnt", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                dynamicParam.Add("o_result", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

                var res = _dbConnection.QueryMultiple(
                    "SP_QUERYPRODUCTV2",
                    dynamicParam,
                    commandType: CommandType.StoredProcedure
                );

                return (res.Read<int>().FirstOrDefault(), res.Read<QueryProductV2RP>());
            }
            catch
            {
                throw;
            }
        }

        public (int, IEnumerable<QueryProductRP>) QueryProductV3(Dictionary<string, object> dicParams)
        {
            try
            {
                var dynamicParam = new OracleDynamicParameters();
                dynamicParam.Add("i_categoryId", dicParams.GetValueOrDefault("CategoryId"), OracleDbType.Int64, ParameterDirection.Input);
                dynamicParam.Add("i_categoryIds", dicParams.GetValueOrDefault("CategoryIds") == null ?
                                                  null : string.Join(',', (IEnumerable<long>)dicParams.GetValueOrDefault("CategoryIds")),
                                  OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("i_productName", dicParams.GetValueOrDefault("ProductName"), OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("i_rowStart", dicParams.GetValueOrDefault("RowStart"), OracleDbType.Int32, ParameterDirection.Input);
                dynamicParam.Add("i_rowLength", dicParams.GetValueOrDefault("RowLength"), OracleDbType.Int32, ParameterDirection.Input);
                dynamicParam.Add("o_result", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

                var res = _dbConnection.Query<QueryProductV3RP>(
                    "SP_QUERYPRODUCTV3",
                    dynamicParam,
                    commandType: CommandType.StoredProcedure
                );

                return (res.FirstOrDefault()?.TotalCnt ?? 0, res);
            }
            catch
            {
                throw;
            }
        }

        public (int, IEnumerable<QueryProductRP>) QueryProductV4(Dictionary<string, object> dicParams)
        {
            try
            {
                var dynamicParam = new OracleDynamicParameters();
                dynamicParam.Add("i_categoryId", dicParams.GetValueOrDefault("CategoryId"), OracleDbType.Int64, ParameterDirection.Input);
                dynamicParam.Add("i_categoryIds", dicParams.GetValueOrDefault("CategoryIds") == null ?
                                                  null : string.Join(',', (IEnumerable<long>)dicParams.GetValueOrDefault("CategoryIds")),
                                  OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("i_productName", dicParams.GetValueOrDefault("ProductName"), OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("i_rowStart", dicParams.GetValueOrDefault("RowStart"), OracleDbType.Int32, ParameterDirection.Input);
                dynamicParam.Add("i_rowLength", dicParams.GetValueOrDefault("RowLength"), OracleDbType.Int32, ParameterDirection.Input);
                dynamicParam.Add("o_result", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

                var res = _dbConnection.Query<QueryProductV4RP>(
                    "SP_QUERYPRODUCTV4",
                    dynamicParam,
                    commandType: CommandType.StoredProcedure
                );

                return (res.FirstOrDefault()?.TotalCnt ?? 0, res);
            }
            catch
            {
                throw;
            }
        }

        public (int, IEnumerable<QueryProductRP>) QueryProductV5(Dictionary<string, object> dicParams)
        {
            try
            {
                var dynamicParam = new OracleDynamicParameters();
                dynamicParam.Add("i_categoryId", dicParams.GetValueOrDefault("CategoryId"), OracleDbType.Int64, ParameterDirection.Input);

                var aryUdtLong = ((IEnumerable<long>)dicParams.GetValueOrDefault("CategoryIds"))
                                 ?.Select(e => new UdtLong() { LongData = e }).ToArray();
                dynamicParam.Add(
                    "i_categoryIds",
                    aryUdtLong == null ? null : new TbUdtLong { UdtLongData = aryUdtLong },
                    OracleDbType.Object,
                    ParameterDirection.Input,
                    udtTypeName: "TB_UDT_LONG"
                );

                dynamicParam.Add("i_productName", dicParams.GetValueOrDefault("ProductName"), OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("i_rowStart", dicParams.GetValueOrDefault("RowStart"), OracleDbType.Int32, ParameterDirection.Input);
                dynamicParam.Add("i_rowLength", dicParams.GetValueOrDefault("RowLength"), OracleDbType.Int32, ParameterDirection.Input);
                dynamicParam.Add("o_result", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

                var res = _dbConnection.Query<QueryProductV5RP>(
                    "SP_QUERYPRODUCTV5",
                    dynamicParam,
                    commandType: CommandType.StoredProcedure
                );

                return (res.FirstOrDefault()?.TotalCnt ?? 0, res);
            }
            catch
            {
                throw;
            }
        }

        public (int, IEnumerable<QueryProductRP>) QueryProductV6(Dictionary<string, object> dicParams)
        {
            try
            {
                var dynamicParam = new OracleDynamicParameters();
                dynamicParam.Add("i_categoryId", dicParams.GetValueOrDefault("CategoryId"), OracleDbType.Int64, ParameterDirection.Input);

                var aryUdtLong = ((IEnumerable<long>)dicParams.GetValueOrDefault("CategoryIds"))
                                 ?.Select(e => new UdtLong() { LongData = e }).ToArray();
                dynamicParam.Add(
                    "i_categoryIds",
                    aryUdtLong == null ? null : new TbUdtLong { UdtLongData = aryUdtLong },
                    OracleDbType.Object,
                    ParameterDirection.Input,
                    udtTypeName: "TB_UDT_LONG"
                );

                dynamicParam.Add("i_productName", dicParams.GetValueOrDefault("ProductName"), OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("i_rowStart", dicParams.GetValueOrDefault("RowStart"), OracleDbType.Int32, ParameterDirection.Input);
                dynamicParam.Add("i_rowLength", dicParams.GetValueOrDefault("RowLength"), OracleDbType.Int32, ParameterDirection.Input);
                dynamicParam.Add("o_cnt", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                dynamicParam.Add("o_result", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

                var res = _dbConnection.QueryMultiple(
                    "PKG_Product.SP_QueryProduct",
                    dynamicParam,
                    commandType: CommandType.StoredProcedure
                );

                return (res.Read<int>().FirstOrDefault(), res.Read<QueryProductV6RP>());
            }
            catch
            {
                throw;
            }
        }

        public bool InsertProduct(Product objProduct)
        {
            try
            {
                var dynamicParam = new OracleDynamicParameters();
                dynamicParam.Add("i_productName", objProduct.ProductName, OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("i_description", objProduct.Description, OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("i_standardCost", objProduct.StandardCost, OracleDbType.Decimal, ParameterDirection.Input);
                dynamicParam.Add("i_listPrice", objProduct.ListPrice, OracleDbType.Decimal, ParameterDirection.Input);
                dynamicParam.Add("i_categoryId", objProduct.CategoryId, OracleDbType.Int64, ParameterDirection.Input);
                dynamicParam.Add("o_productId", dbType: OracleDbType.Int64, direction: ParameterDirection.Output);

                _dbConnection.Execute(
                    "SP_INSERTPRODUCT",
                    dynamicParam,
                    commandType: CommandType.StoredProcedure
                );
                objProduct.ProductId = (long)(dynamicParam.Get<OracleDecimal?>("o_productId") ?? 0);

                return objProduct.ProductId > 0;
            }
            catch
            {
                throw;
            }
        }

        public bool UpdateProduct(Product objProduct)
        {
            try
            {
                var dynamicParam = new OracleDynamicParameters();
                dynamicParam.Add("i_productId", objProduct.ProductId, OracleDbType.Int64, ParameterDirection.Input);
                dynamicParam.Add("i_productName", objProduct.ProductName, OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("i_description", objProduct.Description, OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("i_standardCost", objProduct.StandardCost, OracleDbType.Decimal, ParameterDirection.Input);
                dynamicParam.Add("i_listPrice", objProduct.ListPrice, OracleDbType.Decimal, ParameterDirection.Input);
                dynamicParam.Add("i_categoryId", objProduct.CategoryId, OracleDbType.Int64, ParameterDirection.Input);

                _dbConnection.Execute(
                    "SP_UPDATEPRODUCT",
                    dynamicParam,
                    commandType: CommandType.StoredProcedure
                );

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteProduct(IEnumerable<long> ids)
        {
            try
            {
                var dynamicParam = new OracleDynamicParameters();
                dynamicParam.Add("i_productIds", string.Join(',', ids), OracleDbType.Varchar2, ParameterDirection.Input);

                _dbConnection.Execute(
                    "SP_DELETEPRODUCT",
                    dynamicParam,
                    commandType: CommandType.StoredProcedure
                );

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool InsertProductAndCategory(Product objProduct, ProductCategory objProductCategory)
        {
            try
            {
                var dynamicParam = new OracleDynamicParameters();
                dynamicParam.Add("i_categoryName", objProductCategory.CategoryName, OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("i_productName", objProduct.ProductName, OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("i_description", objProduct.Description, OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("i_standardCost", objProduct.StandardCost, OracleDbType.Decimal, ParameterDirection.Input);
                dynamicParam.Add("i_listPrice", objProduct.ListPrice, OracleDbType.Decimal, ParameterDirection.Input);
                dynamicParam.Add("o_productId", dbType: OracleDbType.Int64, direction: ParameterDirection.Output);

                _dbConnection.Execute(
                    "SP_INSERTPRODUCTANDCATEGORY",
                    dynamicParam,
                    commandType: CommandType.StoredProcedure
                );
                objProduct.ProductId = (long)(dynamicParam.Get<OracleDecimal?>("o_productId") ?? 0);

                return objProduct.ProductId > 0;
            }
            catch
            {
                throw;
            }
        }
    }
}
