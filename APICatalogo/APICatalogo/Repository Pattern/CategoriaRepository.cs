using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repository_Pattern.Repository;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository_Pattern
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext contexto) : base(contexto) { }

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return Get().Include(x => x.Produtos);
        }
    }
}
