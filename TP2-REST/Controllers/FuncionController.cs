using Application.Exceptions;
using Application.Interfaces;
using Application.Request;
using Application.Response;
using Microsoft.AspNetCore.Mvc;
using BadRequest = Application.Response.BadRequest;

namespace TP2_REST.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FuncionController : ControllerBase
    {
        private readonly IFuncionesService _funcionesService;
        private readonly ITicketsService _ticketsService;

        public FuncionController(IFuncionesService funcionesService, ITicketsService ticketsService)
        {
            _funcionesService = funcionesService;
            _ticketsService = ticketsService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<FuncionGetResponse>), 200)]
        [ProducesResponseType(typeof(BadRequest), 400)]
        public async Task<IActionResult> GetPeliculaByfilter(string? fecha, string? titulo, int? genero)
        {
            try
            {
                var result = await _funcionesService.GetFuncionesByTituloFechaOGenero(titulo, fecha, genero ?? 0);
                return Ok(result);
            }
            catch (SyntaxErrorException ex)
            {
                return new JsonResult(new BadRequest { Message = ex.Message }) { StatusCode = 400 };
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(FuncionResponse), 201)]
        [ProducesResponseType(typeof(BadRequest), 400)]
        [ProducesResponseType(typeof(BadRequest), 400)]
        [ProducesResponseType(typeof(BadRequest), 409)]
        public async Task<IActionResult> RegisterFuncion(FuncionRequest request)
        {
            try
            {
                var result = await _funcionesService.RegisterFuncion(request);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (SyntaxErrorException ex)
            {
                return new JsonResult(new BadRequest { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (ConflictException ex)
            {
                return new JsonResult(new BadRequest { Message = ex.Message }) { StatusCode = 409 };
            }
            catch (InvalidOperationException ex)
            {
                return new JsonResult(new BadRequest { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (PeliculaNotFoundException ex)
            {
                return new JsonResult(new BadRequest { Message = ex.Message }) { StatusCode = 404 };
            }
            catch (SalaNotFoundException ex)
            {
                return new JsonResult(new BadRequest { Message = ex.Message }) { StatusCode = 404 };
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error interno del servidor.");
            }

        }


        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(FuncionResponse), 200)]
        [ProducesResponseType(typeof(BadRequest), 400)]
        [ProducesResponseType(typeof(BadRequest), 404)]
        public async Task<IActionResult> GetFuncionById(int Id)
        {
            try
            {
                var result = await _funcionesService.GetFuncionResponseById(Id);
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

        [HttpDelete("{Id}")]
        [ProducesResponseType(typeof(FuncionDelete), 200)]
        [ProducesResponseType(typeof(BadRequest), 400)]
        [ProducesResponseType(typeof(BadRequest), 404)]
        [ProducesResponseType(typeof(BadRequest), 409)]
        public async Task<IActionResult> DeleteFuncion(int Id)
        {
            try
            {
                var result = await _funcionesService.DeleteFuncion(Id);
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
                return new JsonResult(new BadRequest { Message = ex.Message }) { StatusCode = 409 };
            }
        }


        /// <summary>
        /// Permite visualizar la cantidad de tickets disponibles para una función.
        /// </summary>
        /// <param name="Id">El identificador de la función</param>
        /// <returns>Una lista de tickets disponibles</returns>
        [HttpGet("{Id}/tickets")]
        [ProducesResponseType(typeof(CantidadTicketsResponse), 200)]
        [ProducesResponseType(typeof(BadRequest), 400)]
        [ProducesResponseType(typeof(BadRequest), 404)]
        public async Task<IActionResult> GetTicketById(int Id)
        {
            try
            {
                var result = await _funcionesService.GetCantidadTickets(Id);
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

        [HttpPost("{Id}/tickets")]
        [ProducesResponseType(typeof(TicketResponse), 201)]
        [ProducesResponseType(typeof(BadRequest), 400)]
        [ProducesResponseType(typeof(BadRequest), 404)]
        [ProducesResponseType(typeof(BadRequest), 409)]
        public async Task<IActionResult> RegisterTicket(int Id, TicketsRequest request)
        {
            try
            {
                var result = await _ticketsService.createTicket(Id, request);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (SyntaxErrorException ex)
            {
                return new JsonResult(new BadRequest { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (BadRequestException ex)
            {
                return new JsonResult(new BadRequest { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (NotFoundException ex)
            {
                return new JsonResult(new BadRequest { Message = ex.Message }) { StatusCode = 404 };
            }
            catch (ConflictException ex)
            {
                return new JsonResult(new BadRequest { Message = ex.Message }) { StatusCode = 409 };
            }
        }
    }
}