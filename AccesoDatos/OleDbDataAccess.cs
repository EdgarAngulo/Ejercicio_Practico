using System;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;
using AccesoDatos.Entidades;

namespace AccesoDatos.cad
{
    	/// <summary>
	/// El OleDbDataAccessLayer contiene la capa de acceso a datos para proveedor de
	/// datos Oledb. Esta clase implementa los metodos abstractos en la clase
	/// AccesoDatosClaseBase.
	/// </summary>
    public class OleDbDataAccessLayer : AccesoDatosClaseBase
    {
        // constructores del proveedor
        public OleDbDataAccessLayer() { }
        public OleDbDataAccessLayer(string connectionString) { this.ConnectionString = connectionString; }
        public OleDbDataAccessLayer(DatosConexion dc)
        {
            this.DatosDeConexion = dc;
        }

        // Miembros AccesoDatosClaseBase
        internal override IDbConnection obtenerDataProviderConnection()
        {
            return new OleDbConnection();
        }
        internal override IDbCommand obtenerDataProviderCommand()
        {
            return new OleDbCommand();
        }

        internal override IDbDataAdapter obtenerDataProviderDataAdapter()
        {
            return new OleDbDataAdapter();
        }
    }
}


