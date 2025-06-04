using TecnusAPI.Models;

namespace TecnusAPI.Repositorio.Interfaces
{
    public interface ICursoRepositorio
    {
        Task<List<CursoModel>> BuscarTodosCursos();
        Task<CursoModel> BuscarPorId(int id);
        Task<CursoModel> Adicionar(CursoModel curso);

        Task<CursoModel> Atualizar(CursoModel curso, int id);
        Task<bool> Apagar(int id);

    }
}

