using System;
using System.Data;
using System.Data.Odbc;
using System.Data.Common;
using AccesoDatos.Entidades;

namespace AccesoDatos.cad
{
    	/// <summary>
	/// El OdbcDataAccessLayer contiene la capa de acceso a datos para proveedor de
	/// datos Odbc. Esta clase implementa los metodos abstractos en la clase
	/// AccesoDatosClaseBase.
	/// </summary>
    public class OdbcDataAccessLayer : AccesoDatosClaseBase
    {
        // constructores del proveedor
        public OdbcDataAccessLayer() { }
        public OdbcDataAccessLayer(string connectionString) { this.ConnectionString = connectionString; }
        public OdbcDataAccessLayer(DatosConexion dc)
        {
            this.DatosDeConexion = dc;
        }

        /// <summary>
        /// Establece que la Conexion sera a un Servidor de SQL SERVER
        /// </summary>
        public void ServidorSqlServer()
        {
            this.DriverServidor = "SQL Server";
        }

        /// <summary>
        /// Establece que la Conexion sera a un Servidor de POSTGRESQL
        /// </summary>
        public void ServidorPostgreSql()
        {
            this.DriverServidor = "PostgreSQL";
        }

        // miembros de AccesoDatosClaseBase
        internal override IDbConnection obtenerDataProviderConnection()
        {
            return new OdbcConnection();
        }
        internal override IDbCommand obtenerDataProviderCommand()
        {
            return new OdbcCommand();
        }
        internal override IDbDataAdapter obtenerDataProviderDataAdapter()
        {
            return new OdbcDataAdapter();
        }
    }
}
