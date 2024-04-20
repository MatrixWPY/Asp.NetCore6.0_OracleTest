CREATE OR REPLACE PROCEDURE SP_QUERYPRODUCTV2(
    i_categoryId NUMBER DEFAULT NULL,
    i_categoryIds VARCHAR2 DEFAULT NULL,
    i_productName VARCHAR2 DEFAULT NULL,
    i_rowStart NUMBER DEFAULT 0,
    i_rowLength NUMBER DEFAULT 10,
    o_cnt OUT SYS_REFCURSOR,
    o_result OUT SYS_REFCURSOR
)
AS
    v_TempTable TB_QUERYPRODUCTV2;
BEGIN

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
    AND
    (
        (i_categoryId IS NULL)
        OR
        (PC.CATEGORY_ID = i_categoryId)
    )
	AND
    (
        (i_categoryIds IS NULL)
        OR
        (INSTR(','||i_categoryIds||',', ','||PC.CATEGORY_ID||',') > 0)
    )
    AND
    (
        (i_productName IS NULL)
        OR
        (LOWER(P.PRODUCT_NAME) LIKE i_productName)
    );

    OPEN o_cnt FOR
    SELECT
        COUNT(1)
    FROM
        TABLE(CAST(v_TempTable AS TB_QUERYPRODUCTV2));

    OPEN o_result FOR
    SELECT
        R.*,
        COALESCE(SUM(I.QUANTITY), 0) AS QUANTITY
    FROM
        TABLE(CAST(v_TempTable AS TB_QUERYPRODUCTV2)) R
    LEFT JOIN
        INVENTORIES I
            ON R.PRODUCT_ID = I.PRODUCT_ID
    GROUP BY
        R.PRODUCT_ID,
        R.PRODUCT_NAME,
        R.DESCRIPTION,
        R.STANDARD_COST,
        R.LIST_PRICE,
        R.CATEGORY_ID,
        R.CATEGORY_NAME
    ORDER BY R.PRODUCT_ID DESC
    OFFSET i_rowStart ROWS FETCH NEXT i_rowLength ROWS ONLY;

END SP_QUERYPRODUCTV2;