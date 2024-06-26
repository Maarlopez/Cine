﻿using Domain.Entities;

namespace Application.Interfaces
{
    public interface IGenerosService
    {
        Task<List<Generos>> GetGeneros();
        Task<bool> VerifyGenero(int generoId);
    }
}