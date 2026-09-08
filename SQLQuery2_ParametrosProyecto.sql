USE HelpDeskTI;
GO



CREATE TRIGGER trg_AuditoriaTickets
ON Tickets
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Operacion NVARCHAR(50);
    
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        SET @Operacion = 'ACTUALIZACIÓN';
        
        INSERT INTO BitacoraTickets (TicketID, UsuarioOperacion, Operacion, Fecha, CambiosEfectuados)
        SELECT 
            i.TicketID,
            SYSTEM_USER, 
            @Operacion,
            GETDATE(),
            CONCAT('Estado: ', d.Estado, ' -> ', i.Estado, ' | Técnico asignado ID: ', ISNULL(CAST(i.TecnicoAsignadoID AS NVARCHAR), 'Sin asignar'))
        FROM inserted i
        INNER JOIN deleted d ON i.TicketID = d.TicketID;
    END
    ELSE IF EXISTS (SELECT * FROM inserted)
    BEGIN
        SET @Operacion = 'CREACIÓN';
        
        INSERT INTO BitacoraTickets (TicketID, UsuarioOperacion, Operacion, Fecha, CambiosEfectuados)
        SELECT 
            i.TicketID,
            SYSTEM_USER,
            @Operacion,
            GETDATE(),
            CONCAT('Ticket nuevo registrado con integridad Hash SHA256: ', i.HashSHA256)
        FROM inserted i;
    END
END;
GO



CREATE PROCEDURE sp_RegistrarTicket
    @SolicitanteID INT,
    @Departamento NVARCHAR(50),
    @Categoria NVARCHAR(50),
    @Prioridad NVARCHAR(20),
    @Titulo NVARCHAR(150),
    @Descripcion NVARCHAR(MAX),
    @HashSHA256 NVARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Tickets (SolicitanteID, Departamento, Categoria, Prioridad, Titulo, Descripcion, HashSHA256)
    VALUES (@SolicitanteID, @Departamento, @Categoria, @Prioridad, @Titulo, @Descripcion, @HashSHA256);
    
    SELECT SCOPE_IDENTITY() AS NuevoTicketID;
END;
GO


CREATE PROCEDURE sp_ActualizarEstadoTicket
    @TicketID INT,
    @TecnicoAsignadoID INT,
    @NuevoEstado NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Tickets
    SET Estado = @NuevoEstado,
        TecnicoAsignadoID = @TecnicoAsignadoID
    WHERE TicketID = @TicketID;
END;
GO



CREATE PROCEDURE sp_ConsultarTickets
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        t.TicketID,
        t.FechaCreacion,
        u.NombreCompleto AS Solicitante,
        t.Departamento,
        t.Prioridad,
        t.Estado,
        t.HashSHA256,
        ISNULL(tec.NombreCompleto, 'Sin asignar') AS TecnicoAsignado
    FROM Tickets t
    INNER JOIN Usuarios u ON t.SolicitanteID = u.UsuarioID
    LEFT JOIN Usuarios tec ON t.TecnicoAsignadoID = tec.UsuarioID
    ORDER BY t.FechaCreacion DESC;
END;
GO

