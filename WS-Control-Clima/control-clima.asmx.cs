using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using WS_Control_Clima.Models;

namespace WS_Control_Clima
{
    /// <summary>
    /// Descripción breve de control_clima
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    
    public class control_clima : System.Web.Services.WebService
    {
        private SqlConnection con;
        private SqlDataAdapter da = new SqlDataAdapter();
        private DataTable dt = new DataTable();
        private SqlCommand cmd;
        public SqlDataReader dr;

       
        private void Conectar()
        {
            con = new SqlConnection("server=EMPRESARIO;user=sa;password=abcd.1234;initial catalog=Control_Clima");
            con.Open();
        }

        private void Desconectar()
        {
            con.Close();
        }

        private void CrearComando(String consulta)
        {
            cmd = new SqlCommand(consulta, con);
        }

        [WebMethod]
        public int ValidarUsuario(string usuario, string contrasena)
        {
            int resultado = 0;
            Conectar();
            string query = string.Format(@"SELECT * FROM USUARIOS WHERE NOMBRE='{0}' AND CONTRASEÑA='{1}'AND ESTADO='A'",usuario, contrasena);
            CrearComando(query);
            da.SelectCommand = cmd;
            try
            {
                da.Fill(dt);
                resultado = dt.Rows.Count;
                
            }
            catch (System.Exception ex)
            {
                string error = "Error: " + ex.Message;
                resultado = 0;
            }
            return resultado;

        }

        [WebMethod]
        public List<Clima> ListarClimas()
        {
            List<Clima> lista = new List<Clima>();
            Conectar();
            string query = "SELECT CL.ID, CL.NOMBRE, CIU.NOMBRE AS CIUDAD, CL.GRADOS, CAST(CL.FECHAYHORA AS DATE) AS FECHAYHORA FROM CLIMAS AS CL " +
                        "INNER JOIN CIUDADES CIU ON (CIU.ID=CL.CIUDAD) WHERE ESTADO='A'";
            CrearComando(query);
            da.SelectCommand = cmd;
            try
            {
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    Clima clima = new Clima()
                    {
                        id = Convert.ToInt32(dr["ID"].ToString()),
                        nombre = dr["NOMBRE"].ToString(),
                        ciudad = dr["CIUDAD"].ToString(),
                        grados = dr["GRADOS"].ToString(),
                        fechaHora = dr["FECHAYHORA"].ToString()

                    };
                    lista.Add(clima);
                }
            }
            catch (System.Exception ex)
            {
                string error = "Error: " + ex.Message;
            }
            return lista;

        }

        [WebMethod]
        public List<Maestro> ListarMaestro(string tabla, string filtro)
        {
            List<Maestro> lista = new List<Maestro>();
            Conectar();
            string query = string.Format(@"SELECT * FROM {0} AS TABLA {1}", tabla, filtro);
            CrearComando(query);
            da.SelectCommand = cmd;
            try
            {
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    Maestro maestro = new Maestro()
                    {
                        id = Convert.ToInt32(dr["ID"].ToString()),
                        nombre = dr["NOMBRE"].ToString(),
                    };
                    lista.Add(maestro);
                }
            }
            catch (System.Exception ex)
            {
                string error = "Error: " + ex.Message;
            }
            return lista;

        }

        [WebMethod]
        public List<Clima> ConsultarClima(int ciudad)
        {
            List<Clima> lista = new List<Clima>();
            Conectar();
            string query = string.Format(@"SELECT CL.ID, CL.NOMBRE, CIU.NOMBRE AS CIUDAD, CL.GRADOS, CL.FECHAYHORA FROM CLIMAS AS CL " +
                        "INNER JOIN CIUDADES CIU ON (CIU.ID=CL.CIUDAD) WHERE ESTADO='A'AND CL.CIUDAD={0}", ciudad);
            CrearComando(query);
            da.SelectCommand = cmd;
            try
            {
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    Clima clima = new Clima()
                    {
                        id = Convert.ToInt32(dr["ID"].ToString()),
                        nombre = dr["NOMBRE"].ToString(),
                        ciudad = dr["CIUDAD"].ToString(),
                        grados = dr["GRADOS"].ToString(),
                        fechaHora = dr["FECHAYHORA"].ToString()

                    };
                    lista.Add(clima);
                }
            }
            catch (System.Exception ex)
            {
                string error = "Error: " + ex.Message;
            }
            return lista;

        }

        [WebMethod]
        public int CrearClima(Clima clima)
        {
            List<Clima> lista = new List<Clima>();
            int regAfectados = 0;
            var fecha = Convert.ToDateTime(clima.fechaHora).ToString("s");
            Conectar();
            
            string query = string.Format(@"INSERT INTO CLIMAS(NOMBRE, CIUDAD, GRADOS, FECHAYHORA, ESTADO) VALUES ('{0}',{1},'{2}','{3}','A') ", 
                                        clima.nombre, clima.ciudad, clima.grados, fecha);
            
            CrearComando(query);
            try
            {
                regAfectados = cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                string error = "Error: " + ex.Message;
                regAfectados = -1;
            }

            return regAfectados;
        }

        [WebMethod]
        public List<Clima> RecuperarClima(int idClima)
        {
            List<Clima> lista = new List<Clima>();
            Conectar();
            string query = string.Format(@"SELECT CL.ID, CL.NOMBRE, CL.CIUDAD, CL.GRADOS, CL.FECHAYHORA FROM CLIMAS AS CL
                        INNER JOIN CIUDADES CIU ON (CIU.ID=CL.CIUDAD) WHERE CL.ID={0}", idClima);

            CrearComando(query);
            da.SelectCommand = cmd;
            try
            {
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    Clima clima = new Clima()
                    {
                        id = Convert.ToInt32(dr["ID"].ToString()),
                        nombre = dr["NOMBRE"].ToString(),
                        ciudad = dr["CIUDAD"].ToString(),
                        grados = dr["GRADOS"].ToString(),
                        fechaHora = dr["FECHAYHORA"].ToString()
                    };
                    lista.Add(clima);
                }
            }
            catch (System.Exception ex)
            {
                string error = "Error: " + ex.Message;
            }
            return lista;

        }

        [WebMethod]
        public int GuardarClima(Clima clima)
        {
            int regAfectados = 0;
            var fecha = Convert.ToDateTime(clima.fechaHora).ToString("s");
            Conectar();
            string query = string.Format(@"UPDATE CLIMAS SET NOMBRE='{0}', CIUDAD={1}, GRADOS='{2}',FECHAYHORA='{3}' 
                                           WHERE ID={4}", clima.nombre, clima.ciudad, clima.grados, fecha, clima.id);
            CrearComando(query);            
            try
            {
                regAfectados = cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                string error = "Error: " + ex.Message;
                regAfectados=-1;
            }

            return regAfectados;
        }

        [WebMethod]
        public int EliminarClima(int id)
        {
            int regAfectados = 0;
            Conectar();
            string query = string.Format(@"UPDATE CLIMAS SET ESTADO='B' WHERE ID={0}",id);
            CrearComando(query);
            try
            {
                regAfectados = cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                string error = "Error: " + ex.Message;
                regAfectados = -1;
            }

            return regAfectados;
        }
    }
}
