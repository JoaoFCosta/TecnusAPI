using Microsoft.AspNetCore.Mvc;
using TecnusAPI.Models.ViewModel;
using TecnusAPI.Models;
using TecnusAPI.Repositorio.Interfaces;

namespace Tecnus_API.Controllers
{
    public class VisualizacaoVideoController : ControllerBase
    {
        private readonly IVisualizacaoVideoRepositorio _visualizacaoRepositorio;
        private readonly IVideoRepositorio _videoRepositorio;

        public VisualizacaoVideoController(
            IVisualizacaoVideoRepositorio visualizacaoRepositorio,
            IVideoRepositorio videoRepositorio)
        {
            _visualizacaoRepositorio = visualizacaoRepositorio;
            _videoRepositorio = videoRepositorio;
        }

        // Endpoint para registrar o progresso de visualização
        [HttpPost("registrar-progresso")]
        public async Task<IActionResult> RegistrarProgresso([FromBody] RegistrarProgressoViewModel model)
        {
            // Validação básica
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var video = await _videoRepositorio.BuscarPorId(model.VideoId);
            if (video == null)
                return NotFound("Vídeo não encontrado");

            // Busca ou cria um registro de visualização
            var visualizacao = await _visualizacaoRepositorio.ObterPorUsuarioEVideo(model.UsuarioId, model.VideoId);

            if (visualizacao == null)
            {
                visualizacao = new VisualizacaoVideoModel
                {
                    VideoId_Visualizacao = model.VideoId,
                    UsuarioId_Visualizacao = model.UsuarioId,
                    TempoAssistidoSegundos_Visualizacao = model.TempoAssistidoSegundos,
                    Concluido = model.TempoAssistidoSegundos >= video.DuracaoSegundos
                };
                await _visualizacaoRepositorio.Adicionar(visualizacao);
            }
            else
            {
                visualizacao.TempoAssistidoSegundos_Visualizacao = model.TempoAssistidoSegundos;
                visualizacao.Concluido = model.TempoAssistidoSegundos >= video.DuracaoSegundos;
                await _visualizacaoRepositorio.Atualizar(visualizacao);
            }

            // Persiste as alterações
            await _visualizacaoRepositorio.SalvarAlteracoes();

            return Ok(new
            {
                PorcentagemAssistida = (decimal)visualizacao.TempoAssistidoSegundos_Visualizacao / video.DuracaoSegundos * 100,
                TempoAssistidoSegundos = visualizacao.TempoAssistidoSegundos_Visualizacao,
                Concluido = visualizacao.Concluido
            });
        }

        // Endpoint para obter o progresso do usuário em um vídeo
        [HttpGet("progresso/{usuarioId}/{videoId}")]
        public async Task<IActionResult> ObterProgresso(string usuarioId, int videoId)
        {
            var visualizacao = await _visualizacaoRepositorio.ObterPorUsuarioEVideo(usuarioId, videoId);
            var video = await _videoRepositorio.BuscarPorId(videoId);

            if (video == null)
                return NotFound("Vídeo não encontrado");

            if (visualizacao == null)
                return Ok(new
                {
                    PorcentagemAssistida = 0,
                    Concluido = false,
                    TempoAssistidoSegundos = 0
                });

            return Ok(new
            {
                PorcentagemAssistida = (decimal)visualizacao.TempoAssistidoSegundos_Visualizacao / video.DuracaoSegundos * 100,
                TempoAssistidoSegundos = visualizacao.TempoAssistidoSegundos_Visualizacao,
                Concluido = visualizacao.Concluido
            });
        }

    }


}


