CREATE OR REPLACE PROCEDURE SP_INSERTPRODUCTANDCATEGORY(
    i_categoryName Varchar2,
    i_productName VARCHAR2,
    i_description VARCHAR2 DEFAULT NULL,
    i_standardCost NUMBER DEFAULT NULL,
    i_listPrice NUMBER DEFAULT NULL,
    o_productId OUT NUMBER
)
AS
    v_categoryId NUMBER;
BEGIN

    BEGIN
        INSERT INTO PRODUCT_CATEGORIES
            (CATEGORY_NAME)
        VALUES
            (i_categoryName)
        RETURNING CATEGORY_ID INTO v_categoryId;
        
        INSERT INTO PRODUCTS
            (PRODUCT_NAME, DESCRIPTION, STANDARD_COST, LIST_PRICE, CATEGORY_ID)
        VALUES
            (i_productName, i_description, i_standardCost, i_listPrice, v_categoryId)
        RETURNING PRODUCT_ID INTO o_productId;
        
        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
    END;

END SP_INSERTPRODUCTANDCATEGORY;