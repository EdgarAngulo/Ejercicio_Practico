--DROP TABLE cat_clientes

CREATE TABLE cat_clientes
(
	num_cliente				INT IDENTITY(1,1) PRIMARY KEY,
	des_nombre				VARCHAR(20) NOT NULL DEFAULT '',
	des_apellidopaterno		VARCHAR(20) NOT NULL DEFAULT '',
	des_apellidomaterno		VARCHAR(20) NOT NULL DEFAULT '',
	des_direccion			VARCHAR(100) NOT NULL DEFAULT ''
)

INSERT INTO cat_clientes (des_nombre, des_apellidopaterno, des_apellidomaterno, des_direccion)
VALUES	('NOMBRE1', 'APELLIDOPATERNO1', 'APELLIDOMATERNO1', 'DIRECCION1'),
		('NOMBRE2', 'APELLIDOPATERNO2', 'APELLIDOMATERNO2', 'DIRECCION2'),
		('NOMBRE3', 'APELLIDOPATERNO3', 'APELLIDOMATERNO3', 'DIRECCION3'),
		('NOMBRE4', 'APELLIDOPATERNO4', 'APELLIDOMATERNO4', 'DIRECCION4'), 
		('NOMBRE5', 'APELLIDOPATERNO5', 'APELLIDOMATERNO5', 'DIRECCION5')
