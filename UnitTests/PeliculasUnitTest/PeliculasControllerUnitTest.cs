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

namespace UnitTests.PeliculasUnitTest
{
    public class PeliculasControllerUnitTest
    {
        // ID DE PELICULA INVALIDO
        [Fact]
        public async Task GetPeliculaById_WithInvalidId_ThrowsSyntaxErrorException()
        {
            // Arrange
            var peliculasQueryMock = new Mock<IPeliculasQuery>();
            var peliculaMapperMock = new Mock<IPeliculaMapper>();
            var peliculasService = new PeliculasService(peliculasQueryMock.Object, null, null, peliculaMapperMock.Object);

            int invalidPeliculaId = -1;

            // Act & Assert
            await Assert.ThrowsAsync<SyntaxErrorException>(() => peliculasService.GetPeliculaById(invalidPeliculaId));
        }

        // PELICULA NO ENCONTRADA
        [Fact]
        public async Task GetPeliculaById_WithNonExistingId_ThrowsPeliculaNotFoundException()
        {
            // Arrange
            var peliculasQueryMock = new Mock<IPeliculasQuery>();
            var peliculaMapperMock = new Mock<IPeliculaMapper>();
            var peliculasService = new PeliculasService(peliculasQueryMock.Object, null, null, peliculaMapperMock.Object);

            int nonExistingPeliculaId = 999;

            peliculasQueryMock.Setup(x => x.GetPeliculaById(nonExistingPeliculaId)).ReturnsAsync((Peliculas)null);

            // Act & Assert
            await Assert.ThrowsAsync<PeliculaNotFoundException>(() => peliculasService.GetPeliculaById(nonExistingPeliculaId));
        }

        // PELICULA ENCONTRADA
        [Fact]
        public async Task GetPeliculaById_WithValidId_ReturnsPeliculaResponse()
        {
            // Arrange
            var peliculasQueryMock = new Mock<IPeliculasQuery>();
            var peliculaMapperMock = new Mock<IPeliculaMapper>();
            var peliculasService = new PeliculasService(peliculasQueryMock.Object, null, null, peliculaMapperMock.Object);

            int validPeliculaId = 1;
            var expectedPelicula = new Peliculas
            {
                // Completa las propiedades de Peliculas aquí
            };
            var expectedResponse = new PeliculaResponse
            {
                // Completa las propiedades esperadas de PeliculaResponse aquí
            };

            peliculasQueryMock.Setup(x => x.GetPeliculaById(validPeliculaId)).ReturnsAsync(expectedPelicula);
            peliculaMapperMock.Setup(x => x.GeneratePeliculaResponse(expectedPelicula)).ReturnsAsync(expectedResponse);

            // Act
            var result = await peliculasService.GetPeliculaById(validPeliculaId);

            // Assert
            Assert.NotNull(result);
            // Agrega más aserciones para verificar los detalles específicos de la respuesta
        }

        // UPDATE PELICULA CON ID INVALIDO
        [Fact]
        public async Task UpdatePelicula_WithInvalidId_ThrowsSyntaxErrorException()
        {
            // Arrange
            var peliculasQueryMock = new Mock<IPeliculasQuery>();
            var peliculasCommandMock = new Mock<IPeliculasCommand>();
            var generosServiceMock = new Mock<IGenerosService>(); // Mock añadido para IGenerosService
            var peliculaMapperMock = new Mock<IPeliculaMapper>();

            var peliculasService = new PeliculasService(
                peliculasQueryMock.Object,
                peliculasCommandMock.Object,
                generosServiceMock.Object, // Añade el mock como argumento
                peliculaMapperMock.Object);

            int invalidPeliculaId = -1; // Un ID inválido para la prueba
            PeliculaRequest peliculaRequest = new PeliculaRequest
            {
                Titulo = "Titulo Test",
                Poster = "URL al poster",
                Trailer = "URL al trailer",
                Sinopsis = "Una sinopsis de prueba",
                Genero = 1
            };

            // Act & Assert
            await Assert.ThrowsAsync<SyntaxErrorException>(() => peliculasService.UpdatePelicula(invalidPeliculaId, peliculaRequest));
        }

        // UPDATE PELICULA CON ID INEXISTENTE
        [Fact]
        public async Task UpdatePelicula_WithNonExistentId_ThrowsNotFoundException()
        {
            // Arrange
            var peliculasQueryMock = new Mock<IPeliculasQuery>();
            peliculasQueryMock.Setup(p => p.GetPeliculaById(It.IsAny<int>())).ReturnsAsync((Peliculas)null); // Simula que no se encuentra la película

            var peliculasService = new PeliculasService(
                peliculasQueryMock.Object,
                new Mock<IPeliculasCommand>().Object,
                new Mock<IGenerosService>().Object,
                new Mock<IPeliculaMapper>().Object);

            int nonExistentPeliculaId = 999;
            PeliculaRequest peliculaRequest = new PeliculaRequest(); // Detalles de la solicitud omitidos para brevedad

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => peliculasService.UpdatePelicula(nonExistentPeliculaId, peliculaRequest));
        }

        // UPDATE PELICULA CON TITULO YA EXISTENTE
        [Fact]
        public async Task UpdatePelicula_WithExistingTitle_ThrowsConflictException()
        {
            // Arrange
            var peliculasQueryMock = new Mock<IPeliculasQuery>();
            var peliculasCommandMock = new Mock<IPeliculasCommand>();
            var generosServiceMock = new Mock<IGenerosService>(); // Cambiado a IGenerosService
            var peliculaMapperMock = new Mock<IPeliculaMapper>();

            // Configurar el mock para que devuelva true cuando se llama a VerifySameName
            peliculasQueryMock.Setup(p => p.VerifySameName(It.IsAny<string>(), It.IsAny<int>()))
                              .ReturnsAsync(true);

            // Configurar el mock para que devuelva una instancia de Peliculas al llamar a GetPeliculaById
            peliculasQueryMock.Setup(p => p.GetPeliculaById(It.IsAny<int>()))
                              .ReturnsAsync(new Peliculas());

            // Configurar el mock para que devuelva true al llamar a VerifyGenero
            generosServiceMock.Setup(p => p.VerifyGenero(It.IsAny<int>())) // Usar el mock de IGenerosService
                                .ReturnsAsync(true);

            var peliculasService = new PeliculasService(
                peliculasQueryMock.Object,
                peliculasCommandMock.Object,
                generosServiceMock.Object, // Pasar el mock de IGenerosService
                peliculaMapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() =>
                peliculasService.UpdatePelicula(1, new PeliculaRequest
                {
                    Titulo = "Una esposa de mentira",
                    Poster = "https://raw.githubusercontent.com/Maarlopez/imagenes/main/1.jpg",
                    Trailer = "https://www.youtube.com/embed/xuZnmYjAKww?si=B_1rowxqE-bt2Azm",
                    Sinopsis = "El cirujano Danny decide contratar a su ayudante Katherine, una madre soltera con hijos, para que finjan ser su familia.",
                    Genero = 4
                }));
        }
    }
}
