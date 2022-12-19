using Hernan_Fiorito_ProyectoFinal.Models;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.ComponentModel;
using Hernan_Fiorito_ProyectoFinal.Repositories;

namespace Hernan_Fiorito_ProyectoFinal.Repositories
{
    
    public class VentaRepository
    {
        private ProductoRepository prodRepos = new ProductoRepository();
        private SqlConnection? conexion;
        private string cadenaConexion = "Server=sql.bsite.net\\MSSQL2016;Database=harry9110_sistemagestion;User Id=harry9110_sistemagestion;Password=Emma9110..9110;";

        public VentaRepository()
        {
            try
            {
                conexion = new SqlConnection(cadenaConexion);
            }
            catch (Exception ex)
            {
            }
        }
        public Venta obtenerVentaDesdeReader(SqlDataReader reader)
        {
            Venta vta = new Venta();
            vta.id = long.Parse(reader["Id"].ToString());
            vta.comentarios = reader["Comentarios"].ToString();
            vta.idUsuario = long.Parse(reader["IdUsuario"].ToString());
            return vta;
        }
        public List<Venta> listaVentaDesdeIdUser(int idUser)
        {
            List<Venta> ventaU = new List<Venta>();
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROm Venta WHERE idUsuario = @idUsuario", conexion))
                {
                    conexion.Open();
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Venta venta = obtenerVentaDesdeReader(reader);
                            ventaU.Add(venta);

                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
            return ventaU;
        }

        // Cargar una venta en la tabla venta, cargar la venta el ProductoVendido y descontar stock en tabla Producto
        public Venta cargarVenta(Venta venta)
        {
            if(conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using(SqlCommand cmd = new SqlCommand("INSERT INTO Venta (Id, Comentarios, IdUsuario) VALUES (@id, @comentarios, @idUsuario); SELECT @@Identity", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("Comentarios", SqlDbType.VarChar) { Value = venta.comentarios });
                    cmd.Parameters.Add(new SqlParameter("IdUsuario", SqlDbType.BigInt) { Value = venta.idUsuario });
                    venta.id= long.Parse(cmd.ExecuteScalar().ToString());
                    return venta;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
        }
        //Eliminar venta desde Id, eliminar los ítem de ProductoVendido con esa venta, sumar stock a los Productos
        public bool eliminarVenta(long id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");

            }
            try
            {
                int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Venta WHERE id=@id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    filasAfectadas = cmd.ExecuteNonQuery();
                }

                return filasAfectadas > 0;
            }
            catch
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
        }
        // Traer todas las ventas hechas con las descripciones de los respectivos productos
        public List<Venta> listarVentas()
        {
            List<Venta> lista = new List<Venta>();
            if (conexion== null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using(SqlCommand cmd = new SqlCommand("SELECT * FROM Venta", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Venta venta = obtenerVentaDesdeReader(reader);
                            lista.Add(venta);

                        }
                    }
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
            return lista;
        }
    }
}
