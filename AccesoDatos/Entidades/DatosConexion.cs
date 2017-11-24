using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccesoDatos.Entidades
{
    public class DatosConexion
    {
        private string _baseDeDatos;
        private string _ip;
        private string _usuario;
        private string _pass;

        public DatosConexion(string BaseDeDatos, string Ip, string Usuario, string Pass)
        {
            this.BaseDeDatos = BaseDeDatos;
            this.Ip = Ip;
            this.Usuario = Usuario;
            this.Pass = Pass;
        }

        public string BaseDeDatos
        {
            //read property
            get { return _baseDeDatos; }
            //write property
            set { _baseDeDatos = value; }
        }

        public string Ip
        {
            //read property
            get { return _ip; }
            //write property
            set { _ip = value; }
        }

        public string Usuario
        {
            //read property
            get { return _usuario; }
            //write property
            set { _usuario = value; }
        }

        public string Pass
        {
            //read property
            get { return _pass; }
            //write property
            set { _pass = value; }
        }

    }
}
