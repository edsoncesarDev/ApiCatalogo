using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Models
{
    public class Categoria
    {
        public Categoria()
        {
            Produtos = new Collection<Produto>();
        }
        [Key]
        public int CategoriaId { get; set; }
        public string Nome { get; set; }
        public string ImagemUrl { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; }
    }
}
