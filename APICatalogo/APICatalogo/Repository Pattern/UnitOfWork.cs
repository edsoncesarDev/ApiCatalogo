using APICatalogo.Context;

namespace APICatalogo.Repository_Pattern
{
    public class UnitOfWork : IUnitOfWork
    {
        private ProdutoRepository _produtoRepository;
        private  CategoriaRepository _categoriaRepository;
        public AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            { 
                return _produtoRepository = _produtoRepository ?? new ProdutoRepository(_context); 
            }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _categoriaRepository = _categoriaRepository ?? new CategoriaRepository(_context);
            }
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        //Enquanto não fazemos o Dispose do Contexto, ele mantem a referência de todas as entidades que foram carregadas.
        //quando mantemos o contexto aberto por muito tempo, podemos acabar trabalhando com dados que já foram modificados no banco de produção.
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
