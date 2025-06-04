using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TecnusAPI.Models;
using TecnusAPI.Repositorio.Interfaces;

namespace Tecnus_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoRepositorio _videoRepositorio;

        public VideoController(IVideoRepositorio videoRepositorio)
        {
            _videoRepositorio = videoRepositorio;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoModel>> ObterPorId(int id)
        {
            var video = await _videoRepositorio.ObterVideoPorId(id);
            if (video == null) return NotFound();
            return Ok(video);
        }

        [HttpPost]
        public async Task<ActionResult<VideoModel>> Create([FromBody] VideoModel video)
        {
            // Validação simples (pode ser substituída por FluentValidation)
            if (string.IsNullOrEmpty(video.Titulo)
                || string.IsNullOrEmpty(video.Url)
                || video.DuracaoSegundos <= 0)
            {
                return BadRequest("Dados do vídeo inválidos");
            }

            var createdVideo = await _videoRepositorio.CriarVideo(video);
            return CreatedAtAction(nameof(ObterPorId),
                new { id = createdVideo.Id_Video },
                createdVideo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VideoModel video)
        {
            if (id != video.Id_Video) return BadRequest("ID inconsistente");

            await _videoRepositorio.AtualizarVideo(video);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _videoRepositorio.DeletarVideoPorId(id);
            return NoContent();
        }
    }
}
