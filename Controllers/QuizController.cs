using Microsoft.AspNetCore.Mvc;
using static TecnusAPI.Models.QuizModel;
using TecnusAPI.Models;
using TecnusAPI.Repositorio.Interfaces;

namespace Tecnus_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepositorio _quizRepositorio;

        public QuizController(IQuizRepositorio quizRepositorio)
        {
            _quizRepositorio = quizRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<QuizModel>>> Get()
        {
            var quizzes = await _quizRepositorio.BuscarTodos();
            return Ok(quizzes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizModel>> Get(int id)
        {
            var quiz = await _quizRepositorio.BuscarPorId(id);
            if (quiz == null)
            {
                return NotFound();
            }
            return Ok(quiz);
        }

        [HttpPost]
        public async Task<ActionResult<QuizModel>> Post([FromBody] QuizModel quiz)
        {
            var novoQuiz = await _quizRepositorio.Adicionar(quiz);
            return CreatedAtAction(nameof(Get), new { id = novoQuiz.Id_Quiz }, novoQuiz);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<QuizModel>> Put(int id, [FromBody] QuizModel quiz)
        {
            var quizAtualizado = await _quizRepositorio.Atualizar(quiz, id);
            return Ok(quizAtualizado);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _quizRepositorio.Apagar(id);
            return NoContent();
        }
    }
}
