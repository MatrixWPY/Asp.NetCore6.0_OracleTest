CREATE OR REPLACE PROCEDURE SP_INSERTPRODUCT(
    i_productName VARCHAR2,
    i_description VARCHAR2 DEFAULT NULL,
    i_standardCost NUMBER DEFAULT NULL,
    i_listPrice NUMBER DEFAULT NULL,
    i_categoryId NUMBER,
    o_productId OUT NUMBER
)
AS 
BEGIN

    INSERT INTO PRODUCTS
        (PRODUCT_NAME, DESCRIPTION, STANDARD_COST, LIST_PRICE, CATEGORY_ID)
    VALUES
        (i_productName, i_description, i_standardCost, i_listPrice, i_categoryId)
    RETURNING PRODUCT_ID INTO o_productId;

END SP_INSERTPRODUCT;