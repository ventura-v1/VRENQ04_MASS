USE [XONTVenturaUserMASS_DEV]
GO
/****** Object:  StoredProcedure [RD].[usp_VRENQ04GetReportData]    Script Date: 11/14/2013 08:46:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [RD].[usp_VRENQ04GetReportData]	'MASS','NBP198751K',0,'',''
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [RD].[usp_VRENQ04GetReportData]	
	@BusinessUnit CHAR(4) = '',	
	--@User VARCHAR(40)  = '',
	--@PowerUser CHAR(1)  = '',
	--@FromDate datetime ,
	--@ToDate datetime,
	--@TerritoryCode  CHAR(4) = '',
	@ProductCode VARCHAR(24) = '',
	@ProductClassCount int = 0,
	@ProductClassification VARCHAR(100)  = '',
	@ProductClassificationCode VARCHAR(200)  = ''
AS
BEGIN

	CREATE TABLE #VRENQ04_PRODUCT
	(
		ProductCode VARCHAR(24) PRIMARY KEY,
		Description VARCHAR (40)
	)	
    DECLARE @ProductIndex INT
	DECLARE @ProductSubClassification VARCHAR(100)
	DECLARE @ProductSubClassificationCode VARCHAR(200)
    DECLARE @SQLProduct VARCHAR(MAX)    
    
    BEGIN 
		SET @ProductIndex = 1
		SET @ProductSubClassification = @ProductClassification 
		SET @ProductSubClassificationCode = @ProductClassificationCode 

			SET @SQLProduct= 'INSERT INTO #VRENQ04_PRODUCT SELECT DISTINCT Product.ProductCode,Product.Description
								FROM RD.Product AS Product WITH (NOLOCK)
								INNER JOIN RD.ProductClassification AS Classification WITH (NOLOCK)
								ON Classification.BusinessUnit=Product.BusinessUnit
								AND Classification.ProductCode=Product.ProductCode
								INNER JOIN XA.UserMasterValues AS UserMaster WITH (NOLOCK)
								ON Classification.BusinessUnit=UserMaster.BusinessUnit
								AND Classification.MasterGroup=UserMaster.MasterGroup
								AND Classification.MasterGroupValue=UserMaster.MasterGroupValue
								'
        WHILE (@ProductIndex <= @ProductClassCount)
        BEGIN
			IF (CHARINDEX(',',@ProductSubClassificationCode) > 0 )
            BEGIN
				SET   @SQLProduct = @SQLProduct + ' CROSS APPLY (SELECT ProductCode FROM RD.ProductClassification 
                                          WHERE RD.ProductClassification.BusinessUnit = ''' + @BusinessUnit + '''
                                          AND RD.ProductClassification.ProductCode = Product.ProductCode
                                          AND RD.ProductClassification.MasterGroup = ''' + substring(@ProductSubClassification,0,CHARINDEX(',',@ProductSubClassification))+'''
                                          AND RD.ProductClassification.MasterGroupValue = ''' + substring(@ProductSubClassificationCode,0,CHARINDEX(',',@ProductSubClassificationCode))+'''
                                          ) AS ProductClass' + CAST(@ProductIndex as VARCHAR) + ''
                 SET @ProductIndex = @ProductIndex + 1
                 SET @ProductSubClassificationCode = SUBSTRING(@ProductSubClassificationCode,CHARINDEX(',',@ProductSubClassificationCode)+1,LEN(@ProductSubClassificationCode)-CHARINDEX(',',@ProductSubClassificationCode))
                 SET @ProductSubClassification = SUBSTRING(@ProductSubClassification,CHARINDEX(',',@ProductSubClassification)+1,LEN(@ProductSubClassification)-CHARINDEX(',',@ProductSubClassification))
            END
            ELSE
            BEGIN
                 SET @SQLProduct = @SQLProduct + ' CROSS APPLY  (SELECT ProductCode FROM RD.ProductClassification 
                                                      WHERE RD.ProductClassification.BusinessUnit = ''' + @BusinessUnit + '''
                                                      AND RD.ProductClassification.ProductCode = Product.ProductCode
                                                      AND RD.ProductClassification.MasterGroup = ''' + @ProductSubClassification + '''
                                                      AND RD.ProductClassification.MasterGroupValue = ''' + @ProductSubClassificationCode + '''
                                                      ) AS ProductClass' + CAST(@ProductIndex as VARCHAR) + ' '
                 SET @ProductIndex = @ProductIndex + 1                    
            END 
		END     
        SET   @SQLProduct = @SQLProduct + ' WHERE Product.BusinessUnit = ''' + @BusinessUnit + ''''            
                  
        IF @ProductCode <> '' 
			SET   @SQLProduct = @SQLProduct + ' AND Product.ProductCode = ''' + @ProductCode + ''''    
        EXEC (@SQLProduct) 
	END   
   
    
    Select RD.PurchaseOrderLine.ProductCode as ItemCode,RD.Product.Description as Description,OrderQtyU1 as OrderQty,
DeliveryDate, OrderDate,RD.Supplier.SupplierName as Supplier ,PurchaseCategoryCode,OrderNumber
--PurchaseCategoryCode+'/'+ CAST(OrderNumber as Varchar ) as PONo

From RD.PurchaseOrderLine  

INNER JOIN #VRENQ04_PRODUCT AS P ON RD.PurchaseOrderLine.ProductCode=P.ProductCode

INNER JOIN  RD.Product ON  RD.Product.BusinessUnit = RD.PurchaseOrderLine.BusinessUnit 
AND RD.Product.ProductCode = RD.PurchaseOrderLine.ProductCode 

INNER JOIN  RD.Supplier ON  RD.Supplier.BusinessUnit = RD.PurchaseOrderLine.BusinessUnit 
AND RD.Supplier.SupplierCode = RD.PurchaseOrderLine.SupplierCode 

INNER JOIN  RD.TerritoryControl ON  RD.TerritoryControl.BusinessUnit = RD.PurchaseOrderLine.BusinessUnit 
AND RD.TerritoryControl.TerritoryCode = RD.PurchaseOrderLine.TerritoryCode
AND RD.TerritoryControl.HeadOfficeTerritory = '1'

WHERE RD.PurchaseOrderLine.BusinessUnit = @BusinessUnit 
--AND  RD.PurchaseOrderLine.ProductCode = CASE WHEN  @ProductCode <>'' THEN @ProductCode ELSE RD.PurchaseOrderLine.ProductCode END
AND RD.PurchaseOrderLine.OrderStatus = '3'

ORDER BY DeliveryDate,ItemCode

END

