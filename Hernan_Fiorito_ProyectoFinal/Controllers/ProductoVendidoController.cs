using Hernan_Fiorito_ProyectoFinal.Models;
using Hernan_Fiorito_ProyectoFinal.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Hernan_Fiorito_ProyectoFinal.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductoVendidoController : Controller
    {
        private ProductoVendidoReposiroty repository = new ProductoVendidoReposiroty();

        //Cargar produto vendido
        [HttpPost]
        public ActionResult Post([FromBody] ProductoVendido productoV)
        {
            try
            {
                ProductoVendido productoVendido = repository.cargarProductosVendidos(productoV);
                return StatusCode(StatusCodes.Status201Created, productoVendido);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        //eliminar producto vendido desde IdVenta
        [HttpDelete]
        public ActionResult Delete([FromBody] long idVta)
        {
            try
            {
                bool seElimino = repository.eliminarProductoVendido(idVta);
                if (seElimino)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpGet]//listo productos
        public ActionResult<List<ProductoVendido>> Get(long idVenta)
        {
            try
            {
                List<ProductoVendido> lista = repository.obtenerProdVendidoIdVta(idVenta);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
