using Microsoft.AspNetCore.Mvc;
using TecnusAPI.Models;
using TecnusAPI.Repositorio.Interfaces;

namespace TecnusAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackRepositorio _feedbackRepositorio;

        public FeedbackController(IFeedbackRepositorio feedbackRepositorio)
        {
            _feedbackRepositorio = feedbackRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<FeedbackModel>>> ListarTodas()
        {
            List<FeedbackModel> feedback = await _feedbackRepositorio.BuscarTodosCursos();
            return Ok(feedback);
        }

        [HttpGet("Buscar Todos Feedback")]
        public async Task<ActionResult<FeedbackModel>> BuscarPorId(int id)
        {
            FeedbackModel feedback = await _feedbackRepositorio.BuscarPorId(id);
            return Ok(feedback);
        }

        [HttpPost("Cadastrar Feedback")]
        public async Task<ActionResult<FeedbackModel>> Cadastrar([FromBody] FeedbackModel feedbackModel)
        {
            FeedbackModel feedback = await _feedbackRepositorio.Adicionar(feedbackModel);

            return Ok(feedback);

        }

        [HttpPut("Atualizar Feedback por id")]
        public async Task<ActionResult<FeedbackModel>> Atualizar([FromBody] FeedbackModel feedbackModel, int id)
        {
            feedbackModel.Id_Feedback = id;
            FeedbackModel feedback = await _feedbackRepositorio.Atualizar(feedbackModel, id);
            return Ok(feedback);
        }

        [HttpDelete("Deletar por id")]
        public async Task<ActionResult<FeedbackModel>> Apagar(int id)
        {

            bool apagado = await _feedbackRepositorio.Apagar(id);
            return Ok(apagado);
        }
    }
}
