using Domain.Entities;

namespace Application.Interfaces
{
    public interface IFuncionesCommand
    {
        Funciones RegistrarFuncion(int peliculaId, int salaId, DateTime fecha, TimeSpan horario);
        //Funciones DeleteFuncion(int funcionId);
    }
}
