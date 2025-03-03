﻿using B3.Domain.Interfaces;
using B3.Infra.Data.Context;
using B3.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace B3.Infra.Data.Repositories
{
    public class TituloRepository : ITituloRepository
    {

        private readonly AppDbContext _context;

        public TituloRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Titulo> GetTituloById(Guid id)
        {
            return await _context.Titulos.Include(x => x.Indexador).Include(x => x.TipoTitulo).FirstAsync(x => x.IdTitulo == id);
        }

        public async Task<List<Titulo>> GetTitulos()
        {
            return await _context.Titulos.Include(x => x.Indexador).Include(x => x.TipoTitulo).ToListAsync();
        }
    }
}
