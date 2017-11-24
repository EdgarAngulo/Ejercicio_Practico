using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using AccesoDatos.Entidades;

namespace AccesoDatos.cad
{
	/// <summary>
	/// El SqlDataAccessLayer contiene la capa de acceso a datos para servidor SQL
	/// Server Esta clase implementa los metodos abstractos en la clase
	/// AccesoDatosClaseBase.
	/// </summary>
    public class SqlDataAccessLayer : AccesoDatosClaseBase
    {
        // constructores del proveedor
        public SqlDataAccessLayer() { }
        public SqlDataAccessLayer(string connectionString) { this.ConnectionString = connectionString; }
        public SqlDataAccessLayer(DatosConexion dc)
        {
            this.DatosDeConexion = dc;
        }

        // AccesoDatosClaseBase
        internal override IDbConnection obtenerDataProviderConnection()
        {
            return new SqlConnection();
        }
        internal override IDbCommand obtenerDataProviderCommand()
        {
            return new SqlCommand();
        }

        internal override IDbDataAdapter obtenerDataProviderDataAdapter()
        {
            return new SqlDataAdapter();
        }
    }
}
