using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient; // Importo las herramientas para conectarme con MS SQL Server.


namespace ProductosConDB
{
    

    class AccesoDatos
    {
        // Instancio la clase Conexion.
        SqlConnection conexion = new SqlConnection();
        // Instancio la clase Comando.
        SqlCommand comando = new SqlCommand();
        // Instancio el DataReader.
        SqlDataReader lector;
        // Instancio la clase DataTable.
        DataTable tabla = new DataTable();
        // Creo una variable para mi cadena de conexión.
        string cadenaConexion;

        public SqlDataReader pLector {
            set { lector = value; }
            get { return lector; }
        }

        public string pCadenaConexion {
            set { cadenaConexion = value; }
            get { return cadenaConexion; }
        }

        public AccesoDatos() {
            this.conexion = new SqlConnection();
            this.comando = new SqlCommand();
            this.tabla = new DataTable();
            this.lector = null;
            this.cadenaConexion = "";
        }

        public AccesoDatos(string cadenaConexion) {
            this.conexion = new SqlConnection();
            this.comando = new SqlCommand();
            this.tabla = new DataTable();
            this.lector = null;
            this.cadenaConexion = cadenaConexion;
        }

        // Creo un método para conectar a la DB.
        private void Conectar() {
            conexion.ConnectionString = cadenaConexion; // Le digo que se conecte con el string de la variable.
            conexion.Open(); // Abro la conexión.
            comando.Connection = conexion; // Le doy a mi objeto comando la conexión con la que va a trabajar.
            comando.CommandType = CommandType.Text; // Le digo que los comando serán de tipo texto.
        }

        public void Desconectar() { // Tiene que ser público porque cuando use el DataReader voy a tener que cerrar la conexion manualmente desde el Form y tiene que ser accesible.
            conexion.Close(); // Cierro la conexión.
            conexion.Dispose(); // Libero los recursos. 
        }

        // Creo un método para consultar las tablas con Datatable.
        public DataTable consultarTabla(string nombreTabla) {
            this.Conectar(); // Primero le digo que se conecte usando el método que creé previamente.
            comando.CommandText = "SELECT * FROM " + nombreTabla; // Le doy el query.
            tabla.Load(comando.ExecuteReader()); // Le digo que me cargue mi datatable con el resultado de la ejecución del comando.
            this.Desconectar(); // Que se desconecte de la DB.
            return this.tabla; // Y finalmente que me devuleva ese Datatable.
        }

        // Creo otro método para leer mi tablas con Datareader.
        public void LeerTabla(string nombreTabla) {
            this.Conectar(); // Primero me conecto a la DB.
            this.comando.CommandText = "SELECT * FROM " + nombreTabla; // Le doy mi query.
            this.lector = this.comando.ExecuteReader(); // Y le digo que me cargue el lector con el resultado de la ejecucion del query.
            // No puedo desconectarlo porque pierdo los datos del DataReader, así que lo tengo que hacer manualmente.
        }

        // Creo un método para actualizar la base de datos que como parámetro va a recibir el Query que voy a crear desde el form.
        public void ActualizarDB(string consultaSQL) {
            this.Conectar(); // Primero le digo que se conecte a la DB.
            comando.CommandText = consultaSQL; // Luego, que el query que traigo del form lo use como comando.
            comando.ExecuteNonQuery(); // Que lo ejecute en la DB.
            this.Desconectar(); // Y finalmente que se desconecte.
        }
    }
}
