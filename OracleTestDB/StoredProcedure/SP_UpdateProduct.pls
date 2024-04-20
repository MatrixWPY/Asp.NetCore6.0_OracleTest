CREATE OR REPLACE PROCEDURE SP_UPDATEPRODUCT(
    i_productId NUMBER,
    i_productName VARCHAR2,
    i_description VARCHAR2 DEFAULT NULL,
    i_standardCost NUMBER DEFAULT NULL,
    i_listPrice NUMBER DEFAULT NULL,
    i_categoryId NUMBER
)
AS 
BEGIN

    UPDATE PRODUCTS SET
        PRODUCT_NAME = i_productName,
        DESCRIPTION = i_description,
        STANDARD_COST = i_standardCost,
        LIST_PRICE = i_listPrice,
        CATEGORY_ID = i_categoryId
    WHERE
        PRODUCT_ID = i_productId;

END SP_UPDATEPRODUCT;