using TecnusAPI.Models;

namespace TecnusAPI.Repositorio.Interfaces
{
    public interface IFeedbackRepositorio
    {
        Task<List<FeedbackModel>> BuscarTodosCursos();
        Task<FeedbackModel> BuscarPorId(int id);
        Task<FeedbackModel> Adicionar(FeedbackModel feedback);

        Task<FeedbackModel> Atualizar(FeedbackModel feedback, int id);
        Task<bool> Apagar(int id);

    }
}
