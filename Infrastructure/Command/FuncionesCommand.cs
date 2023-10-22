using System.Globalization;
using System.Text.RegularExpressions;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using BadRequest = Application.Exceptions.FuncionNotFoundException;


namespace Infrastructure.Command
{
    public class FuncionesCommand : IFuncionesCommand
    {
        private readonly CineContext _context;


        public FuncionesCommand(CineContext context)
        {
            _context = context;
        }

        public Funciones RegistrarFuncion(int peliculaId, int salaId, DateTime fecha, TimeSpan horario)
        {
            // 1. Validar el formato del horario
            if (!IsValidTimeFormat(horario))
            {
                throw new BadRequest("Formato de horario incorrecto.");
            }

            // 2. Validar el formato de la fecha
            if (!IsValidDateFormat(fecha))
            {
                throw new BadRequest("Formato de fecha incorrecto. Debe ser dd/MM.");
            }

            // 3. Verificar superposición con otras funciones
            var overlappingFunction = _context.Funciones
                .Where(f => f.SalaId == salaId && f.Fecha == fecha)
                .Where(f => (f.Horario <= horario && f.Horario.Add(new TimeSpan(2, 30, 0)) > horario) ||
                            (horario <= f.Horario && horario.Add(new TimeSpan(2, 30, 0)) > f.Horario))
                .FirstOrDefault();

            if (overlappingFunction != null)
            {
                throw new BadRequest("La función se superpone con otra función existente en la misma sala.");
            }

            // 4. Asegurarse de que no exista una película con el mismo título
            var pelicula = _context.Peliculas.Find(peliculaId);
            if (pelicula == null)
            {
                throw new BadRequest("Película no encontrada.");
            }

            // Convertir el título a Title Case (solo la primera letra)
            pelicula.Titulo = ConvertToTitleCase(pelicula.Titulo);

            var existingPelicula = _context.Peliculas.FirstOrDefault(p => p.Titulo == pelicula.Titulo);
            if (existingPelicula != null && existingPelicula.PeliculaId != peliculaId)
            {
                throw new BadRequest("Ya existe una película con el mismo título.");
            }

            // Si todo está bien, procede a guardar la nueva función
            var nuevaFuncion = new Funciones
            {
                PeliculaId = peliculaId,
                SalaId = salaId,
                Fecha = fecha,
                Horario = horario,
                Tickets = new List<Tickets>()
            };

            _context.Funciones.Add(nuevaFuncion);
            _context.SaveChanges();

            return nuevaFuncion;
        }

        private bool IsValidTimeFormat(TimeSpan horario)
        {
            // Convertir TimeSpan a string en formato hh:mm
            string horarioString = horario.ToString(@"hh\:mm");

            // Expresión regular para validar formato hh:mm
            Regex regex = new Regex(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$");
            return regex.IsMatch(horarioString);
        }

        private bool IsValidDateFormat(DateTime fecha)
        {
            // Convertir DateTime a string en formato dd/MM
            string fechaString = fecha.ToString("dd/MM");

            // Verificar si la fecha convertida coincide con la fecha original
            DateTime parsedDate;
            return DateTime.TryParseExact(fechaString, "dd/MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate) && fecha.Date == parsedDate;
        }


        private string ConvertToTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        //public Funciones DeleteFuncion(int funcionId)
        //{
        //    var funcion = _context.Funciones.Find(funcionId);
        //    if (funcionId == null)
        //        throw new BadRequest("Función no encontrada");

        //    _context.Funciones.Remove(funcion);
        //    _context.SaveChanges();

        //    return funcion;
        //}
    }
}