using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Command
{
    public class FuncionesCommand : IFuncionesCommand
    {
        private readonly CineContext _context;
        public FuncionesCommand(CineContext context)
        {
            _context = context;
        }
        public async Task<int> InsertFuncion(Funciones funcion)
        {
            try
            {
                _context.Add(funcion);
                await _context.SaveChangesAsync();
                return funcion.FuncionId;
            }
            catch (DbUpdateException)
            {
                throw new ConflictException("Error en la base de datos: Problema al añadir la funcion.");
            }
        }

        public async Task<Funciones> DeleteFuncion(Funciones funcion)
        {
            try
            {
                _context.Remove(funcion);
                await _context.SaveChangesAsync();
                return funcion;
            }
            catch (DbUpdateException)
            {
                throw new ConflictException("Error en la base de datos: Problema al eliminar la funcion.");
            }
        }
    }
}