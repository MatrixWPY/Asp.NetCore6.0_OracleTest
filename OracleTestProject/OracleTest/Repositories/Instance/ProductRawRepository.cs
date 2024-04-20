using Dapper;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using OracleTest.Models.Data;
using OracleTest.Models.Data.Udt;
using OracleTest.Models.Response;
using OracleTest.Repositories.Interface;
using OracleTest.Utility;
using System.Data;
using System.Text;

namespace OracleTest.Repositories.Instance
{
    public class ProductRawRepository : IProductRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProductRawRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public GetProductRP GetProduct(long id)
        {
            try
            {
                var dynamicParam = new OracleDynamicParameters();
                var sbQuery = new StringBuilder();

                sbQuery.AppendLine(@"
                    SELECT
                        P.PRODUCT_ID,
                        P.PRODUCT_NAME,
                        P.DESCRIPTION,
                        P.STANDARD_COST,
                        P.LIST_PRICE,
                        PC.CATEGORY_ID,
                        PC.CATEGORY_NAME,
                        COALESCE(SUM(I.QUANTITY), 0) AS QUANTITY
                    FROM
                        PRODUCTS P
                    INNER JOIN
                        PRODUCT_CATEGORIES PC
                            ON P.CATEGORY_ID = PC.CATEGORY_ID
                    LEFT JOIN
                        INVENTORIES I
                            ON P.PRODUCT_ID = I.PRODUCT_ID
                    WHERE
                        P.PRODUCT_ID = :ProductId
                    GROUP BY
                        P.PRODUCT_ID,
                        P.PRODUCT_NAME,
                        P.DESCRIPTION,
                        P.STANDARD_COST,
                        P.LIST_PRICE,
                        PC.CATEGORY_ID,
                        PC.CATEGORY_NAME
                ");
                dynamicParam.Add("ProductId", id, OracleDbType.Int64, ParameterDirection.Input);

                var res = _dbConnection.QueryFirstOrDefault<GetProductRP>(sbQuery.ToString(), dynamicParam);

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
                var sbCnt = new StringBuilder();
                var sbResult = new StringBuilder();
                var sbQuery = new StringBuilder();

                sbCnt.AppendLine(@"
                    OPEN :o_Cnt FOR
                    SELECT
                        COUNT(1)
                    FROM
                        PRODUCTS P
                    INNER JOIN
                        PRODUCT_CATEGORIES PC
                            ON P.CATEGORY_ID = PC.CATEGORY_ID
                    WHERE 1 = 1
                ");
                dynamicParam.Add("o_Cnt", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

                sbResult.AppendLine(@"
                    OPEN :o_Result FOR
                    SELECT
                        P.PRODUCT_ID,
                        P.PRODUCT_NAME,
                        P.DESCRIPTION,
                        P.STANDARD_COST,
                        P.LIST_PRICE,
                        PC.CATEGORY_ID,
                        PC.CATEGORY_NAME,
                        COALESCE(SUM(I.QUANTITY), 0) AS QUANTITY
                    FROM
                        PRODUCTS P
                    INNER JOIN
                        PRODUCT_CATEGORIES PC
                            ON P.CATEGORY_ID = PC.CATEGORY_ID
                    LEFT JOIN
                        INVENTORIES I
                            ON P.PRODUCT_ID = I.PRODUCT_ID
                    WHERE 1 = 1
                ");
                dynamicParam.Add("o_Result", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

                #region [Query Condition]
                foreach (var key in dicParams.Keys)
                {
                    switch (key)
                    {
                        case "CategoryId":
                            sbQuery.AppendLine("AND PC.CATEGORY_ID = :CategoryId");
                            dynamicParam.Add("CategoryId", dicParams["CategoryId"], OracleDbType.Int64, ParameterDirection.Input);
                            break;

                        case "CategoryIds":
                            sbQuery.AppendLine("AND PC.CATEGORY_ID IN :CategoryIds");
                            dynamicParam.AddDynamicParams(new { CategoryIds = (IEnumerable<long>)dicParams["CategoryIds"] });
                            break;

                        case "ProductName":
                            sbQuery.AppendLine("AND LOWER(P.PRODUCT_NAME) LIKE :ProductName");
                            dynamicParam.Add("ProductName", dicParams["ProductName"], OracleDbType.Varchar2, ParameterDirection.Input);
                            break;
                    }
                }
                #endregion

                sbCnt.Append(sbQuery.ToString());
                sbResult.Append(sbQuery.ToString());

                #region [GroupBy]
                sbResult.AppendLine(@"
                    GROUP BY
                        P.PRODUCT_ID,
                        P.PRODUCT_NAME,
                        P.DESCRIPTION,
                        P.STANDARD_COST,
                        P.LIST_PRICE,
                        PC.CATEGORY_ID,
                        PC.CATEGORY_NAME
                ");
                #endregion

                #region [Order]
                sbResult.AppendLine("ORDER BY P.PRODUCT_ID DESC");
                #endregion

                #region [Paging]
                sbResult.AppendLine("OFFSET :RowStart ROWS FETCH NEXT :RowLength ROWS ONLY");
                dynamicParam.Add("RowStart", dicParams["RowStart"], OracleDbType.Int32, ParameterDirection.Input);
                dynamicParam.Add("RowLength", dicParams["RowLength"], OracleDbType.Int32, ParameterDirection.Input);
                #endregion

                var res = _dbConnection.QueryMultiple($"BEGIN {sbCnt}; {sbResult}; END;", dynamicParam);

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
                var sbDeclare = new StringBuilder();
                var sbTemp = new StringBuilder();
                var sbQuery = new StringBuilder();
                var sbCnt = new StringBuilder();
                var sbResult = new StringBuilder();

                sbDeclare.AppendLine(@"
                    DECLARE v_TempTable TB_QUERYPRODUCTV2
                ");

                sbTemp.AppendLine(@"
                    SELECT OBJ_QUERYPRODUCTV2
                    (
                        P.PRODUCT_ID,
                        P.PRODUCT_NAME,
                        P.DESCRIPTION,
                        P.STANDARD_COST,
                        P.LIST_PRICE,
                        PC.CATEGORY_ID,
                        PC.CATEGORY_NAME
                    )
                    BULK COLLECT INTO
                        v_TempTable
                    FROM
                        PRODUCTS P
                    INNER JOIN
                        PRODUCT_CATEGORIES PC
                            ON P.CATEGORY_ID = PC.CATEGORY_ID
                    WHERE 1 = 1
                ");

                #region [Query Condition]
                foreach (var key in dicParams.Keys)
                {
                    switch (key)
                    {
                        case "CategoryId":
                            sbQuery.AppendLine("AND PC.CATEGORY_ID = :CategoryId");
                            dynamicParam.Add("CategoryId", dicParams["CategoryId"], OracleDbType.Int64, ParameterDirection.Input);
                            break;

                        case "CategoryIds":
                            sbQuery.AppendLine("AND PC.CATEGORY_ID IN :CategoryIds");
                            dynamicParam.AddDynamicParams(new { CategoryIds = (IEnumerable<long>)dicParams["CategoryIds"] });
                            break;

                        case "ProductName":
                            sbQuery.AppendLine("AND LOWER(P.PRODUCT_NAME) LIKE :ProductName");
                            dynamicParam.Add("ProductName", dicParams["ProductName"], OracleDbType.Varchar2, ParameterDirection.Input);
                            break;
                    }
                }
                #endregion

                sbTemp.Append(sbQuery.ToString());

                sbCnt.AppendLine(@"
                    OPEN :o_Cnt FOR
                    SELECT
                        COUNT(1)
                    FROM
                        TABLE(CAST(v_TempTable AS TB_QUERYPRODUCTV2))
                ");
                dynamicParam.Add("o_Cnt", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

                sbResult.AppendLine(@"
                    OPEN :o_Result FOR
                    SELECT
                        R.*,
                        COALESCE(SUM(I.QUANTITY), 0) AS QUANTITY
                    FROM
                        TABLE(CAST(v_TempTable AS TB_QUERYPRODUCTV2)) R
                    LEFT JOIN
                        INVENTORIES I
                            ON R.PRODUCT_ID = I.PRODUCT_ID
                ");
                dynamicParam.Add("o_Result", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);

                #region [GroupBy]
                sbResult.AppendLine(@"
                    GROUP BY
                        R.PRODUCT_ID,
                        R.PRODUCT_NAME,
                        R.DESCRIPTION,
                        R.STANDARD_COST,
                        R.LIST_PRICE,
                        R.CATEGORY_ID,
                        R.CATEGORY_NAME
                ");
                #endregion

                #region [Order]
                sbResult.AppendLine("ORDER BY R.PRODUCT_ID DESC");
                #endregion

                #region [Paging]
                sbResult.AppendLine("OFFSET :RowStart ROWS FETCH NEXT :RowLength ROWS ONLY");
                dynamicParam.Add("RowStart", dicParams["RowStart"], OracleDbType.Int32, ParameterDirection.Input);
                dynamicParam.Add("RowLength", dicParams["RowLength"], OracleDbType.Int32, ParameterDirection.Input);
                #endregion

                var res = _dbConnection.QueryMultiple($"{sbDeclare}; BEGIN {sbTemp}; {sbCnt}; {sbResult}; END;", dynamicParam);

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
                var sbSQL = new StringBuilder();
                sbSQL.AppendLine(@"
                    SELECT
                        P.PRODUCT_ID,
                        P.PRODUCT_NAME,
                        P.DESCRIPTION,
                        P.STANDARD_COST,
                        P.LIST_PRICE,
                        PC.CATEGORY_ID,
                        PC.CATEGORY_NAME,
                        COALESCE(SUM(I.QUANTITY), 0) AS QUANTITY,
                        COUNT(*) OVER() AS TotalCnt
                    FROM
                        PRODUCTS P
                    INNER JOIN
                        PRODUCT_CATEGORIES PC
                            ON P.CATEGORY_ID = PC.CATEGORY_ID
                    LEFT JOIN
                        INVENTORIES I
                            ON P.PRODUCT_ID = I.PRODUCT_ID
                    WHERE 1 = 1
                ");

                #region [Query Condition]
                foreach (var key in dicParams.Keys)
                {
                    switch (key)
                    {
                        case "CategoryId":
                            sbSQL.AppendLine("AND PC.CATEGORY_ID = :CategoryId");
                            dynamicParam.Add("CategoryId", dicParams["CategoryId"], OracleDbType.Int64, ParameterDirection.Input);
                            break;

                        case "CategoryIds":
                            sbSQL.AppendLine("AND PC.CATEGORY_ID IN :CategoryIds");
                            dynamicParam.AddDynamicParams(new { CategoryIds = (IEnumerable<long>)dicParams["CategoryIds"] });
                            break;

                        case "ProductName":
                            sbSQL.AppendLine("AND LOWER(P.PRODUCT_NAME) LIKE :ProductName");
                            dynamicParam.Add("ProductName", dicParams["ProductName"], OracleDbType.Varchar2, ParameterDirection.Input);
                            break;
                    }
                }
                #endregion

                #region [GroupBy]
                sbSQL.AppendLine(@"
                    GROUP BY
                        P.PRODUCT_ID,
                        P.PRODUCT_NAME,
                        P.DESCRIPTION,
                        P.STANDARD_COST,
                        P.LIST_PRICE,
                        PC.CATEGORY_ID,
                        PC.CATEGORY_NAME
                ");
                #endregion

                #region [Order]
                sbSQL.AppendLine("ORDER BY P.PRODUCT_ID DESC");
                #endregion

                #region [Paging]
                sbSQL.AppendLine("OFFSET :RowStart ROWS FETCH NEXT :RowLength ROWS ONLY");
                dynamicParam.Add("RowStart", dicParams["RowStart"], OracleDbType.Int32, ParameterDirection.Input);
                dynamicParam.Add("RowLength", dicParams["RowLength"], OracleDbType.Int32, ParameterDirection.Input);
                #endregion

                var res = _dbConnection.Query<QueryProductV3RP>(sbSQL.ToString(), dynamicParam);

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
                var sbSQL = new StringBuilder();
                sbSQL.AppendLine(@"
                    WITH CTE AS
                    (
                        SELECT
                            P.PRODUCT_ID,
                            P.PRODUCT_NAME,
                            P.DESCRIPTION,
                            P.STANDARD_COST,
                            P.LIST_PRICE,
                            PC.CATEGORY_ID,
                            PC.CATEGORY_NAME
                        FROM
                            PRODUCTS P
                        INNER JOIN
                            PRODUCT_CATEGORIES PC
                                ON P.CATEGORY_ID = PC.CATEGORY_ID
                        WHERE 1 = 1
                ");

                #region [Query Condition]
                foreach (var key in dicParams.Keys)
                {
                    switch (key)
                    {
                        case "CategoryId":
                            sbSQL.AppendLine("AND PC.CATEGORY_ID = :CategoryId");
                            dynamicParam.Add("CategoryId", dicParams["CategoryId"], OracleDbType.Int64, ParameterDirection.Input);
                            break;

                        case "CategoryIds":
                            sbSQL.AppendLine("AND PC.CATEGORY_ID IN :CategoryIds");
                            dynamicParam.AddDynamicParams(new { CategoryIds = (IEnumerable<long>)dicParams["CategoryIds"] });
                            break;

                        case "ProductName":
                            sbSQL.AppendLine("AND LOWER(P.PRODUCT_NAME) LIKE :ProductName");
                            dynamicParam.Add("ProductName", dicParams["ProductName"], OracleDbType.Varchar2, ParameterDirection.Input);
                            break;
                    }
                }
                #endregion

                sbSQL.AppendLine(@"
                    )
                    SELECT
                        C.*,
                        COALESCE(SUM(I.QUANTITY), 0) AS QUANTITY,
                        COUNT(*) OVER() AS TotalCnt
                    FROM
                        CTE C
                    LEFT JOIN
                        INVENTORIES I
                            ON C.PRODUCT_ID = I.PRODUCT_ID
                ");

                #region [GroupBy]
                sbSQL.AppendLine(@"
                    GROUP BY
                        C.PRODUCT_ID,
                        C.PRODUCT_NAME,
                        C.DESCRIPTION,
                        C.STANDARD_COST,
                        C.LIST_PRICE,
                        C.CATEGORY_ID,
                        C.CATEGORY_NAME
                ");
                #endregion

                #region [Order]
                sbSQL.AppendLine("ORDER BY C.PRODUCT_ID DESC");
                #endregion

                #region [Paging]
                sbSQL.AppendLine("OFFSET :RowStart ROWS FETCH NEXT :RowLength ROWS ONLY");
                dynamicParam.Add("RowStart", dicParams["RowStart"], OracleDbType.Int32, ParameterDirection.Input);
                dynamicParam.Add("RowLength", dicParams["RowLength"], OracleDbType.Int32, ParameterDirection.Input);
                #endregion

                var res = _dbConnection.Query<QueryProductV4RP>(sbSQL.ToString(), dynamicParam);

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
                var sbSQL = new StringBuilder();
                sbSQL.AppendLine(@"
                    WITH CTE AS
                    (
                        SELECT
                            P.PRODUCT_ID,
                            P.PRODUCT_NAME,
                            P.DESCRIPTION,
                            P.STANDARD_COST,
                            P.LIST_PRICE,
                            PC.CATEGORY_ID,
                            PC.CATEGORY_NAME
                        FROM
                            PRODUCTS P
                        INNER JOIN
                            PRODUCT_CATEGORIES PC
                                ON P.CATEGORY_ID = PC.CATEGORY_ID
                        WHERE 1 = 1
                ");

                #region [Query Condition]
                foreach (var key in dicParams.Keys)
                {
                    switch (key)
                    {
                        case "CategoryId":
                            sbSQL.AppendLine("AND PC.CATEGORY_ID = :CategoryId");
                            dynamicParam.Add("CategoryId", dicParams["CategoryId"], OracleDbType.Int64, ParameterDirection.Input);
                            break;

                        case "CategoryIds":
                            sbSQL.AppendLine("AND PC.CATEGORY_ID IN (SELECT * FROM TABLE(CAST(:CategoryIds AS TB_UDT_LONG)))");
                            var aryUdtLong = ((IEnumerable<long>)dicParams["CategoryIds"])
                                             .Select(e => new UdtLong() { LongData = e }).ToArray();
                            dynamicParam.Add(
                                "CategoryIds",
                                new TbUdtLong { UdtLongData = aryUdtLong },
                                OracleDbType.Object,
                                ParameterDirection.Input,
                                udtTypeName: "TB_UDT_LONG"
                            );
                            break;

                        case "ProductName":
                            sbSQL.AppendLine("AND LOWER(P.PRODUCT_NAME) LIKE :ProductName");
                            dynamicParam.Add("ProductName", dicParams["ProductName"], OracleDbType.Varchar2, ParameterDirection.Input);
                            break;
                    }
                }
                #endregion

                sbSQL.AppendLine(@"
                    )
                    SELECT
                        C.*,
                        COALESCE(SUM(I.QUANTITY), 0) AS QUANTITY,
                        COUNT(*) OVER() AS TotalCnt
                    FROM
                        CTE C
                    LEFT JOIN
                        INVENTORIES I
                            ON C.PRODUCT_ID = I.PRODUCT_ID
                ");

                #region [GroupBy]
                sbSQL.AppendLine(@"
                    GROUP BY
                        C.PRODUCT_ID,
                        C.PRODUCT_NAME,
                        C.DESCRIPTION,
                        C.STANDARD_COST,
                        C.LIST_PRICE,
                        C.CATEGORY_ID,
                        C.CATEGORY_NAME
                ");
                #endregion

                #region [Order]
                sbSQL.AppendLine("ORDER BY C.PRODUCT_ID DESC");
                #endregion

                #region [Paging]
                sbSQL.AppendLine("OFFSET :RowStart ROWS FETCH NEXT :RowLength ROWS ONLY");
                dynamicParam.Add("RowStart", dicParams["RowStart"], OracleDbType.Int32, ParameterDirection.Input);
                dynamicParam.Add("RowLength", dicParams["RowLength"], OracleDbType.Int32, ParameterDirection.Input);
                #endregion

                var res = _dbConnection.Query<QueryProductV5RP>(sbSQL.ToString(), dynamicParam);

                return (res.FirstOrDefault()?.TotalCnt ?? 0, res);
            }
            catch
            {
                throw;
            }
        }

        public (int, IEnumerable<QueryProductRP>) QueryProductV6(Dictionary<string, object> dicParams)
        {
            throw new Exception(@"Please change setting from appsettings.json { ""DbType"": ""SP"" }");
        }

        public bool InsertProduct(Product objProduct)
        {
            try
            {
                var sbSQL = new StringBuilder();
                sbSQL.AppendLine(@"
                    INSERT INTO PRODUCTS
                        (PRODUCT_NAME, DESCRIPTION, STANDARD_COST, LIST_PRICE, CATEGORY_ID)
                    VALUES
                        (:ProductName, :Description, :StandardCost, :ListPrice, :CategoryId)
                    RETURNING PRODUCT_ID INTO :ProductId
                ");

                var dynamicParam = new OracleDynamicParameters();
                dynamicParam.Add("ProductName", objProduct.ProductName, OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("Description", objProduct.Description, OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("StandardCost", objProduct.StandardCost, OracleDbType.Decimal, ParameterDirection.Input);
                dynamicParam.Add("ListPrice", objProduct.ListPrice, OracleDbType.Decimal, ParameterDirection.Input);
                dynamicParam.Add("CategoryId", objProduct.CategoryId, OracleDbType.Int64, ParameterDirection.Input);
                dynamicParam.Add("ProductId", dbType: OracleDbType.Int64, direction: ParameterDirection.Output);

                _dbConnection.Execute(sbSQL.ToString(), dynamicParam);
                objProduct.ProductId = (long)(dynamicParam.Get<OracleDecimal?>("ProductId") ?? 0);

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
                var sbSQL = new StringBuilder();
                sbSQL.AppendLine(@"
                    UPDATE PRODUCTS SET
                        PRODUCT_NAME = :ProductName,
                        DESCRIPTION = :Description,
                        STANDARD_COST = :StandardCost,
                        LIST_PRICE = :ListPrice,
                        CATEGORY_ID = :CategoryId
                    WHERE
                        PRODUCT_ID = :ProductId
                ");

                var dynamicParam = new OracleDynamicParameters();
                dynamicParam.Add("ProductId", objProduct.ProductId, OracleDbType.Int64, ParameterDirection.Input);
                dynamicParam.Add("ProductName", objProduct.ProductName, OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("Description", objProduct.Description, OracleDbType.Varchar2, ParameterDirection.Input);
                dynamicParam.Add("StandardCost", objProduct.StandardCost, OracleDbType.Decimal, ParameterDirection.Input);
                dynamicParam.Add("ListPrice", objProduct.ListPrice, OracleDbType.Decimal, ParameterDirection.Input);
                dynamicParam.Add("CategoryId", objProduct.CategoryId, OracleDbType.Int64, ParameterDirection.Input);

                return _dbConnection.Execute(sbSQL.ToString(), dynamicParam) > 0;
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
                var sbSQL = new StringBuilder();
                sbSQL.AppendLine(@"
                    DELETE FROM
                        PRODUCTS
                    WHERE
                        PRODUCT_ID IN :ProductIds
                ");

                var dynamicParam = new OracleDynamicParameters();
                dynamicParam.AddDynamicParams(new { ProductIds = ids });

                return _dbConnection.Execute(sbSQL.ToString(), dynamicParam) > 0;
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
                var sbCategory = new StringBuilder();
                sbCategory.AppendLine(@"
                    INSERT INTO PRODUCT_CATEGORIES
                        (CATEGORY_NAME)
                    VALUES
                        (:CategoryName)
                    RETURNING CATEGORY_ID INTO :CategoryId
                ");

                var dpCategory = new OracleDynamicParameters();
                dpCategory.Add("CategoryName", objProductCategory.CategoryName, OracleDbType.Varchar2, ParameterDirection.Input);
                dpCategory.Add("CategoryId", dbType: OracleDbType.Int64, direction: ParameterDirection.Output);

                var sbProduct = new StringBuilder();
                sbProduct.AppendLine(@"
                    INSERT INTO PRODUCTS
                        (PRODUCT_NAME, DESCRIPTION, STANDARD_COST, LIST_PRICE, CATEGORY_ID)
                    VALUES
                        (:ProductName, :Description, :StandardCost, :ListPrice, :CategoryId)
                    RETURNING PRODUCT_ID INTO :ProductId
                ");

                var dpProduct = new OracleDynamicParameters();
                dpProduct.Add("ProductName", objProduct.ProductName, OracleDbType.Varchar2, ParameterDirection.Input);
                dpProduct.Add("Description", objProduct.Description, OracleDbType.Varchar2, ParameterDirection.Input);
                dpProduct.Add("StandardCost", objProduct.StandardCost, OracleDbType.Decimal, ParameterDirection.Input);
                dpProduct.Add("ListPrice", objProduct.ListPrice, OracleDbType.Decimal, ParameterDirection.Input);
                dpProduct.Add("ProductId", dbType: OracleDbType.Int64, direction: ParameterDirection.Output);

                _dbConnection.Open();
                using (var tran = _dbConnection.BeginTransaction())
                {
                    try
                    {
                        _dbConnection.Execute(sbCategory.ToString(), dpCategory, tran);
                        objProductCategory.CategoryId = (long)dpCategory.Get<OracleDecimal>("CategoryId");

                        objProduct.CategoryId = objProductCategory.CategoryId;
                        dpProduct.Add("CategoryId", objProduct.CategoryId, OracleDbType.Int64, ParameterDirection.Input);

                        _dbConnection.Execute(sbProduct.ToString(), dpProduct, tran);
                        objProduct.ProductId = (long)dpProduct.Get<OracleDecimal>("ProductId");

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }

                return objProduct.ProductId > 0;
            }
            catch
            {
                throw;
            }
            finally
            {
                _dbConnection.Close();
            }
        }
    }
}
