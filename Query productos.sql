CREATE DATABASE Productos
USE Productos

-- Tabla Marcas
CREATE TABLE Marcas (
	id_marca int identity(1,1) NOT NULL,
	detalle varchar(50),
	constraint pk_id_marca primary key (id_marca)
)

--Tabla Productos
CREATE TABLE Productos (
	codigo int identity (1,1) not null,
	detalle varchar(50),
	tipo int,
	id_marca int,
	precio decimal(10,2),
	fecha datetime,
	constraint pk_codigo primary key (codigo),
	constraint fk_id_marca foreign key (id_marca) references Marcas(id_marca)
)

--Marcas de prueba
INSERT INTO Marcas (detalle)
VALUES ('HP'),('EPSON'),('COMPAQ'),('DELL'),('ASUS'),('BANGHO'),('SONY')

-- Productos de prueba.
INSERT INTO Productos (detalle, tipo, id_marca, precio, fecha)
VALUES ('Pavilion',1,1,5000,'2014-09-01'),
	   ('Studio',2,4,6000,'2014-10-01')

-- Cosultas
SELECT * FROM Marcas

SELECT * FROM Productos

Select * from Productos P
JOIN Marcas M ON M.id_marca = P.id_marca