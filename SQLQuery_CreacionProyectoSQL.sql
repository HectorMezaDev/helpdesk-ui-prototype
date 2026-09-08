CREATE DATABASE HelpDeskTI;
GO
USE HelpDeskTI;
GO

CREATE TABLE Usuarios (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
    NombreCompleto NVARCHAR(100) NOT NULL,
    Rol NVARCHAR(50) NOT NULL,
    Departamento NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE Tickets (
    TicketID INT IDENTITY(1001,1) PRIMARY KEY,
    SolicitanteID INT NOT NULL FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    TecnicoAsignadoID INT NULL FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    Departamento NVARCHAR(50) NOT NULL,
    Categoria NVARCHAR(50) NOT NULL,
    Prioridad NVARCHAR(20) NOT NULL,
    Titulo NVARCHAR(150) NOT NULL,
    Descripcion NVARCHAR(MAX) NOT NULL,
    RutaEvidencia NVARCHAR(255) NULL, 
    Estado NVARCHAR(50) DEFAULT 'Abierto',
    FechaCreacion DATETIME DEFAULT GETDATE(),
    HashSHA256 NVARCHAR(64) NOT NULL 
);
GO

CREATE TABLE BitacoraTickets (
    BitacoraID INT IDENTITY(1,1) PRIMARY KEY,
    TicketID INT NOT NULL,
    UsuarioOperacion NVARCHAR(100) NOT NULL,
    Operacion NVARCHAR(50) NOT NULL, 
    Fecha DATETIME DEFAULT GETDATE(),
    CambiosEfectuados NVARCHAR(MAX) NOT NULL
);
GO

INSERT INTO Usuarios (NombreCompleto, Rol, Departamento) 
VALUES 
('David Meza', 'TI Admin', 'Sistemas'),
('Ana Martínez', 'Empleado', 'Finanzas'),
('Carlos Mendoza', 'Soporte', 'Sistemas');
GO
