using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccesoDatos.cad
{
    /// <summary>
    /// controlamos los errores de la capa de datos
    /// </summary>
    public class CBaseDatosException : Exception
    {
        /// <summary>
        /// Construye una instancia en base a un mensaje de error y excepcion original
        /// </summary>
        /// <param name="mensaje">el mensaje de error</param>
        /// <param name="original">la excepcion original</param>
        public CBaseDatosException(string mensaje, Exception original) : base(mensaje, original) { }

        /// <summary>
        /// Construye una instancia en base a un mensaje de error
        /// </summary>
        /// <param name="mensaje">el mensaje de error</param>
        public CBaseDatosException(string mensaje) : base(mensaje) { }
    }
}
