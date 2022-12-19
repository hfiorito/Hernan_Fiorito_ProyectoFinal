using Hernan_Fiorito_ProyectoFinal.Models;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Hernan_Fiorito_ProyectoFinal.Repositories;

namespace Hernan_Fiorito_ProyectoFinal.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : Controller
    {

        private ProductoRepository repository = new ProductoRepository();
        //Crear producto
        [HttpPost]
        public ActionResult Post([FromBody] Producto producto)
        {
            try
            {
                Producto productoCreado = repository.crearProducto(producto);
                return StatusCode(StatusCodes.Status201Created, productoCreado);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        // Modificar Producto
        [HttpPut]
        public ActionResult<Producto> Put(long id, [FromBody] Producto prductoAActualizar)
        {
            try
            {
                Producto? productoActualizado = repository.modificarProductoDesdeId(id, prductoAActualizar);

                return Ok(productoActualizado);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // Eliminar producto
        [HttpDelete]

        public ActionResult Delete([FromBody] long id)
        {
            try
            {
                bool seElimino = repository.eliminarProducto(id);
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

        //Traer todos los productos cargados
        [HttpGet]//listo productos
        public ActionResult<List<Producto>> Get()
        {
            try
            {
                List<Producto> lista = repository.listarProductos();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
