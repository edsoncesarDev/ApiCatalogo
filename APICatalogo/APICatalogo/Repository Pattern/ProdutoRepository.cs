using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repository_Pattern.Repository;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository_Pattern
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext contexto) : base(contexto) { }
        public async Task<IEnumerable<Produto>> GetProdutosPorPreco()
        {
            return await Get().OrderBy(c => c.Preco).ToListAsync();
        }
    }
}
