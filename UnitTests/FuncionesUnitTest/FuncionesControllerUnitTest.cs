using FluentAssertions;
using Moq;
using Application.Interfaces;
using Application.IMappers;
using Application.UseCases;
using Application.Request;
using Application.Exceptions;
using Application.Response;
using Domain.Entities;
using Xunit;

namespace UnitTests.FuncionesUnitTest
{
    public class FuncionesControllerUnitTest
    {
        // PELICULA EXISTENTE
        [Fact]
        public async Task RegisterFuncion_WithExistingMovie_ShouldRegisterSuccessfully()
        {
            // Arrange
            var peliculasServiceMock = new Mock<IPeliculasService>();
            peliculasServiceMock.Setup(p => p.PeliculaExists(It.IsAny<int>())).ReturnsAsync(true);

            var salasServiceMock = new Mock<ISalasService>();
            salasServiceMock.Setup(s => s.SalaExists(It.IsAny<int>())).ReturnsAsync(true);

            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            funcionesQueryMock.Setup(f => f.VerifyIfSalaisEmpty(It.IsAny<DateTime>(), It.IsAny<TimeSpan>(), It.IsAny<int>())).ReturnsAsync(true);
            funcionesQueryMock.Setup(f => f.GetFunciones()).ReturnsAsync(new List<Funciones>());

            var funcionesCommandMock = new Mock<IFuncionesCommand>();
            funcionesCommandMock.Setup(f => f.InsertFuncion(It.IsAny<Funciones>())).ReturnsAsync(1); // Simular ID de función insertada

            var mapperMock = new Mock<IFuncionMapper>();
            mapperMock.Setup(m => m.GenerateFuncionResponse(It.IsAny<Funciones>())).ReturnsAsync(new FuncionResponse
            {
                FuncionId = 1,
                Pelicula = new PeliculaGetResponse
                {
                    PeliculaId = 1,
                    Titulo = "Una esposa de mentira",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/1.jpg",
                    Genero = new Genero
                    {
                        Id = 4,
                        Nombre = "Comedia"
                    }
                },
                Sala = new Sala
                {
                    Id = 1,
                    Nombre = "2D",
                    Capacidad = 5
                },
                Fecha = DateTime.Parse("2024-12-12"),
                Horario = "20:00"
            });


            var funcionesService = new FuncionesService(funcionesCommandMock.Object, funcionesQueryMock.Object, peliculasServiceMock.Object, salasServiceMock.Object, mapperMock.Object, null);

            var request = new FuncionRequest
            {
                Pelicula = 1, // ID de película existente
                Sala = 1, // ID de sala existente
                Fecha = "2024-12-12",
                Horario = "20:00"
            };

            // Act
            var result = await funcionesService.RegisterFuncion(request);

            // Assert
            result.Should().NotBeNull();
        }

        // PELICULA INEXISTENTE
        [Fact]
        public async Task RegisterFuncion_WithNonExistingMovie_ShouldThrowPeliculaNotFoundException()
        {
            // Arrange
            var peliculasServiceMock = new Mock<IPeliculasService>();
            peliculasServiceMock.Setup(p => p.PeliculaExists(It.IsAny<int>())).ReturnsAsync(false);

            var funcionesService = new FuncionesService(null, null, peliculasServiceMock.Object, null, null, null);

            var request = new FuncionRequest
            {
                Pelicula = 99, // ID de película inexistente
                Sala = 1, // Este dato no se llega a validar debido a la inexistencia de la película
                Fecha = "2024-04-01",
                Horario = "20:00"
            };

            // Act & Assert
            await Assert.ThrowsAsync<PeliculaNotFoundException>(() => funcionesService.RegisterFuncion(request));
        }

        // SALA EXISTENTE
        [Fact]
        public async Task RegisterFuncion_WithExistingSala_ShouldProceed()
        {
            // Mock setup para una sala existente
            var salasServiceMock = new Mock<ISalasService>();
            salasServiceMock.Setup(s => s.SalaExists(It.IsAny<int>())).ReturnsAsync(true);

            var peliculasServiceMock = new Mock<IPeliculasService>();
            peliculasServiceMock.Setup(p => p.PeliculaExists(It.IsAny<int>())).ReturnsAsync(true);

            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            funcionesQueryMock.Setup(f => f.GetFunciones()).ReturnsAsync(new List<Funciones>());
            funcionesQueryMock.Setup(f => f.VerifyIfSalaisEmpty(It.IsAny<DateTime>(), It.IsAny<TimeSpan>(), It.IsAny<int>())).ReturnsAsync(true);

            var funcionesCommandMock = new Mock<IFuncionesCommand>();
            funcionesCommandMock.Setup(f => f.InsertFuncion(It.IsAny<Funciones>())).ReturnsAsync(1);

            var funcionMapperMock = new Mock<IFuncionMapper>();
            funcionMapperMock.Setup(m => m.GenerateFuncionResponse(It.IsAny<Funciones>())).ReturnsAsync(new FuncionResponse
            {
                FuncionId = 1, // ID específico para esta prueba
                Pelicula = new PeliculaGetResponse
                {
                    PeliculaId = 1,
                    Titulo = "Una esposa de mentira",
                    Poster = "https://ejemplo.com/poster2.jpg",
                    Genero = new Genero
                    {
                        Id = 4, // ID de género específico para esta prueba
                        Nombre = "Comedia" // Género específico para esta prueba
                    }
                },
                Sala = new Sala
                {
                    Id = 2, // ID de sala específico para esta prueba
                    Nombre = "3D",
                    Capacidad = 15 // Capacidad específica para esta prueba
                },
                Fecha = DateTime.Parse("2024-08-10"), // Fecha específica para esta prueba
                Horario = "21:00" // Horario específico para esta prueba
            });

            var ticketMapperMock = new Mock<ITicketMapper>();

            var funcionesService = new FuncionesService(
                funcionesCommandMock.Object,
                funcionesQueryMock.Object,
                peliculasServiceMock.Object,
                salasServiceMock.Object,
                funcionMapperMock.Object,
                ticketMapperMock.Object);

            var request = new FuncionRequest
            {
                Sala = 2, // Asegurando que este ID coincida con la configuración del mock
                Pelicula = 1, // ID de película para esta prueba
                Fecha = "2024-08-10",
                Horario = "21:00"
            };

            // Act
            var result = await funcionesService.RegisterFuncion(request);

            // Assert
            result.Should().NotBeNull();
            result.FuncionId.Should().Be(1);
            result.Pelicula.Titulo.Should().Be("Una esposa de mentira");
            result.Sala.Nombre.Should().Be("3D");
        }

