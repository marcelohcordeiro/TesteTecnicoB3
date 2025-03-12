using B3.Domain.Models;
using Microsoft.EntityFrameworkCore;
using B3.Infra.Data.Context;
using B3.Domain.Interfaces;

namespace B3.Infra.Data.Repositories
{
    public class DescontoImpostoRendaRepository:IDescontoImpostoRendaRepository
    {
        private readonly AppDbContext _context;
        public DescontoImpostoRendaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DescontoImpostoRenda> GetDescontoImpostoRendaByMonths(int qtdeMesesInvestimento)
        {
            return await _context.descontoImpostoRendas!.Where(x => x.QtdeMesesInicio <= qtdeMesesInvestimento && (x.QtdeMesesFim == null ? qtdeMesesInvestimento : x.QtdeMesesFim) >= qtdeMesesInvestimento).FirstAsync();
        }

        public async Task<List<DescontoImpostoRenda>> GetDescontoImpostoRendas()
        {
            var x = await _context.descontoImpostoRendas!.ToListAsync();
            return x;
        }
    }
}
