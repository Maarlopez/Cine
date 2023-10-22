using Application.Exceptions;
using Application.Interfaces;
using Application.Request;
using Application.Response;
using Microsoft.AspNetCore.Mvc;
using BadRequest = Application.Response.BadRequest;

namespace TP2_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionController : ControllerBase
    {
        private readonly IFuncionesService _funcionesService;

        public FuncionController(IFuncionesService funcionesService)
        {
            _funcionesService = funcionesService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(FuncionGetResponse), 200)]
        [ProducesResponseType(typeof(BadRequest), 400)]
        //[ProducesResponseType(typeof(BadRequest), 404)]
        public IActionResult GetFuncionesPorFechaTituloYGenero(DateTime fecha, string titulo, string genero)
        {

            try
            {
                var funciones = _funcionesService.GetFuncionesPorFechaTituloYGenero(fecha, titulo, genero);
                return Ok(funciones);
            }
            //catch (PeliculaNotFoundException ex)
            //{
            //    return NotFound(ex.Message);
            //}
            //catch (GeneroNotFoundException ex)
            //{
            //    return NotFound(new BadRequest
            //    {
            //        Message = ex.Message
            //    });
            //}
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FuncionResponse), 200)]
        [ProducesResponseType(typeof(BadRequest), 400)]
        [ProducesResponseType(typeof(BadRequest), 404)]
        public IActionResult GetById(int id)
        {

            try
            {
                var funciones = _funcionesService.GetById(id);
                return Ok(funciones);
            }
            catch (FuncionNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(FuncionResponse), 201)]
        [ProducesResponseType(typeof(BadRequest), 400)]
        public IActionResult CreateUsuario(FuncionRequest request)
        {
            FuncionResponse result = null;

            try
            {
                result = _funcionesService.RegistrarFuncion(request);
            }
            catch (ConflictException e)
            {
                return new JsonResult(new BadRequest { Message = e.Message }) { StatusCode = 409 };
            }
            catch (Exception)
            {
                return new JsonResult(new BadRequest { Message = "Puede que existan campos invalidos" }) { StatusCode = 400 };
            }
            return new JsonResult(result);
        }
    }
}