        // SALA INEXISTENTE
        [Fact]
        public async Task RegisterFuncion_WithNonExistingSala_ShouldThrowSalaNotFoundException()
        {
            // Arrange
            var peliculasServiceMock = new Mock<IPeliculasService>();
            peliculasServiceMock.Setup(p => p.PeliculaExists(It.IsAny<int>())).ReturnsAsync(true);

            var salasServiceMock = new Mock<ISalasService>();
            salasServiceMock.Setup(s => s.SalaExists(It.IsAny<int>())).ReturnsAsync(false); // Configura para que retorne false, indicando que la sala no existe

            var funcionesService = new FuncionesService(null, null, peliculasServiceMock.Object, salasServiceMock.Object, null, null);

            var request = new FuncionRequest
            {
                Pelicula = 1,
                Sala = 99, // ID de sala inexistente
                Fecha = "2024-04-01",
                Horario = "20:00"
            };

            // Act & Assert
            await Assert.ThrowsAsync<SalaNotFoundException>(() => funcionesService.RegisterFuncion(request));
        }

        // FORMATO DE FECHA
        [Fact]
        public async Task RegisterFuncion_WithInvalidDateFormat_ShouldThrowSyntaxErrorException()
        {
            // Arrange
            var peliculasServiceMock = new Mock<IPeliculasService>();
            peliculasServiceMock.Setup(p => p.PeliculaExists(It.IsAny<int>())).ReturnsAsync(true);

            var salasServiceMock = new Mock<ISalasService>();
            salasServiceMock.Setup(s => s.SalaExists(It.IsAny<int>())).ReturnsAsync(true);

            var funcionesService = new FuncionesService(
                null,
                null,
                peliculasServiceMock.Object,
                salasServiceMock.Object,
                null,
                null);

            var request = new FuncionRequest
            {
                Pelicula = 1, // ID de película válida
                Sala = 1, // ID de sala válida
                Fecha = "formato de fecha incorrecto",
                Horario = "20:00"
            };

            // Act & Assert
            await Assert.ThrowsAsync<SyntaxErrorException>(() => funcionesService.RegisterFuncion(request));
        }

        // HORARIO CON FORMATO INVALIDO
        [Fact]
        public async Task RegisterFuncion_WithInvalidTimeFormat_ShouldThrowSyntaxErrorException()
        {
            // Arrange
            var peliculasServiceMock = new Mock<IPeliculasService>();
            peliculasServiceMock.Setup(p => p.PeliculaExists(It.IsAny<int>())).ReturnsAsync(true);

            var salasServiceMock = new Mock<ISalasService>();
            salasServiceMock.Setup(s => s.SalaExists(It.IsAny<int>())).ReturnsAsync(true);

            var funcionesService = new FuncionesService(
                null,
                null,
                peliculasServiceMock.Object,
                salasServiceMock.Object,
                null,
                null);

            var request = new FuncionRequest
            {
                Pelicula = 1, // ID de película válida
                Sala = 1, // ID de sala válida
                Fecha = "2024-12-12",
                Horario = "25:00" // Horario mal formado o fuera de rango
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SyntaxErrorException>(() => funcionesService.RegisterFuncion(request));
            exception.Message.Should().Be("Formato érroneo para el horario, ingrese horario desde las 00:00 a 23:59hs");
        }

        // SALA YA OCUPADA
        [Fact]
        public async Task RegisterFuncion_WhenSalaIsAlreadyBooked_ShouldThrowConflictException()
        {
            // Arrange
            var peliculasServiceMock = new Mock<IPeliculasService>();
            peliculasServiceMock.Setup(p => p.PeliculaExists(It.IsAny<int>())).ReturnsAsync(true);

            var salasServiceMock = new Mock<ISalasService>();
            salasServiceMock.Setup(s => s.SalaExists(It.IsAny<int>())).ReturnsAsync(true);

            var funcionesQueryMock = new Mock<IFuncionesQuery>();

            funcionesQueryMock.Setup(f => f.GetFunciones()).ReturnsAsync(new List<Funciones> {
        new Funciones {
            SalaId = 1,
            Fecha = new DateTime(2024, 04, 30),
            Horario = new TimeSpan(15, 00, 00) // Este horario provocará la superposición
        }
    });
            funcionesQueryMock.Setup(f => f.VerifyIfSalaisEmpty(It.IsAny<DateTime>(), It.IsAny<TimeSpan>(), It.IsAny<int>()))
                              .ReturnsAsync(false); // Indica que hay superposición

            var funcionesService = new FuncionesService(
                new Mock<IFuncionesCommand>().Object,
                funcionesQueryMock.Object,
                peliculasServiceMock.Object,
                salasServiceMock.Object,
                new Mock<IFuncionMapper>().Object,
                new Mock<ITicketMapper>().Object);

            var request = new FuncionRequest
            {
                Pelicula = 1, // ID de película existente
                Sala = 1, // ID de sala existente
                Fecha = "2024-04-30", // Fecha de la nueva función
                Horario = "17:00" // Horario de la nueva función que se superpone
            };

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => funcionesService.RegisterFuncion(request));
        }

