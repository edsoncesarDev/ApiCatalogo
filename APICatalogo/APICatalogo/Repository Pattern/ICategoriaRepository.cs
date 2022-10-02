using APICatalogo.Models;
using APICatalogo.Repository;

namespace APICatalogo.Repository_Pattern
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<IEnumerable<Categoria>> GetCategoriasProdutos();
    }
}
