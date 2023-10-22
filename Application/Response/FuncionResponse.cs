﻿using Domain.DTO;

namespace Application.Response
{
    public class FuncionResponse
    {
        public int FuncionId { get; set; }
        public PeliculaGetResponse Pelicula { get; set; }
        public Sala Sala { get; set; }
        public DateTime Fecha { get; set; }
        public string? Horario { get; set; }
    }
}
