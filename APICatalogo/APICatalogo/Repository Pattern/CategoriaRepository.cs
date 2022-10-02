using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repository_Pattern.Repository;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository_Pattern
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext contexto) : base(contexto) { }

        public async Task<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return await Get().Include(x => x.Produtos).ToListAsync();
        }
    }
}
