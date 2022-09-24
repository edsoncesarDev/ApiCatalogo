using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repository_Pattern;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        public CategoriasController(IUnitOfWork uof)
        {
            _uof = uof;
        }


        [HttpGet("produtos")]   //adicionando "produtos" a rota - api/[controller]/"produtos"
        public IActionResult GetCategoriasProdutos()
        {
            try
            {
                //include -> incluindo os produtos das categorias
                return Ok(_uof.CategoriaRepository.GetCategoriasProdutos().ToList());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }

        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_uof.CategoriaRepository.Get().ToList());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
           
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var Categoria = _uof.CategoriaRepository.GetById(x => x.CategoriaId == id);

                if (Categoria is null)
                    return NotFound($"Categoria com id {id} não encontrada");

                return Ok(Categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }


        [HttpPost]
        public IActionResult AddCategoria(Categoria categoria)
        {
            try
            {
                if (categoria is null)
                    return BadRequest("Categoria inválida");

                _uof.CategoriaRepository.Add(categoria);
                _uof.Commit();
                return CreatedAtAction(nameof(GetById), new { id = categoria.CategoriaId }, categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateCategoria(int id, Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                    return BadRequest($"id {id} inválido.");

                var ExisteCategoria = _uof.CategoriaRepository.GetById(x => x.CategoriaId == id);
                if (ExisteCategoria is null)
                    return NotFound($"Categoria com id {id} não encontrada");

                ExisteCategoria.Nome = categoria.Nome;
                ExisteCategoria.ImagemUrl = categoria.ImagemUrl;

                _uof.CategoriaRepository.Update(ExisteCategoria);
                _uof.Commit();

                return Ok(ExisteCategoria);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCategoria(int id)
        {
            try
            {
                var ExisteCategoria =  _uof.CategoriaRepository.GetById(x => x.CategoriaId == id);
                if (ExisteCategoria is null)
                    return NotFound($"Categoria com id {id} não encontrada");

                _uof.CategoriaRepository.Delete(ExisteCategoria);
                _uof.Commit();

                return Ok("Categoria Deletada");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }
    }
}
