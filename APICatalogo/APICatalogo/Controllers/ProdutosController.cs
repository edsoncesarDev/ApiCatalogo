using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository_Pattern;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
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

        [HttpGet("preco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPrecos()
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDto;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            try
            {
                var Produtos = _uof.ProdutoRepository.Get().ToList();

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
        public ActionResult<ProdutoDTO> GetById(int id)
        {
            try
            {
                var Produtos = _uof.ProdutoRepository.GetById(x => x.ProdutoId == id);

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
        public IActionResult AddProduto(ProdutoDTO ProdutoDto)
        {
            try
            {
                if (ProdutoDto is null)
                    return BadRequest("Produto inválido");

                var produto = _mapper.Map<Produto>(ProdutoDto);
                produto.Estoque = 5;
                produto.DataCadastro = DateTime.Now;

                _uof.ProdutoRepository.Add(produto);
                _uof.Commit();

                var produtoDto = _mapper.Map<ProdutoDTO>(produto);

                return CreatedAtAction(nameof(GetById), new { id = produtoDto.ProdutoId }, produtoDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> UpdateProduto(int id, ProdutoDTO ProdutoDto)
        {
            try
            {
                if (id != ProdutoDto.ProdutoId)
                    return BadRequest($"id {id} inválido");

                var ExisteProduto = _uof.ProdutoRepository.GetById(x => x.ProdutoId == id);
                if (ExisteProduto is null)
                    return NotFound("Produto não encontrado");

                ExisteProduto.Nome = ProdutoDto.Nome;
                ExisteProduto.Descricao = ProdutoDto.Descricao;
                ExisteProduto.Preco = ProdutoDto.Preco;
                ExisteProduto.ImagemUrl = ProdutoDto.ImagemUrl;
                ExisteProduto.CategoriaId = ProdutoDto.CategoriaId;

                _uof.ProdutoRepository.Update(ExisteProduto);
                _uof.Commit();

                return Ok(ProdutoDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> DeleteProduto(int id)
        {
            try
            {
                var ExisteProduto = _uof.ProdutoRepository.GetById(x => x.ProdutoId == id);
                if (ExisteProduto is null)
                    return NotFound($"Produto com id {id} não encontrado.");

                _uof.ProdutoRepository.Delete(ExisteProduto);
                _uof.Commit();

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