        // FECHA NO ANTERIOR AL DIA ACTUAL
        [Fact]
        public async Task RegisterFuncion_WithPastDate_ShouldThrowInvalidOperationException()
        {
            // Configuración inicial
            var funcionesCommandMock = new Mock<IFuncionesCommand>();
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var peliculasServiceMock = new Mock<IPeliculasService>();
            var salasServiceMock = new Mock<ISalasService>();
            var funcionMapperMock = new Mock<IFuncionMapper>();
            var ticketMapperMock = new Mock<ITicketMapper>();

            // Configurando los mocks necesarios
            peliculasServiceMock.Setup(p => p.PeliculaExists(It.IsAny<int>())).ReturnsAsync(true);
            salasServiceMock.Setup(s => s.SalaExists(It.IsAny<int>())).ReturnsAsync(true);
            funcionesQueryMock.Setup(q => q.GetFunciones()).ReturnsAsync(new List<Funciones>());

            var funcionesService = new FuncionesService(
                funcionesCommandMock.Object,
                funcionesQueryMock.Object,
                peliculasServiceMock.Object,
                salasServiceMock.Object,
                funcionMapperMock.Object,
                ticketMapperMock.Object);

            // Simulando una fecha pasada para el registro de la función
            var pastDate = DateTime.Today.AddDays(-1).ToString("2024-03-02");

            var request = new FuncionRequest
            {
                Pelicula = 1, // El ID exacto no es relevante para esta prueba
                Sala = 1, // El ID exacto no es relevante para esta prueba
                Fecha = pastDate,
                Horario = "17:00"
            };

            // Verificando que se lanza la InvalidOperationException
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await funcionesService.RegisterFuncion(request));
        }

