create or replace PROCEDURE SP_GETPRODUCT(
    i_productId NUMBER,
    o_result OUT SYS_REFCURSOR
)
AS 
BEGIN

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
    WHERE
        P.PRODUCT_ID = i_productId
    GROUP BY
        P.PRODUCT_ID,
        P.PRODUCT_NAME,
        P.DESCRIPTION,
        P.STANDARD_COST,
        P.LIST_PRICE,
        PC.CATEGORY_ID,
        PC.CATEGORY_NAME;

END SP_GETPRODUCT;