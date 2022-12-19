using Hernan_Fiorito_ProyectoFinal.Models;
using Hernan_Fiorito_ProyectoFinal.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Hernan_Fiorito_ProyectoFinal.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LoginController : Controller
    {
        LoginRepository repository = new LoginRepository();

        [HttpPost]
        public ActionResult<Usuario> Login(Usuario usuario)
        {
            try
            {
                
                bool usuarioExiste = repository.verificarUsuario(usuario);
                return usuarioExiste ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