        // DELETE FUNCION CON TICKETS COMPRADOS
        [Fact]
        public async Task DeleteFuncion_WithTicketsRegistered_ThrowsConflictException()
        {
            var funcionesCommandMock = new Mock<IFuncionesCommand>();
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            funcionesQueryMock.Setup(q => q.GetFuncionById(It.IsAny<int>()))
                              .ReturnsAsync(new Funciones { Tickets = new List<Tickets> { new Tickets() } });

            var funcionesService = new FuncionesService(
                funcionesCommandMock.Object,
                funcionesQueryMock.Object,
                new Mock<IPeliculasService>().Object,
                new Mock<ISalasService>().Object,
                new Mock<IFuncionMapper>().Object,
                new Mock<ITicketMapper>().Object);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => funcionesService.DeleteFuncion(1));
        }

        // DELETE DE FUNCION CON ID NEGATIVO
        [Fact]
        public async Task DeleteFuncion_WithNegativeId_ShouldThrowSyntaxErrorException()
        {
            // Configuración de mocks
            var funcionesCommandMock = new Mock<IFuncionesCommand>();
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var peliculasServiceMock = new Mock<IPeliculasService>();
            var salasServiceMock = new Mock<ISalasService>();
            var funcionMapperMock = new Mock<IFuncionMapper>();
            var ticketMapperMock = new Mock<ITicketMapper>();

            // Configura los mocks para comportarse de manera neutra, ya que el flujo debería fallar antes de que se utilicen
            funcionesQueryMock.Setup(q => q.GetFuncionById(It.IsAny<int>())).ReturnsAsync(new Funciones());
            peliculasServiceMock.Setup(p => p.PeliculaExists(It.IsAny<int>())).ReturnsAsync(true);
            salasServiceMock.Setup(s => s.SalaExists(It.IsAny<int>())).ReturnsAsync(true);
            // No es necesario configurar los mocks de command y mapper para este escenario específico

            var funcionesService = new FuncionesService(
                funcionesCommandMock.Object,
                funcionesQueryMock.Object,
                peliculasServiceMock.Object,
                salasServiceMock.Object,
                funcionMapperMock.Object,
                ticketMapperMock.Object);

            // Intenta eliminar una función con un ID negativo, lo cual debería lanzar SyntaxErrorException
            await Assert.ThrowsAsync<SyntaxErrorException>(() => funcionesService.DeleteFuncion(-1));
        }

        // DELETE DE FUNCION INEXISTENTE
        [Fact]
        public async Task DeleteFuncion_WithNonexistentId_ThrowsFuncionNotFoundException()
        {
            // Configuración de Mocks
            var funcionesCommandMock = new Mock<IFuncionesCommand>();
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            funcionesQueryMock.Setup(q => q.GetFuncionById(It.IsAny<int>()))
                              .ReturnsAsync((Funciones)null); // Simula que no se encontró la función

            var peliculasServiceMock = new Mock<IPeliculasService>();
            peliculasServiceMock.Setup(p => p.PeliculaExists(It.IsAny<int>())).ReturnsAsync(true);

            var salasServiceMock = new Mock<ISalasService>();
            salasServiceMock.Setup(s => s.SalaExists(It.IsAny<int>())).ReturnsAsync(true);

            var funcionMapperMock = new Mock<IFuncionMapper>();
            var ticketMapperMock = new Mock<ITicketMapper>();

            var funcionesService = new FuncionesService(
                funcionesCommandMock.Object,
                funcionesQueryMock.Object,
                peliculasServiceMock.Object,
                salasServiceMock.Object,
                funcionMapperMock.Object,
                ticketMapperMock.Object);

            // ID de la función que no existe
            int funcionId = 999;

            // Act & Assert: Intenta eliminar la función y verifica que se lanza la excepción esperada
            var exception = await Assert.ThrowsAsync<FuncionNotFoundException>(() => funcionesService.DeleteFuncion(funcionId));
        }

        // DELETE FUNCION NO ENCONTRADA CON MENSAJE PERSONALIZADO
        [Fact]
        public async Task DeleteFuncion_WhenFuncionNotFound_ThrowsFuncionNotFoundExceptionWithCustomMessage()
        {
            // Configuración de Mocks
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            // Simular que la función no se encuentra para provocar FuncionNotFoundException dentro de DeleteFuncion
            funcionesQueryMock.Setup(q => q.GetFuncionById(It.IsAny<int>())).ReturnsAsync((Funciones)null);

            var funcionesService = new FuncionesService(
                new Mock<IFuncionesCommand>().Object,
                funcionesQueryMock.Object,
                new Mock<IPeliculasService>().Object,
                new Mock<ISalasService>().Object,
                new Mock<IFuncionMapper>().Object,
                new Mock<ITicketMapper>().Object);

            // ID de la función que se intentará eliminar y se espera que no exista
            int funcionId = 999;

            // Act & Assert
            FuncionNotFoundException exception = await Assert.ThrowsAsync<FuncionNotFoundException>(() => funcionesService.DeleteFuncion(funcionId));

            // Verificar que el mensaje de la excepción contiene el mensaje personalizado esperado
            Assert.Contains("Error al remover la función: No existe ninguna funcion con ese Id.", exception.Message);
        }

        //DELETE FUNCION SIN TICKETS
        [Fact]
        public async Task DeleteFuncion_WithNoTickets_ShouldDeleteSuccessfully()
        {
            // Arrange
            var funcionesCommandMock = new Mock<IFuncionesCommand>();
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var peliculasServiceMock = new Mock<IPeliculasService>();
            var salasServiceMock = new Mock<ISalasService>();
            var funcionMapperMock = new Mock<IFuncionMapper>();
            var ticketMapperMock = new Mock<ITicketMapper>(); // Agrega este mock

            int funcionId = 1; // ID de la función a eliminar

            // Configura los mocks
            funcionesQueryMock.Setup(q => q.GetFuncionById(funcionId)).ReturnsAsync(new Funciones { FuncionId = funcionId, Tickets = new List<Tickets>() });
            funcionesCommandMock.Setup(c => c.DeleteFuncion(It.IsAny<Funciones>())).ReturnsAsync(new Funciones()); // Suponiendo que retorna la entidad eliminada
            funcionMapperMock.Setup(m => m.GenerateFuncionDelete(It.IsAny<Funciones>())).ReturnsAsync(new FuncionDelete());

            // Inicializa el servicio con los mocks
            var funcionesService = new FuncionesService(
                funcionesCommandMock.Object,
                funcionesQueryMock.Object,
                peliculasServiceMock.Object,
                salasServiceMock.Object,
                funcionMapperMock.Object,
                ticketMapperMock.Object
            );

            // Act
            var result = await funcionesService.DeleteFuncion(funcionId);

            // Assert
            Assert.NotNull(result);
        }

        //PARAMETROS NULOS O CERO
        [Fact]
        public async Task GetFuncionesByTituloFechaOGenero_AllParametersNull_ReturnsAllFunciones()
        {
            // Arrange
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var funcionMapperMock = new Mock<IFuncionMapper>();

            funcionesQueryMock.Setup(q => q.GetFunciones()).ReturnsAsync(new List<Funciones>());
            funcionMapperMock.Setup(m => m.GenerateListFuncionGetResponse(It.IsAny<List<Funciones>>()))
                             .ReturnsAsync(new List<FuncionGetResponse>());

            var funcionesService = new FuncionesService(
                new Mock<IFuncionesCommand>().Object,
                funcionesQueryMock.Object,
                new Mock<IPeliculasService>().Object,
                new Mock<ISalasService>().Object,
                funcionMapperMock.Object,
                new Mock<ITicketMapper>().Object
            );

            // Act
            var result = await funcionesService.GetFuncionesByTituloFechaOGenero(null, null, 0);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        // FECHA NO ES NULA
        [Fact]
        public async Task GetFuncionesByTituloFechaOGenero_ValidDate_EntersIfCondition()
        {
            // Arrange
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var funcionMapperMock = new Mock<IFuncionMapper>();

            var validDate = "2024-01-01";
            funcionesQueryMock.Setup(q => q.GetFuncionesByFecha(It.IsAny<DateTime>()))
                              .ReturnsAsync(new List<Funciones>());
            funcionMapperMock.Setup(m => m.GenerateListFuncionGetResponse(It.IsAny<List<Funciones>>()))
                             .ReturnsAsync(new List<FuncionGetResponse>());

            var funcionesService = new FuncionesService(
                new Mock<IFuncionesCommand>().Object,
                funcionesQueryMock.Object,
                new Mock<IPeliculasService>().Object,
                new Mock<ISalasService>().Object,
                funcionMapperMock.Object,
                new Mock<ITicketMapper>().Object
            );

            // Act
            var result = await funcionesService.GetFuncionesByTituloFechaOGenero(null, validDate, 0);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        //SyntaxErrorException por una fecha mal formateada
        [Fact]
        public async Task GetFuncionesByTituloFechaOGenero_InvalidDateFormat_ThrowsSyntaxErrorException()
        {
            // Arrange
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var funcionMapperMock = new Mock<IFuncionMapper>();

            var funcionesService = new FuncionesService(
                new Mock<IFuncionesCommand>().Object,
                funcionesQueryMock.Object,
                new Mock<IPeliculasService>().Object,
                new Mock<ISalasService>().Object,
                funcionMapperMock.Object,
                new Mock<ITicketMapper>().Object);

            var invalidDate = "invalid-date-format";

            // Act & Assert
            await Assert.ThrowsAsync<SyntaxErrorException>(() =>
                funcionesService.GetFuncionesByTituloFechaOGenero(null, invalidDate, 0));
        }

        // TITULO NO ES NULO
        [Fact]
        public async Task GetFuncionesByTitulo_EntersIfCondition_ReturnsFilteredFunciones()
        {
            // Arrange
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var funcionMapperMock = new Mock<IFuncionMapper>();

            var mockTitulo = "Una esposa de mentira";
            funcionesQueryMock.Setup(q => q.GetFuncionesByTitulo(It.IsAny<string>()))
                              .ReturnsAsync(new List<Funciones>());
            funcionMapperMock.Setup(m => m.GenerateListFuncionGetResponse(It.IsAny<List<Funciones>>()))
                             .ReturnsAsync(new List<FuncionGetResponse>());

            var funcionesService = new FuncionesService(
                new Mock<IFuncionesCommand>().Object,
                funcionesQueryMock.Object,
                new Mock<IPeliculasService>().Object,
                new Mock<ISalasService>().Object,
                funcionMapperMock.Object,
                new Mock<ITicketMapper>().Object
            );

            // Act
            var result = await funcionesService.GetFuncionesByTituloFechaOGenero(mockTitulo, null, 0);

            // Assert
            Assert.NotNull(result);
        }

        // GENERO NO ES NULO NI CERO
        [Fact]
        public async Task GetFuncionesByGenero_EntersIfCondition_ReturnsFilteredFunciones()
        {
            // Arrange
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var funcionMapperMock = new Mock<IFuncionMapper>();

            var mockGeneroId = 1; // Asumir que es un ID de género válido
            funcionesQueryMock.Setup(q => q.GetFuncionByGenero(It.IsAny<int>()))
                              .ReturnsAsync(new List<Funciones>());
            funcionMapperMock.Setup(m => m.GenerateListFuncionGetResponse(It.IsAny<List<Funciones>>()))
                             .ReturnsAsync(new List<FuncionGetResponse>());

            var funcionesService = new FuncionesService(
                new Mock<IFuncionesCommand>().Object,
                funcionesQueryMock.Object,
                new Mock<IPeliculasService>().Object,
                new Mock<ISalasService>().Object,
                funcionMapperMock.Object,
                new Mock<ITicketMapper>().Object
            );

            // Act
            var result = await funcionesService.GetFuncionesByTituloFechaOGenero(null, null, mockGeneroId);

            // Assert
            Assert.NotNull(result);
        }

        // FECHA VALIDA Y TITULO NO ES NULO
        [Fact]
        public async Task GetFuncionesByTituloFechaOGenero_WhenFechaNotNullAndListIsEmptyAndTituloNotEmpty_ReturnsEmptyList()
        {
            // Arrange
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var funcionMapperMock = new Mock<IFuncionMapper>();

            var validDate = "2024-04-02";
            DateTime parsedDate;
            DateTime.TryParse(validDate, out parsedDate); // Simular una fecha válida parseada correctamente

            // Simular que no se encuentran funciones para la fecha dada
            funcionesQueryMock.Setup(q => q.GetFuncionesByFecha(parsedDate))
                              .ReturnsAsync(new List<Funciones>());

            // Configurar mock para simular que tampoco se encuentran funciones para el título dado
            funcionesQueryMock.Setup(q => q.GetFuncionesByTitulo("Una esposa de mentira"))
                              .ReturnsAsync(new List<Funciones>());

            // Simular respuesta vacía para cualquier lista de funciones proporcionada
            funcionMapperMock.Setup(m => m.GenerateListFuncionGetResponse(It.IsAny<List<Funciones>>()))
                             .ReturnsAsync(new List<FuncionGetResponse>());

            var funcionesService = new FuncionesService(
                new Mock<IFuncionesCommand>().Object,
                funcionesQueryMock.Object,
                new Mock<IPeliculasService>().Object,
                new Mock<ISalasService>().Object,
                funcionMapperMock.Object,
                new Mock<ITicketMapper>().Object);

            var mockTitulo = "Una esposa de mentira";

            // Act
            var result = await funcionesService.GetFuncionesByTituloFechaOGenero(mockTitulo, validDate, 0);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            funcionMapperMock.Verify(m => m.GenerateListFuncionGetResponse(It.IsAny<List<Funciones>>()), Times.Once);
        }

        // Prueba para un ID válido que devuelve una función
        [Fact]
        public async Task GetFuncionResponseById_ValidId_ReturnsFuncionResponse()
        {
            // Arrange
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var funcionMapperMock = new Mock<IFuncionMapper>();
            int validId = 1;

            // Definiendo un objeto Funciones con valores de ejemplo
            Funciones funcionExample = new Funciones
            {
                FuncionId = 1,
                PeliculaId = 1, // Asumiendo que esta propiedad se asignará internamente y no se usa directamente en el mapeo a FuncionResponse
                SalaId = 1, // Igualmente, asumiendo un uso interno
                Fecha = DateTime.Parse("2024-12-12"),
                Horario = TimeSpan.Parse("20:00")
            };

            // Definiendo un objeto FuncionResponse esperado
            FuncionResponse expectedResponse = new FuncionResponse
            {
                FuncionId = 1,
                Pelicula = new PeliculaGetResponse
                {
                    PeliculaId = 1,
                    Titulo = "Una esposa de mentira",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/1.jpg",
                    Genero = new Genero
                    {
                        Id = 4,
                        Nombre = "Comedia"
                    }
                },
                Sala = new Sala
                {
                    Id = 1,
                    Nombre = "2D",
                    Capacidad = 5
                },
                Fecha = DateTime.Parse("2024-12-12"),
                Horario = "20:00"
            };

            funcionesQueryMock.Setup(m => m.GetFuncionById(validId)).ReturnsAsync(funcionExample);
            funcionMapperMock.Setup(m => m.GenerateFuncionResponse(funcionExample)).ReturnsAsync(expectedResponse);

            var service = new FuncionesService(null, funcionesQueryMock.Object, null, null, funcionMapperMock.Object, null);

            // Act
            var result = await service.GetFuncionResponseById(validId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.FuncionId, result.FuncionId);
            Assert.Equal(expectedResponse.Pelicula.Titulo, result.Pelicula.Titulo);
            Assert.Equal(expectedResponse.Sala.Nombre, result.Sala.Nombre);
            // Asegurar que todas las propiedades de FuncionResponse coincidan con las esperadas
            Assert.Equal(expectedResponse.FuncionId, result.FuncionId);
            Assert.Equal(expectedResponse.Fecha, result.Fecha);
            Assert.Equal(expectedResponse.Horario, result.Horario);

            // Aserciones para la película dentro de FuncionResponse
            Assert.Equal(expectedResponse.Pelicula.PeliculaId, result.Pelicula.PeliculaId);
            Assert.Equal(expectedResponse.Pelicula.Titulo, result.Pelicula.Titulo);
            Assert.Equal(expectedResponse.Pelicula.Poster, result.Pelicula.Poster);
            Assert.Equal(expectedResponse.Pelicula.Genero.Id, result.Pelicula.Genero.Id);
            Assert.Equal(expectedResponse.Pelicula.Genero.Nombre, result.Pelicula.Genero.Nombre);

            // Aserciones para la sala dentro de FuncionResponse
            Assert.Equal(expectedResponse.Sala.Id, result.Sala.Id);
            Assert.Equal(expectedResponse.Sala.Nombre, result.Sala.Nombre);
            Assert.Equal(expectedResponse.Sala.Capacidad, result.Sala.Capacidad);
        }

        // Prueba para un ID que no existe y se espera una excepción
        [Fact]
        public async Task GetFuncionResponseById_InvalidId_ThrowsResourceNotFoundException()
        {
            // Arrange
            var mockQuery = new Mock<IFuncionesQuery>();
            int invalidId = 999;

            mockQuery.Setup(m => m.GetFuncionById(invalidId)).ReturnsAsync((Funciones)null);

            var service = new FuncionesService(null, mockQuery.Object, null, null, new Mock<IFuncionMapper>().Object, null);

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => service.GetFuncionResponseById(invalidId));
        }

        // ID Negativo
        [Fact]
        public async Task GetFuncionResponseById_NegativeId_ThrowsSyntaxErrorException()
        {
            // Arrange
            var mockFuncionMapper = new Mock<IFuncionMapper>();
            var mockFuncionesQuery = new Mock<IFuncionesQuery>();
            var mockPeliculasService = new Mock<IPeliculasService>();
            var mockSalasService = new Mock<ISalasService>();
            var mockFuncionesCommand = new Mock<IFuncionesCommand>();
            var mockTicketMapper = new Mock<ITicketMapper>();


            var funcionesService = new FuncionesService(
                mockFuncionesCommand.Object,
                mockFuncionesQuery.Object,
                mockPeliculasService.Object,
                mockSalasService.Object,
                mockFuncionMapper.Object,
                mockTicketMapper.Object);

            int negativeId = -1; // ID negativo para la prueba

            // Act & Assert
            await Assert.ThrowsAsync<SyntaxErrorException>(() => funcionesService.GetFuncionResponseById(negativeId));
        }

        // Prueba para Función Existente
        [Fact]
        public async Task GetFuncionById_WithValidId_ReturnsFuncion()
        {
            // Arrange
            var mockQuery = new Mock<IFuncionesQuery>();
            int validId = 1; // Asume un ID válido para el propósito de la prueba
            var expectedFuncion = new Funciones { FuncionId = validId };
            mockQuery.Setup(q => q.GetFuncionById(validId)).ReturnsAsync(expectedFuncion);

            var service = new FuncionesService(null, mockQuery.Object, null, null, null, null);

            // Act
            var result = await service.GetFuncionById(validId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedFuncion.FuncionId, result.FuncionId);
        }

        // Prueba para Función No Existente
        [Fact]
        public async Task GetFuncionById_WithInvalidId_ThrowsResourceNotFoundException()
        {
            // Arrange
            var mockQuery = new Mock<IFuncionesQuery>();
            int invalidId = 999; // Asume un ID no válido/inexistente
            mockQuery.Setup(q => q.GetFuncionById(invalidId)).ReturnsAsync((Funciones)null);

            var service = new FuncionesService(null, mockQuery.Object, null, null, null, null);

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => service.GetFuncionById(invalidId));
        }

        // GetCantidadTickets: ID de función no válido
        [Fact]
        public async Task GetCantidadTickets_WithInvalidId_ThrowsSyntaxErrorException()
        {
            // Arrange
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var ticketMapperMock = new Mock<ITicketMapper>();

            // El servicio FuncionesService se inicializa con mocks, donde es relevante.
            var funcionesService = new FuncionesService(
                null, // IFuncionesCommand no se utiliza en este método
                funcionesQueryMock.Object,
                null, // IPeliculasService no se utiliza en este método
                null, // ISalasService no se utiliza en este método
                null, // IFuncionMapper no se utiliza en este método
                ticketMapperMock.Object
            );

            int invalidFuncionId = -1; // Un ID inválido para la prueba

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SyntaxErrorException>(
                () => funcionesService.GetCantidadTickets(invalidFuncionId)
            );

            // Verificar el mensaje de la excepción
            Assert.Equal("Formato erróneo para el Id, pruebe con un entero.", exception.Message);
        }

        // Funcion no encontrada
        [Fact]
        public async Task GetCantidadTickets_WithNonExistingId_ThrowsFuncionNotFoundException()
        {
            // Arrange
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var ticketMapperMock = new Mock<ITicketMapper>();
            var funcionesService = new FuncionesService(
                null,
                funcionesQueryMock.Object,
                null,
                null,
                null,
                ticketMapperMock.Object
            );

            int nonExistingFuncionId = 999; // Un ID de función que no existe

            funcionesQueryMock.Setup(x => x.GetFuncionById(nonExistingFuncionId)).ReturnsAsync((Funciones)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FuncionNotFoundException>(() => funcionesService.GetCantidadTickets(nonExistingFuncionId));

            // Verificar el mensaje de la excepción
            Assert.Equal("No se encontró ninguna función con ese Id.", exception.Message);
        }

        // Funcion encontrada
        [Fact]
        public async Task GetCantidadTickets_WithValidId_ReturnsCantidadTicketsResponse()
        {
            // Arrange
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var ticketMapperMock = new Mock<ITicketMapper>();
            var funcionesService = new FuncionesService(
                null,
                funcionesQueryMock.Object,
                null,
                null,
                null,
                ticketMapperMock.Object
            );

            int validFuncionId = 1; // Un ID de función válido para la prueba

            var expectedFuncion = new Funciones
            {
                // Completa las propiedades de Funciones aquí
            };
            expectedFuncion.Sala = new Salas { Capacidad = 50 }; // Ejemplo de capacidad
            expectedFuncion.Tickets = new List<Tickets>(); // Ejemplo de tickets

            var expectedResponse = new CantidadTicketsResponse
            {
                Cantidad = expectedFuncion.Tickets.Count()
            };

            funcionesQueryMock.Setup(x => x.GetFuncionById(validFuncionId)).ReturnsAsync(expectedFuncion);
            ticketMapperMock.Setup(x => x.GetCantidadTicketResponse(expectedFuncion.Sala.Capacidad, expectedFuncion.Tickets.Count())).ReturnsAsync(expectedResponse);

            // Act
            var result = await funcionesService.GetCantidadTickets(validFuncionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.Cantidad, result.Cantidad);
        }

        // No hay superposición cuando la función existente termina antes de que comience la nueva función
        [Fact]
        public async Task VerifyIfSalaisEmpty_NoOverlapWithEarlierFunction_ReturnsTrue()
        {
            // Arrange
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var funcionesService = new FuncionesService(null, funcionesQueryMock.Object, null, null, null, null);

            var fecha = new DateTime(2024, 4, 20);
            var horario = new TimeSpan(18, 0, 0); // 6 PM
            var salaId = 1;
            var earlierFuncion = new Funciones
            {
                SalaId = salaId,
                Fecha = fecha,
                Horario = new TimeSpan(15, 0, 0) // 3 PM
            };

            funcionesQueryMock.Setup(f => f.GetFunciones()).ReturnsAsync(new List<Funciones> { earlierFuncion });

            // Act
            var result = await funcionesService.VerifyIfSalaisEmpty(fecha, horario, salaId);

            // Assert
            Assert.True(result);
        }

        // Hay superposición cuando la función existente comienza durante la nueva función
        [Fact]
        public async Task VerifyIfSalaisEmpty_OverlapDuringNewFunction_ReturnsFalse()
        {
            // Arrange
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var funcionesService = new FuncionesService(null, funcionesQueryMock.Object, null, null, null, null);

            var fecha = new DateTime(2024, 4, 20);
            var horario = new TimeSpan(18, 0, 0); // 6 PM
            var salaId = 1;
            var duringFuncion = new Funciones
            {
                SalaId = salaId,
                Fecha = fecha,
                Horario = horario.Add(TimeSpan.FromMinutes(30)) // Empieza a las 6:30 PM
            };

            funcionesQueryMock.Setup(f => f.GetFunciones()).ReturnsAsync(new List<Funciones> { duringFuncion });

            // Act
            var result = await funcionesService.VerifyIfSalaisEmpty(fecha, horario, salaId);

            // Assert
            Assert.False(result);
        }

        // Superposicion
        [Fact]
        public async Task VerifyIfSalaisEmpty_WhenSameSalaIdAndFechaDate_ShouldEvaluateIfCondition()
        {
            // Arrange
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var funcionesService = new FuncionesService(null, funcionesQueryMock.Object, null, null, null, null);

            var fecha = new DateTime(2024, 4, 20);
            var horario = new TimeSpan(18, 0, 0);
            var salaId = 1;

            var existingFuncion = new Funciones
            {
                SalaId = salaId,
                Fecha = fecha.AddHours(-1), // La función existente ocurre 1 hora antes en la misma sala y fecha
                Horario = new TimeSpan(17, 0, 0)
            };

            funcionesQueryMock.Setup(f => f.GetFunciones()).ReturnsAsync(new List<Funciones> { existingFuncion });

            // Act
            var result = await funcionesService.VerifyIfSalaisEmpty(fecha, horario, salaId);

            // Assert
            Assert.True(result); // El resultado debería ser verdadero si no hay superposición
        }

        // Prueba para la misma fecha pero sala diferente
        [Fact]
        public async Task VerifyIfSalaisEmpty_SameDateDifferentSala_Continues()
        {
            // Arrange
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var fecha = new DateTime(2024, 4, 20);
            var horario = new TimeSpan(18, 0, 0); // 6 PM
            int salaId = 1; // Una de las 3 salas existentes

            var otherSalaFuncion = new Funciones
            {
                SalaId = 2, // Asegurarse de que esta salaId exista pero sea diferente a la que estamos probando
                Fecha = fecha,
                Horario = horario
            };

            funcionesQueryMock.Setup(f => f.GetFunciones()).ReturnsAsync(new List<Funciones> { otherSalaFuncion });

            var funcionesService = new FuncionesService(
                null,
                funcionesQueryMock.Object,
                null,
                null,
                null,
                null
            );

            // Act
            var result = await funcionesService.VerifyIfSalaisEmpty(fecha, horario, salaId);

            // Assert
            Assert.True(result); // El resultado debería ser verdadero ya que la sala está vacía en esa fecha y horario
        }

        //Prueba para la misma sala pero fecha diferente
        [Fact]
        public async Task VerifyIfSalaisEmpty_SameSalaDifferentDate_Continues()
        {
            // Arrange
            var funcionesQueryMock = new Mock<IFuncionesQuery>();
            var fecha = new DateTime(2024, 4, 21); // Un día después
            var horario = new TimeSpan(18, 0, 0); // 6 PM
            int salaId = 1; // La misma sala que estamos probando

            var differentDateFuncion = new Funciones
            {
                SalaId = salaId,
                Fecha = fecha.AddDays(-1), // Asegurarse de que esta fecha sea diferente a la que estamos probando
                Horario = horario
            };

            funcionesQueryMock.Setup(f => f.GetFunciones()).ReturnsAsync(new List<Funciones> { differentDateFuncion });

            var funcionesService = new FuncionesService(
                null,
                funcionesQueryMock.Object,
                null,
                null,
                null,
                null
            );

            // Act
            var result = await funcionesService.VerifyIfSalaisEmpty(fecha, horario, salaId);

            // Assert
            Assert.True(result); // El resultado debería ser verdadero ya que la sala está vacía en esa fecha y horario
        }
    }
}