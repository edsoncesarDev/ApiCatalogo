using APICatalogo.Models;
using APICatalogo.Repository;

namespace APICatalogo.Repository_Pattern
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorPreco();
    }
}
