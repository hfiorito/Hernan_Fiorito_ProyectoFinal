using Hernan_Fiorito_ProyectoFinal.Models;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.ComponentModel;
using Microsoft.AspNetCore.Connections;

namespace Hernan_Fiorito_ProyectoFinal.Repositories
{
    public class ProductoRepository
    {
        private SqlConnection? conexion;
        private string cadenaConexion= "Server=sql.bsite.net\\MSSQL2016;Database=harry9110_sistemagestion;User Id=harry9110_sistemagestion;Password=Emma9110..9110;";

        public ProductoRepository()
        {
            try
            {
                conexion = new SqlConnection(cadenaConexion);
            }
            catch (Exception ex)
            {
            }
        }

        public Producto obtenerProductoDesdeReader(SqlDataReader reader)
        {
            Producto producto = new Producto();
            producto.id = long.Parse(reader["Id"].ToString());
            producto.descripcion = reader["Descripciones"].ToString();
            producto.costo = float.Parse(reader["Costo"].ToString());
            producto.precioVenta = float.Parse(reader["PrecioVenta"].ToString());
            producto.stock = int.Parse(reader["Stock"].ToString());
            producto.idUsuario = long.Parse(reader["IdUsuario"].ToString());
            return producto;
        }

        //Crear Producto
        public Producto crearProducto(Producto producto)
        {
            
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try 
            {
                using(SqlCommand cmd = new SqlCommand("INSERT INTO Producto (Descripciones, Costo, PrecioVenta, Stock, IdUsuario) " +
                    "VALUES (@descripciones, @costo, @precioVenta, @stock, @idUsuario)", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("Descipciones", SqlDbType.VarChar) { Value = producto.descripcion });
                    cmd.Parameters.Add(new SqlParameter("Costo", SqlDbType.Float) { Value = producto.costo });
                    cmd.Parameters.Add(new SqlParameter("PrecioVenta", SqlDbType.Float) { Value = producto.precioVenta });
                    cmd.Parameters.Add(new SqlParameter("Stock", SqlDbType.Int) { Value = producto.stock });
                    cmd.Parameters.Add(new SqlParameter("IdUsuario", SqlDbType.BigInt) { Value = producto.idUsuario });
                    producto.id = long.Parse(cmd.ExecuteScalar().ToString());
                    return producto;
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

        // Obtener producto desde ID para usar en Modificar producto desde ID
        public Producto obtnerProductoDesdeID(long id)
        {
            if (conexion == null) { throw new Exception("Conexión no establecida"); }
            try
            {
                using (SqlCommand cmd = new SqlCommand ("SELECT * FROM Producto WHERE Id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = id });
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Producto producto = obtenerProductoDesdeReader(reader);
                            return producto;
                        }
                        else
                        {
                            return null;
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
        }

        //Modificar Producto desde id
        public Producto? modificarProductoDesdeId(long id, Producto productoAActualizar)
        {
            if(conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                Producto? producto = obtnerProductoDesdeID(id);
                if (producto == null)
                {
                    return null;
                }
                List<string> camposAActualizar = new List<string>();
                if (producto.descripcion != productoAActualizar.descripcion && !string.IsNullOrEmpty(productoAActualizar.descripcion))
                {
                    camposAActualizar.Add("descripciones = @descripcion");
                    producto.descripcion = productoAActualizar.descripcion;
                }
                if (producto.costo != productoAActualizar.costo && productoAActualizar.costo > 0)
                {
                    camposAActualizar.Add("costo = @costo");
                    producto.costo = productoAActualizar.costo;
                }
                if (producto.precioVenta != productoAActualizar.precioVenta && productoAActualizar.precioVenta > 0)
                {
                    camposAActualizar.Add("precioVenta = @precioVenta");
                    producto.precioVenta = productoAActualizar.precioVenta;
                }
                if (producto.stock != productoAActualizar.stock && productoAActualizar.stock > 0)
                {
                    camposAActualizar.Add("Stock = @stock");
                    producto.stock = productoAActualizar.stock;
                }
                if (camposAActualizar.Count == 0)
                {
                    throw new Exception("No new fields to update");
                }
                using (SqlCommand cmd = new SqlCommand($"UPDATE Producto SET {String.Join(", ", camposAActualizar)} WHERE id = @id", conexion))
                {
                    cmd.Parameters.Add(new SqlParameter("descripcion", SqlDbType.VarChar) { Value = productoAActualizar.descripcion });
                    cmd.Parameters.Add(new SqlParameter("costo", SqlDbType.Float) { Value = productoAActualizar.costo });
                    cmd.Parameters.Add(new SqlParameter("precioVenta", SqlDbType.Float) { Value = productoAActualizar.precioVenta });
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = productoAActualizar.stock });
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    return producto;
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

        //Traer todos los productos
        public List<Producto> listarProductos()
        {
            List<Producto> lista = new List<Producto>();
            if (conexion == null) { throw new Exception("Conexión no establecida"); }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Producto",conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows) 
                        {
                            Producto producto = obtenerProductoDesdeReader(reader);
                            lista.Add(producto);
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

        

        // Descontar Stock
        public Producto? descontarStock(int stock, long id)
        {
            
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {

                Producto producto = obtnerProductoDesdeID(id);
                if (producto == null)
                {
                    return null;
                }
                List<string> camposAActualizar = new List<string>();
                camposAActualizar.Add("descripciones = @descripcion");
                camposAActualizar.Add("costo = @costo");
                camposAActualizar.Add("precioVenta = @precioVenta");
                camposAActualizar.Add("Stock = @stock");
                if (producto.stock >= stock)
                {
                    producto.stock = producto.stock - stock;
                    
                }
                else
                {
                    stock = producto.stock;
                    producto.stock = 0;
                    
                }
                using (SqlCommand cmd = new SqlCommand($"UPDATE Producto SET {String.Join(", ", camposAActualizar)} WHERE id = @id", conexion))
                {
                    cmd.Parameters.Add(new SqlParameter("descripcion", SqlDbType.VarChar) { Value = producto.descripcion });
                    cmd.Parameters.Add(new SqlParameter("costo", SqlDbType.Float) { Value = producto.costo });
                    cmd.Parameters.Add(new SqlParameter("precioVenta", SqlDbType.Float) { Value = producto.precioVenta });
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = producto.stock });
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    return producto;
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
        // sumar Stock al eliminar venta
        public Producto sumarStock(int stock, long id)
        {
            
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {

                Producto producto = obtnerProductoDesdeID(id);
                if (producto == null)
                {
                    return null;
                }
                List<string> camposAActualizar = new List<string>();
                camposAActualizar.Add("descripciones = @descripcion");
                camposAActualizar.Add("costo = @costo");
                camposAActualizar.Add("precioVenta = @precioVenta");
                camposAActualizar.Add("Stock = @stock");
                producto.stock = producto.stock + stock;
                                
                using (SqlCommand cmd = new SqlCommand($"UPDATE Producto SET {String.Join(", ", camposAActualizar)} WHERE id = @id", conexion))
                {
                    cmd.Parameters.Add(new SqlParameter("descripcion", SqlDbType.VarChar) { Value = producto.descripcion });
                    cmd.Parameters.Add(new SqlParameter("costo", SqlDbType.Float) { Value = producto.costo });
                    cmd.Parameters.Add(new SqlParameter("precioVenta", SqlDbType.Float) { Value = producto.precioVenta });
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = producto.stock });
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    return producto;
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
        //Eliminar Producto desde id

        public bool eliminarProducto(long id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                int filasAfectadas = 0;
                using(SqlCommand cmd = new SqlCommand ("DELETE FROM Producto WHERE id=@id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    filasAfectadas = cmd.ExecuteNonQuery();
                }

                return filasAfectadas >0;
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

    }
}
