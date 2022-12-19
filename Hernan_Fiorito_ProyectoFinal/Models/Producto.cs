namespace Hernan_Fiorito_ProyectoFinal.Models
{
    public class Producto
    {
        public long id { get; set; }
        public string descripcion { get; set; }
        public float costo { get; set; }
        public float precioVenta { get; set; }
        public int stock { get; set; }
        public long idUsuario { get; set; }

        public Producto()
        {
            id = 0;
            descripcion = "";
            costo = 0;
            precioVenta = 0;
            stock = 0;
            idUsuario = 0;

        }

        public Producto(long id, string descripcion, float costo, float precioVenta, int stock, long idUsuario)
        {
            this.id = id;
            this.descripcion = descripcion;
            this.costo = costo;
            this.precioVenta = precioVenta;
            this.stock = stock;
            this.idUsuario = idUsuario;
        }
    }
}
