using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TecnusAPI.Models;

namespace TecnusAPI.Repositorio.Interfaces
{
    public interface IVideoRepositorio
    {
        Task<IEnumerable<VideoModel>> ObterTodosVideos();
        Task<VideoModel> ObterVideoPorId(int id);
        Task<VideoModel> CriarVideo(VideoModel video);
        Task AtualizarVideo(VideoModel video);
        Task DeletarVideoPorId(int id);
        Task<VideoModel> BuscarPorId(int id);
    }
}
