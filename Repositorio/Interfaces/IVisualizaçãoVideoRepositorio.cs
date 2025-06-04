using TecnusAPI.Models;
namespace TecnusAPI.Repositorio.Interfaces
{
    public interface IVisualizacaoVideoRepositorio
    {

        Task<VisualizacaoVideoModel?> ObterPorUsuarioEVideo(string usuarioId, int videoId);


        Task<VisualizacaoVideoModel> Atualizar(VisualizacaoVideoModel visualizacao);


        Task<VisualizacaoVideoModel> Adicionar(VisualizacaoVideoModel visualizacao);


        Task<bool> SalvarAlteracoes();




    }
}
