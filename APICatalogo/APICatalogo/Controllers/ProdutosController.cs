using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository_Pattern;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPrecos()
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorPreco();
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
        {
            try
            {
                var Produtos = await _uof.ProdutoRepository.Get().ToListAsync();

                if (Produtos is null)
                    return NotFound("Produtos não encontrado");

                var produtosDto = _mapper.Map<List<ProdutoDTO>>(Produtos);    

                return Ok(produtosDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpGet("{id:int}")]
        [ActionName("GetById")]
        public async Task<ActionResult<ProdutoDTO>> GetById(int id)
        {
            try
            {
                var Produtos = await _uof.ProdutoRepository.GetById(x => x.ProdutoId == id);

                if (Produtos is null)
                    return NotFound($"Produto com id {id} não econtrado");

                var produtoDto = _mapper.Map<ProdutoDTO>(Produtos);

                return Ok(produtoDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> AddProduto(ProdutoDTO ProdutoDto)
        {
            try
            {
                if (ProdutoDto is null)
                    return BadRequest("Produto inválido");

                var produto = _mapper.Map<Produto>(ProdutoDto);
                produto.Estoque = 5;
                produto.DataCadastro = DateTime.Now;

                _uof.ProdutoRepository.Add(produto);
                await _uof.Commit();

                var produtoDto = _mapper.Map<ProdutoDTO>(produto);

                return CreatedAtAction(nameof(GetById), new { id = produtoDto.ProdutoId }, produtoDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> UpdateProduto(int id, ProdutoDTO ProdutoDto)
        {
            try
            {
                if (id != ProdutoDto.ProdutoId)
                    return BadRequest($"id {id} inválido");

                var ExisteProduto = await _uof.ProdutoRepository.GetById(x => x.ProdutoId == id);
                if (ExisteProduto is null)
                    return NotFound("Produto não encontrado");

                ExisteProduto.Nome = ProdutoDto.Nome;
                ExisteProduto.Descricao = ProdutoDto.Descricao;
                ExisteProduto.Preco = ProdutoDto.Preco;
                ExisteProduto.ImagemUrl = ProdutoDto.ImagemUrl;
                ExisteProduto.CategoriaId = ProdutoDto.CategoriaId;

                _uof.ProdutoRepository.Update(ExisteProduto);
                await _uof.Commit();

                return Ok(ProdutoDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> DeleteProduto(int id)
        {
            try
            {
                var ExisteProduto = await _uof.ProdutoRepository.GetById(x => x.ProdutoId == id);
                if (ExisteProduto is null)
                    return NotFound($"Produto com id {id} não encontrado.");

                _uof.ProdutoRepository.Delete(ExisteProduto);
                await _uof.Commit();

                var produto = _mapper.Map<ProdutoDTO>(ExisteProduto);
                
                return Ok(produto); //return Ok("Produto Deletado");

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Ocorreu um erro ao validar sua solicitação");
            }
            

        }

    }
}
