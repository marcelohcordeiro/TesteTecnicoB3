using B3.Domain.Models;

namespace B3.Domain.Interfaces
{
    public interface ITituloRepository
    {

        Task<List<Titulo>> GetTitulos();
        Task<List<Titulo>> GetTitulosRendaFixa();
        Task<Titulo> GetTituloById(Guid id);


    }
}
