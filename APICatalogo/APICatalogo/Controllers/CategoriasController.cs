using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _context.Categorias.AsNoTracking().ToListAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
           
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var Categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(x => x.CategoriaId == id);

                if (Categoria is null)
                    return NotFound($"Categoria com id {id} não encontrada");

                return Ok(Categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpGet("produtos")]   //adicionando "produtos" a rota - api/[controller]/"produtos"
        public async Task<IActionResult> GetCategoriasProdutos()
        {
            try
            {
                //include -> incluindo os produtos das categorias
                return Ok(await _context.Categorias.Include(p => p.Produtos).ToListAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> AddCategoria(Categoria categoria)
        {
            try
            {
                if (categoria is null)
                    return BadRequest("Categoria inválida");

                await _context.Categorias.AddAsync(categoria);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = categoria.CategoriaId });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategoria(int id, Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                    return BadRequest($"id {id} inválido.");

                var ExisteCategoria = await _context.Categorias.FirstOrDefaultAsync(x => x.CategoriaId == id);
                if (ExisteCategoria is null)
                    return NotFound($"Categoria com id {id} não encontrada");

                ExisteCategoria.Nome = categoria.Nome;
                ExisteCategoria.ImagemUrl = categoria.ImagemUrl;
                await _context.SaveChangesAsync();

                return Ok(ExisteCategoria);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            try
            {
                var ExisteCategoria = await _context.Categorias.FirstOrDefaultAsync(x => x.CategoriaId == id);
                if (ExisteCategoria is null)
                    return NotFound($"Categoria com id {id} não encontrada");

                _context.Remove(ExisteCategoria);
                await _context.SaveChangesAsync();

                return Ok("Categoria Deletada");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }
    }
}
