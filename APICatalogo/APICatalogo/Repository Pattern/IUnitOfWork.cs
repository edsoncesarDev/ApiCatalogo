namespace APICatalogo.Repository_Pattern
{
    public interface IUnitOfWork
    {
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        Task Commit();
        void Dispose();
    }
}
