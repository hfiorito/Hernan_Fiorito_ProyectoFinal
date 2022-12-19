using System.Diagnostics.Eventing.Reader;

namespace Hernan_Fiorito_ProyectoFinal.Models
{
    public class ProductoVendido
    {
        public long id { get; set; }
        public long idProducto { get; set; }
        public int stock { get; set; }
        public long idVenta { get; set; }
        

        public ProductoVendido()
        {
            id = 0;
            idProducto = 0;
            stock = 0;
            idVenta = 0;
            
        }
        public ProductoVendido(long id, long idProducto, int stock, long idVenta)
        {
            this.id = id;
            this.idProducto = idProducto;
            this.stock = stock;
            this.idVenta = idVenta;
            
        }

        
    }
}
