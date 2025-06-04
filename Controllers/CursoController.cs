using Microsoft.AspNetCore.Mvc;
using TecnusAPI.Models;
using TecnusAPI.Repositorio.Interfaces;

namespace TecnusAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CursoController : Controller
    {
        private readonly ICursoRepositorio _cursoRepositorio;

        public CursoController(ICursoRepositorio cursoRepositorio)
        {
            _cursoRepositorio = cursoRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<CursoModel>>> ListarTodas()
        {
            List<CursoModel> curso = await _cursoRepositorio.BuscarTodosCursos();
            return Ok(curso);
        }

        [HttpGet("Buscar Todos Cursos")]
        public async Task<ActionResult<CursoModel>> BuscarPorId(int id)
        {
            CursoModel curso = await _cursoRepositorio.BuscarPorId(id);
            return Ok(curso);
        }

        [HttpPost("Cadastrar Curso")]
        public async Task<ActionResult<CursoModel>> Cadastrar([FromBody] CursoModel cursoModel)
        {
            CursoModel curso = await _cursoRepositorio.Adicionar(cursoModel);

            return Ok(curso);

        }

        [HttpPut("Atualizar Curso por id")]
        public async Task<ActionResult<CursoModel>> Atualizar([FromBody] CursoModel cursoModel, int id)
        {
            cursoModel.Id_Curso = id;
            CursoModel curso = await _cursoRepositorio.Atualizar(cursoModel, id);
            return Ok(curso);
        }

        [HttpDelete("Deletar por id")]
        public async Task<ActionResult<CursoModel>> Apagar(int id)
        {

            bool apagado = await _cursoRepositorio.Apagar(id);
            return Ok(apagado);
        }
    }
}
