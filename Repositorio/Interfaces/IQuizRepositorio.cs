using TecnusAPI.Models;

namespace TecnusAPI.Repositorio.Interfaces
{
    public interface IQuizRepositorio
    {
        Task<List<QuizModel>> BuscarTodos();
        Task<QuizModel> BuscarPorId(int id);
        Task<QuizModel> Adicionar(QuizModel quiz);
        Task<QuizModel> Atualizar(QuizModel quiz, int id);
        Task<bool> Apagar(int id);
    }
}
