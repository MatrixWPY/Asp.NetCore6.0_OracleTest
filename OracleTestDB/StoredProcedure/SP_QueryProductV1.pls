CREATE OR REPLACE PROCEDURE SP_QUERYPRODUCTV1(
    i_categoryId NUMBER DEFAULT NULL,
    i_categoryIds VARCHAR2 DEFAULT NULL,
    i_productName VARCHAR2 DEFAULT NULL,
    i_rowStart NUMBER DEFAULT 0,
    i_rowLength NUMBER DEFAULT 10,
    o_cnt OUT SYS_REFCURSOR,
    o_result OUT SYS_REFCURSOR
)
AS
BEGIN

    OPEN o_cnt FOR
    SELECT
        COUNT(1)
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

    OPEN o_result FOR
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
    GROUP BY
        P.PRODUCT_ID,
        P.PRODUCT_NAME,
        P.DESCRIPTION,
        P.STANDARD_COST,
        P.LIST_PRICE,
        PC.CATEGORY_ID,
        PC.CATEGORY_NAME
    ORDER BY
        P.PRODUCT_ID DESC
    OFFSET i_rowStart ROWS FETCH NEXT i_rowLength ROWS ONLY;

END SP_QUERYPRODUCTV1;