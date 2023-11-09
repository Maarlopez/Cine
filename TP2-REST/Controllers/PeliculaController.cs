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
    public class PeliculaController : ControllerBase
    {
        private readonly IPeliculasService _service;

        public PeliculaController(IPeliculasService service)
        {
            _service = service;
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(PeliculaResponse), 200)]
        [ProducesResponseType(typeof(BadRequest), 400)]
        [ProducesResponseType(typeof(BadRequest), 404)]
        public async Task<IActionResult> GetPeliculaById(int Id)
        {
            try
            {
                var result = await _service.GetPeliculaById(Id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (SyntaxErrorException ex)
            {
                return new JsonResult(new BadRequest { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (NotFoundException ex)
            {
                return new JsonResult(new BadRequest { Message = ex.Message }) { StatusCode = 404 };
            }

        }

        /// <summary>
        /// Permite actualizar la información de una película.
        /// </summary>
        /// <param name="Id">El identificador de la pelicula</param>
        /// <returns>Película actualizada</returns>
        [HttpPut("{Id}")]
        [ProducesResponseType(typeof(PeliculaResponse), 200)]
        [ProducesResponseType(typeof(BadRequest), 400)]
        [ProducesResponseType(typeof(BadRequest), 404)]
        [ProducesResponseType(typeof(BadRequest), 409)]
        public async Task<IActionResult> UpdatePeliculaById(int Id, PeliculaRequest request)
        {
            try
            {
                var result = await _service.UpdatePelicula(Id, request);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (SyntaxErrorException ex)
            {
                return new JsonResult(new BadRequest { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (NotFoundException ex)
            {
                return new JsonResult(new BadRequest { Message = ex.Message }) { StatusCode = 404 };
            }
            catch (ConflictException ex)
            {
                return new JsonResult(new BadRequest { Message = ex.Message }) { StatusCode = 400 };
            }
        }
    }
}