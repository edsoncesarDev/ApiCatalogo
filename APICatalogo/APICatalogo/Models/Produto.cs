﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APICatalogo.Models
{
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public string ImagemUrl { get; set; }
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }
        public int CategoriaId { get; set; }
        [JsonIgnore]
        public virtual Categoria Categorias { get; set; }
    }
}
