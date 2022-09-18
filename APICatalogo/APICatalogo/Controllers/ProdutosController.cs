using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var Produtos = await _context.Produtos.AsNoTracking().ToListAsync();

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
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var Produtos = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(x => x.ProdutoId == id);

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
        public async Task<IActionResult> AddProduto(Produto produto)
        {
            try
            {
                if (produto is null)
                    return BadRequest("Produto inválido");

                await _context.Produtos.AddAsync(produto);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = produto.ProdutoId }, produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduto(int id, Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                    return BadRequest($"id {id} inválido");

                var ExisteProduto = await _context.Produtos.FirstOrDefaultAsync(x => x.ProdutoId == id);
                if (ExisteProduto is null)
                    return NotFound("Produto não encontrado");

                ExisteProduto.Nome = produto.Nome;
                ExisteProduto.Descricao = produto.Descricao;
                ExisteProduto.Preco = produto.Preco;
                ExisteProduto.ImagemUrl = produto.ImagemUrl;
                ExisteProduto.Estoque = produto.Estoque;
                ExisteProduto.DataCadastro = produto.DataCadastro;
                ExisteProduto.CategoriaId = produto.CategoriaId;
                
                await _context.SaveChangesAsync();

                return Ok(ExisteProduto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            try
            {
                var ExisteProduto = await _context.Produtos.FirstOrDefaultAsync(x => x.ProdutoId == id);
                if (ExisteProduto is null)
                    return NotFound($"Produto com id {id} não encontrado.");

                _context.Remove(ExisteProduto);
                await _context.SaveChangesAsync();
                return Ok("Produto Deletado");

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Ocorreu um erro ao validar sua solicitação");
            }
            

        }

    }
}
