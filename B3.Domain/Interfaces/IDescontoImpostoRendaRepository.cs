using B3.Domain.Models;

namespace B3.Domain.Interfaces
{
    public interface IDescontoImpostoRendaRepository
    {
        Task<List<DescontoImpostoRenda>> GetDescontoImpostoRendas();
        Task<DescontoImpostoRenda> GetDescontoImpostoRendaByMonths(int QtdeMeses);

    }
}
