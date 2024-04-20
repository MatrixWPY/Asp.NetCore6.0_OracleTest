CREATE OR REPLACE PACKAGE PKG_Product AS

    TYPE OBJ_Product IS RECORD (
        PRODUCT_ID NUMBER,
        PRODUCT_NAME VARCHAR2(255),
        DESCRIPTION VARCHAR2(2000),
        STANDARD_COST NUMBER(9,2),
        LIST_PRICE NUMBER(9,2),
        CATEGORY_ID NUMBER,
        CATEGORY_NAME VARCHAR2(255)
    );
    TYPE TB_Product IS TABLE OF OBJ_Product;

    PROCEDURE SP_QueryProduct(
        i_categoryId NUMBER DEFAULT NULL,
        i_categoryIds TB_UDT_LONG DEFAULT NULL,
        i_productName VARCHAR2 DEFAULT NULL,
        i_rowStart NUMBER DEFAULT 0,
        i_rowLength NUMBER DEFAULT 10,
        o_cnt OUT SYS_REFCURSOR,
        o_result OUT SYS_REFCURSOR
    );

END PKG_Product;

CREATE OR REPLACE PACKAGE BODY PKG_Product AS

    PROCEDURE SP_QueryProduct(
        i_categoryId NUMBER DEFAULT NULL,
        i_categoryIds TB_UDT_LONG DEFAULT NULL,
        i_productName VARCHAR2 DEFAULT NULL,
        i_rowStart NUMBER DEFAULT 0,
        i_rowLength NUMBER DEFAULT 10,
        o_cnt OUT SYS_REFCURSOR,
        o_result OUT SYS_REFCURSOR
    )
    AS
        v_TempTable TB_Product;
    BEGIN
        SELECT
            P.PRODUCT_ID,
            P.PRODUCT_NAME,
            P.DESCRIPTION,
            P.STANDARD_COST,
            P.LIST_PRICE,
            PC.CATEGORY_ID,
            PC.CATEGORY_NAME
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
            PC.CATEGORY_ID IN (SELECT * FROM TABLE(CAST(i_categoryIds AS TB_UDT_LONG)))
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
            TABLE(v_TempTable);

        OPEN o_result FOR
        SELECT
            R.*,
            COALESCE(SUM(I.QUANTITY), 0) AS QUANTITY
        FROM
            TABLE(v_TempTable) R
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
    END SP_QueryProduct;

END PKG_Product;