using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Context;

public class PeliculasQuery : IPeliculasQuery
{
    private readonly CineContext _context;

    public PeliculasQuery(CineContext context)
    {
        _context = context;
    }
    public IEnumerable<Peliculas> GetAll()
    {
        return _context.Peliculas;
    }

    public Peliculas GetById(int PeliculaId)
    {
        return _context.Peliculas.FirstOrDefault(p => p.PeliculaId == PeliculaId);
    }

    public Peliculas GetByTitulo(string Titulo)
    {
        return _context.Peliculas.FirstOrDefault(p => p.Titulo == Titulo);
    }
}
