using Hernan_Fiorito_ProyectoFinal.Models;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.ComponentModel;

namespace Hernan_Fiorito_ProyectoFinal.Repositories
{
    public class UsuarioRepository
    {

        private SqlConnection? conexion;
        private string cadenaConexion = "Server=sql.bsite.net\\MSSQL2016;Database=harry9110_sistemagestion;User Id=harry9110_sistemagestion;Password=Emma9110..9110;";

        public UsuarioRepository()
        {
            try
            {
                conexion = new SqlConnection(cadenaConexion);
            }
            catch (Exception ex)
            {
            }
        }

        public Usuario obtenerUsuarioDesdeReader(SqlDataReader reader)
        {
            Usuario user = new Usuario();
            user.id = long.Parse(reader["Id"].ToString());
            user.nombre = reader["Nombre"].ToString();
            user.apellido = reader["Apellido"].ToString();
            user.nombreUsuario = reader["NombreUsuario"].ToString();
            user.contrasenia = reader["Contraseña"].ToString();
            user.mail = reader["Mail"].ToString();
            return user;
        }

        //Listar usuario para chequear si existe el nombreusuario al crear uno nuevo
        public List<Usuario> listarUsuario()
        {
            List<Usuario> listaU = new List<Usuario>();
            if(conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using(SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Usuario user = obtenerUsuarioDesdeReader(reader);
                            listaU.Add(user);
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
            return listaU;
        }

        // Crear un usuario pero chequear que el mismo no exista ya con el mismo nombreUsuario
        public Usuario crearUsuario(Usuario user)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                List<Usuario> listaU = listarUsuario();
                using(SqlCommand cmd = new SqlCommand("INSERT INTO Usuario (Nombre, Apellido, NombreUsuario, Contraseña, Mail) " +
                    "VALUES (@nombre, @apellido, @nombreUsuario, @contraseña, @mail); SELECT @@Identity", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("Nombre", SqlDbType.VarChar) { Value = user.nombre });
                    cmd.Parameters.Add(new SqlParameter("Apellido", SqlDbType.VarChar) { Value = user.apellido });
                    foreach (Usuario usuario in listaU)
                    {
                        if (user.nombreUsuario == usuario.nombreUsuario)
                        {
                            //throw new Exception("Ya existe un usuario con el mismo Nombre de Usuario, pruebe con otro");
                            Random random = new Random();
                            int num = random.Next(1, 999990);
                            user.nombreUsuario = user.nombre + num.ToString();
                            
                        }  
                    }
                    cmd.Parameters.Add(new SqlParameter("NombreUsuario", SqlDbType.VarChar) { Value = user.nombreUsuario });
                    cmd.Parameters.Add(new SqlParameter("Contraseña", SqlDbType.VarChar) { Value = user.contrasenia });
                    cmd.Parameters.Add(new SqlParameter("Mail", SqlDbType.VarChar) { Value = user.mail });
                    user.id = long.Parse(cmd.ExecuteScalar().ToString());
                    return user;
                    
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

        // traer usuario desde id para usarlo luego en modificarUsuarioDesdeId
        public Usuario obtenerUsuarioDesdeID(long id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE Id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = id });
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Usuario user = obtenerUsuarioDesdeReader(reader);
                            return user;
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
        // Modificar usuario desde id
        public Usuario? actualizarUsuario(long id, Usuario usuarioAActualizar)
        {
            if(conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                Usuario user = obtenerUsuarioDesdeID(id);
                if(user == null)
                {
                    return null;
                }
                List<string> camposAActualizar = new List<string>();
                if (user.nombre != usuarioAActualizar.nombre && !string.IsNullOrEmpty(usuarioAActualizar.nombre))
                {
                    camposAActualizar.Add("Nombre = @nombre");
                    user.nombre = usuarioAActualizar.nombre;
                }
                if (user.apellido != usuarioAActualizar.apellido && !string.IsNullOrEmpty(usuarioAActualizar.apellido))
                {
                    camposAActualizar.Add("Apellido = @apellido");
                    user.apellido = usuarioAActualizar.apellido;
                }
                if (user.nombreUsuario != usuarioAActualizar.nombreUsuario && !string.IsNullOrEmpty(usuarioAActualizar.nombreUsuario))
                {
                    camposAActualizar.Add("NombreUsuario = @nombreUsuario");
                    user.nombreUsuario = usuarioAActualizar.nombreUsuario;
                }
                if (user.contrasenia != usuarioAActualizar.contrasenia && !string.IsNullOrEmpty(usuarioAActualizar.contrasenia))
                {
                    camposAActualizar.Add("Contraseña = @contraseña");
                    user.contrasenia = usuarioAActualizar.contrasenia;
                }
                if (user.mail != usuarioAActualizar.mail && !string.IsNullOrEmpty(usuarioAActualizar.mail))
                {
                    camposAActualizar.Add("Mail = @mail");
                    user.mail = usuarioAActualizar.mail;
                }
                if(camposAActualizar.Count == 0)
                {
                    throw new Exception("No new fields to update");
                }
                using (SqlCommand cmd = new SqlCommand($"UPDATE Producto SET {String.Join(", ", camposAActualizar)} WHERE id = @id", conexion))
                {
                    cmd.Parameters.Add(new SqlParameter("Nombre", SqlDbType.VarChar) { Value = usuarioAActualizar.nombre });
                    cmd.Parameters.Add(new SqlParameter("Apellido", SqlDbType.VarChar) { Value = usuarioAActualizar.apellido });
                    cmd.Parameters.Add(new SqlParameter("NombreUsuario", SqlDbType.VarChar) { Value = usuarioAActualizar.nombreUsuario });
                    cmd.Parameters.Add(new SqlParameter("Contraseña", SqlDbType.VarChar) { Value = usuarioAActualizar.contrasenia });
                    cmd.Parameters.Add(new SqlParameter("Mail", SqlDbType.VarChar) { Value = usuarioAActualizar.mail });
                    conexion.Open();
                    cmd.ExecuteReader();
                    return user;
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

        // Traer un usuario desde el parámetro nombreUsuario y mostrar todos sus datos 
        public Usuario obtenerUsuarioDsdeNU(string nombreU)
        {
            if(conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using(SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE NombreUsuario = @nombreUsuario", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("NombreUsuario", SqlDbType.VarChar) { Value = nombreU });
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Usuario user = obtenerUsuarioDesdeReader(reader);
                            return user;
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
        // Eliminar usuario desde Id
        public bool eliminarUsuario(int id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                int filasAfectadas = 0;
                using(SqlCommand cmd = new SqlCommand ("DELETE FROM Usuario WHERE Id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("Id", SqlDbType.BigInt) { Value = id });
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

    }
}
