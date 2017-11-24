using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using AccesoDatos.Entidades;

namespace AccesoDatos.cad
{
    /// <summary>
	/// La clase AccesoDatosClaseBase enlista todos los metodos abstractos que cada
	/// proveedor de acceso a datos debe implementar
	/// </summary>
    
    public abstract class AccesoDatosClaseBase
    {
        //Miembros privados

        #region -- Miembros privados, Metodos y Constructores --

        private string sConnectionString;
        private IDbConnection connection;
        private IDbCommand command;
        private IDbTransaction transaction;
        private DatosConexion datosConexion;
        private string sDriverServidor;

        #endregion

        #region -- Propiedades --

        public string ConnectionString
        {
            get
            {
                //aseguramos que la cadena no venga vacia
                if (sConnectionString == string.Empty || sConnectionString.Length == 0)
                {
                    throw new Exception("La cadena de conexion es invalida!!");
                }
                return sConnectionString;
            }
            set
            {
                sConnectionString = value;
            }
        }

        public DatosConexion DatosDeConexion
        {
            get
            { return datosConexion; }
            set
            { datosConexion = value; }
        }

        public string DriverServidor
        {
            get
            { return sDriverServidor; }
            set
            { sDriverServidor = value; }
        }

        #endregion

        #region -- Constructores --

        public AccesoDatosClaseBase() 
        {

        }

        #endregion          

        #region -- Metodos Abstractos --

        /// <summary>
        /// Datos específicos de implementación del proveedor de acceso a bases de datos relacionales.
        /// </summary>
        /// <returns></returns>
        internal abstract IDbConnection obtenerDataProviderConnection();
        /// <summary>
        /// Proveedor específico para la ejecución de sentencia de SQL mientras está conectado a un origen de datos
        /// </summary>
        /// <returns></returns>
        internal abstract IDbCommand obtenerDataProviderCommand();
        /// <summary>
        /// Proveedor específico para el llnado de dataset.
        /// </summary>
        /// <returns></returns>
        internal abstract IDbDataAdapter obtenerDataProviderDataAdapter();

        #endregion

        #region -- Transacciones de Base de Datos --
        /// <summary>
        /// Inicia una transacccion en la DB. Utilizamos nombre en ingles para facilitar la intepretaciòn del desarrollador
        /// y asociación con las transacciones habituales.
        /// </summary>
        public void beginTransaction()
        {
            if (transaction != null)
                return;
            try
            {
                //instanciamos un objeto coneccion
                connection = obtenerDataProviderConnection();
                connection.ConnectionString = this.ConnectionString;                
                connection.Open();
                //iniciamos la transaccion de la base de datos
                transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            }
            catch
            {
                connection.Close();
                throw;
            }
        }

