using APICatalogo.Models;
using APICatalogo.Repository_Pattern;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        public ProdutosController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet("preco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPrecos()
        {
            return _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var Produtos = _uof.ProdutoRepository.Get().ToList();

                if (Produtos is null)
                    return NotFound("Produtos não encontrado");

                return Ok(Produtos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpGet("{id:int}")]
        [ActionName("GetById")]
        public IActionResult GetById(int id)
        {
            try
            {
                var Produtos = _uof.ProdutoRepository.GetById(x => x.ProdutoId == id);

                if (Produtos is null)
                    return NotFound($"Produto com id {id} não econtrado");

                return Ok(Produtos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpPost]
        public IActionResult AddProduto(Produto produto)
        {
            try
            {
                if (produto is null)
                    return BadRequest("Produto inválido");

                _uof.ProdutoRepository.Add(produto);
                _uof.Commit();

                return CreatedAtAction(nameof(GetById), new { id = produto.ProdutoId }, produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateProduto(int id, Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                    return BadRequest($"id {id} inválido");

                var ExisteProduto = _uof.ProdutoRepository.GetById(x => x.ProdutoId == id);
                if (ExisteProduto is null)
                    return NotFound("Produto não encontrado");

                ExisteProduto.Nome = produto.Nome;
                ExisteProduto.Descricao = produto.Descricao;
                ExisteProduto.Preco = produto.Preco;
                ExisteProduto.ImagemUrl = produto.ImagemUrl;
                ExisteProduto.Estoque = produto.Estoque;
                ExisteProduto.DataCadastro = produto.DataCadastro;
                ExisteProduto.CategoriaId = produto.CategoriaId;

                _uof.ProdutoRepository.Update(ExisteProduto);
                _uof.Commit();

                return Ok(ExisteProduto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteProduto(int id)
        {
            try
            {
                var ExisteProduto = _uof.ProdutoRepository.GetById(x => x.ProdutoId == id);
                if (ExisteProduto is null)
                    return NotFound($"Produto com id {id} não encontrado.");

                _uof.ProdutoRepository.Delete(ExisteProduto);
                _uof.Commit();

                return Ok("Produto Deletado");

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Ocorreu um erro ao validar sua solicitação");
            }
            

        }

    }
}
