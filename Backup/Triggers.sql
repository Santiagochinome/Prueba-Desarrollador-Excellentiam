CREATE OR ALTER TRIGGER TR_Validar_Total_Factura
ON DetallesFactura
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        DECLARE @FacturasAfectadas TABLE (IdBill INT);
        
        INSERT INTO @FacturasAfectadas (IdBill)
        SELECT DISTINCT IdBill FROM inserted;
        
        UPDATE f
        SET Total = (SELECT COALESCE(SUM(Amount * UnitPrice), 0) 
                    FROM DetallesFactura WHERE IdBill = f.Id)
        FROM Facturas f
        INNER JOIN @FacturasAfectadas fa ON f.Id = fa.IdBill;
    END TRY
    BEGIN CATCH
        PRINT 'Error en trigger de validación (ignorado): ' + ERROR_MESSAGE();
    END CATCH
END
GO