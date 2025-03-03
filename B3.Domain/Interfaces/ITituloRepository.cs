using B3.Domain.Models;

namespace B3.Domain.Interfaces
{
    public interface ITituloRepository
    {

        Task<List<Titulo>> GetTitulos();
        Task<Titulo> GetTituloById(Guid id);


    }
}
