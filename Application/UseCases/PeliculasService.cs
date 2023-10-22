using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases
{
    public class PeliculasService : IPeliculasService
    {
        private readonly IPeliculasCommand _command;
        private readonly IPeliculasQuery _query;

        public PeliculasService(IPeliculasCommand command, IPeliculasQuery query)
        {
            _command = command;
            _query = query;
        }
        public IEnumerable<Peliculas> GetAll()
        {
            return _query.GetAll();
        }

        public Peliculas GetById(int peliculaId)
        {
            return _query.GetById(peliculaId);
        }

        public Peliculas GetByTitulo(string titulo)
        {
            return _query.GetByTitulo(titulo);
        }
        public Peliculas DeletePelicula(int peliculaId)
        {
            return _command.DeletePelicula(peliculaId);
        }

        public int GetMinId()
        {
            return _query.GetAll().Min(s => s.PeliculaId);
        }

        public int GetMaxId()
        {
            return _query.GetAll().Max(s => s.PeliculaId);
        }
    }
}
