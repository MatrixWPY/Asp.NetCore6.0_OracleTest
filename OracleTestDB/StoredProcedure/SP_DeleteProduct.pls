create or replace PROCEDURE SP_DELETEPRODUCT(
    i_productIds VARCHAR2
)
AS 
BEGIN

    DELETE FROM
        PRODUCTS
    WHERE
        INSTR(','||i_productIds||',', ','||PRODUCT_ID||',') > 0;

END SP_DELETEPRODUCT;