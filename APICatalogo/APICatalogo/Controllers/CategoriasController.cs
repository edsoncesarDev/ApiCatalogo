using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository_Pattern;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public CategoriasController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }


        [HttpGet("produtos")]   //adicionando "produtos" a rota - api/[controller]/"produtos"
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            try
            {
                var categorias = await _uof.CategoriaRepository.GetCategoriasProdutos();
                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);

                return Ok(categoriasDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            try
            {
                var categorias = await _uof.CategoriaRepository.Get().ToListAsync();

                if (categorias is null)
                    return NotFound("Categorias não encontrada");

                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);

                return Ok(categoriasDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
           
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> GetById(int id)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetById(x => x.CategoriaId == id);

                if (categoria is null)
                    return NotFound($"Categoria com id {id} não encontrada");

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return Ok(categoriaDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }


        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> AddCategoria(CategoriaDTO categoriaDto)
        {
            try
            {
                if (categoriaDto is null)
                    return BadRequest("Categoria inválida");

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uof.CategoriaRepository.Add(categoria);
                await _uof.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return CreatedAtAction(nameof(GetById), new { id = categoriaDTO.CategoriaId }, categoriaDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> UpdateCategoria(int id, CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.CategoriaId)
                    return BadRequest($"id {id} inválido.");

                var ExisteCategoria = await _uof.CategoriaRepository.GetById(x => x.CategoriaId == id);
                if (ExisteCategoria is null)
                    return NotFound($"Categoria com id {id} não encontrada");

                ExisteCategoria.Nome = categoriaDto.Nome;
                ExisteCategoria.ImagemUrl = categoriaDto.ImagemUrl;

                _uof.CategoriaRepository.Update(ExisteCategoria);
                await _uof.Commit();

                return Ok(categoriaDto);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> DeleteCategoria(int id)
        {
            try
            {
                var ExisteCategoria =  await _uof.CategoriaRepository.GetById(x => x.CategoriaId == id);
                if (ExisteCategoria is null)
                    return NotFound($"Categoria com id {id} não encontrada");

                _uof.CategoriaRepository.Delete(ExisteCategoria);
                await _uof.Commit();

                var categoria = _mapper.Map<CategoriaDTO>(ExisteCategoria);

                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao validar sua solicitação");
            }
            
        }
    }
}