        /// <summary>
        /// Realiza el commit a la base de datos,Utilizamos nombre en ingles para facilitar la intepretaciòn del desarrollador
        /// y asociación con las transacciones habituales.
        /// </summary>
        public void commitTransaction()
        {
            if (transaction == null)
                return;

            try
            {
                // hace el comit
                transaction.Commit();
            }
            catch
            {
                // revierte
                rollbackTransaction();
                throw;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
        }

        /// <summary>
        /// Hace el roolback del estado .Utilizamos nombre en ingles para facilitar la intepretaciòn del desarrollador
        /// y asociación con las transacciones habituales.
		/// </summary>
        public void rollbackTransaction()
        {
            if (transaction == null)
                return;

            try
            {
                transaction.Rollback();
            }
            catch { }
            finally
            {
                connection.Close();
                transaction = null;
            }
        }
		
        #endregion

        #region -- ExecuteDataReader --

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public IDataReader executeDataReader(string commandText)
        {
            return this.executeDataReader(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Ejecuta CommandText en Connection y genera un IDataReader.
        /// </summary>
        public IDataReader executeDataReader(string commandText, CommandType commandType)
        {
            return this.executeDataReader(commandText, commandType, null);
        }

        /// <summary>
        /// Ejecuta el  CommandText parametrizado en la Connection y genera un IDataReader.
        /// </summary>
        public IDataReader executeDataReader(string commandText, IDataParameter[] commandParameters)
        {
            return this.executeDataReader(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Ejectua un comando en la conexion y genra un IDataReader.
        /// </summary>
        public IDataReader executeDataReader(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        { 
            try
            {
                preparaCommand(commandType, commandText, commandParameters);

                IDataReader dr;

                if (transaction == null)
                {
                    // Genera el reader.CommandBehavior.CloseConnection realiza el cierre
                    // de la conexion que debera ser cerrada cuando el reader se cierre
                    command.CommandTimeout = 1000000000;
                    dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                }
                else
                {
                    command.CommandTimeout = 1000000000;
                    dr = command.ExecuteReader();
                }

                return dr;
            }
            catch
            {
                if (transaction == null)
                {
                    connection.Close();
                    command.Dispose();
                }
                else
                    rollbackTransaction();

                throw;
            }
        }

        #endregion

        #region -- ExecuteQuery --

        /// <summary>
        /// Ejecuta una sentencia SQL en el objeto de conexión de un proveedor de datos de. NET Framework, y devuelve el número de filas afectadas.
        /// </summary>
        public int executeQuery(string commandText)
        {
            return this.executeQuery(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Ejecuta una sentencia SQL en el objeto de conexión de un proveedor de datos de. NET Framework, y devuelve el número de filas afectadas.
        /// </summary>
        public int executeQuery(string commandText, CommandType commandType)
        {
            return this.executeQuery(commandText, commandType, null);
        }

        /// <summary>
        /// Ejecuta una sentencia SQL con parámetros declarados en el objeto de conexión de un proveedor de datos de. NET Framework, y devuelve el número de filas afectadas.
        /// </summary>
        public int executeQuery(string commandText, IDataParameter[] commandParameters)
        {
            return this.executeQuery(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado en el objeto de conexión de un proveedor de datos de. NET Framework, y devuelve el número de filas afectadas
        /// </summary>
        public int executeQuery(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            try
            {
                preparaCommand(commandType, commandText, commandParameters);

                // ejectua commando
                command.CommandTimeout = 1000000000;
                int iRowsAfectados = command.ExecuteNonQuery();
                // retorna el nu, de rows afectados
                return iRowsAfectados;
            }
            catch
            {
                if (transaction != null)
                    rollbackTransaction();

                throw;
            }
            finally
            {
                if (transaction == null)
                {
                    connection.Close();
                    command.Dispose();
                }
            }
        }

        #endregion

        #region -- ExecuteScalar --

        /// <summary>
        /// Ejecuta la consulta y devuelve la primera columna de la primera fila del conjunto de resultados devueltos por la consulta. Columnas o filas adicionales se omiten.
        /// </summary>
        public object executeScalar(string commandText)
        {
            return this.executeScalar(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Ejecuta la consulta y devuelve la primera columna de la primera fila del conjunto de resultados devueltos por la consulta. Columnas o filas adicionales se omiten.
        /// </summary>
        public object executeScalar(string commandText, CommandType commandType)
        {
            return this.executeScalar(commandText, commandType, null);
        }

        /// <summary>
        /// Ejecuta una consulta con parámetros, y devuelve la primera columna de la primera fila del conjunto de resultados devueltos por la consulta. Columnas o filas adicionales se omiten.
        /// </summary>
        public object executeScalar(string commandText, IDataParameter[] commandParameters)
        {
            return this.executeScalar(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado y devuelve la primera columna de la primera fila del conjunto de resultados devueltos por la consulta. Columnas o filas adicionales se omiten
        /// </summary>
        public object executeScalar(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            try
            {
                preparaCommand(commandType, commandText, commandParameters);

                // ejectua el commando
                object objValue = command.ExecuteScalar();
                // checar el valor
                if (objValue != DBNull.Value)
                    // retorna valor
                    return objValue;
                else
                    // retorna null 
                    return null;
            }
            catch
            {
                if (transaction != null)
                    rollbackTransaction();

                throw;
            }
            finally
            {
                if (transaction == null)
                {
                    connection.Close();
                    command.Dispose();
                }
            }
        }

        #endregion

        /// <summary>
        /// Este comando abre (si es necesario) y asigna una coneccion , transaccion  tipo de comando y parametros
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        private void preparaCommand(CommandType commandType, string commandText, IDataParameter[] commandParameters)
        {
            if (connection == null)
            {
                connection = obtenerDataProviderConnection();

                if (connection.ConnectionString == string.Empty)
                {
                    if (DriverServidor != null)
                    {
                        connection.ConnectionString = connection.ConnectionString = "Driver={" + DriverServidor + "};database=" + datosConexion.BaseDeDatos.Trim() + ";server=" + datosConexion.Ip.Trim() + ";uid=" + datosConexion.Usuario.Trim() + ";pwd=" + datosConexion.Pass.Trim();
                    }
                    else
                    {
                        connection.ConnectionString = connection.ConnectionString = "database=" + datosConexion.BaseDeDatos.Trim() + ";server=" + datosConexion.Ip.Trim() + ";uid=" + datosConexion.Usuario.Trim() + ";pwd=" + datosConexion.Pass.Trim();
                    }
                }
            }
            //si el proveedor de datos no tiene un coneccion abierta abrir coneccion
            if (connection.State != ConnectionState.Open)
                connection.Open();
            //Proporciona objeto de comando del proveedor de datos específicos , si el objeto comando es nulo
            if (command == null)
                command = obtenerDataProviderCommand();
            //asociamos la coneccion con el comando
            command.Connection = connection;
            //establecemos el comando, sencia SQL o Sp.
            command.CommandText = commandText;
            //ponemos tipo de comando
            command.CommandType = commandType;
            //si proveemos una transaccion  la asignamos
            if (transaction != null)
                command.Transaction = transaction;
            //adjuntamos los parametros al comando si son proporcionados
            if (commandParameters != null)
            {
                foreach (IDataParameter param in commandParameters)
                    command.Parameters.Add(param);
            }
        }
    }
}
