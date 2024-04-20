CREATE OR REPLACE PROCEDURE SP_QUERYPRODUCTV4(
    i_categoryId NUMBER DEFAULT NULL,
    i_categoryIds VARCHAR2 DEFAULT NULL,
    i_productName VARCHAR2 DEFAULT NULL,
    i_rowStart NUMBER DEFAULT 0,
    i_rowLength NUMBER DEFAULT 10,
    o_result OUT SYS_REFCURSOR
)
AS 
BEGIN

    OPEN o_result FOR
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
        )
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
    GROUP BY
        C.PRODUCT_ID,
        C.PRODUCT_NAME,
        C.DESCRIPTION,
        C.STANDARD_COST,
        C.LIST_PRICE,
        C.CATEGORY_ID,
        C.CATEGORY_NAME
    ORDER BY C.PRODUCT_ID DESC
    OFFSET i_rowStart ROWS FETCH NEXT i_rowLength ROWS ONLY;

END SP_QUERYPRODUCTV4;