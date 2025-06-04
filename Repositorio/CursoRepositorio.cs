using TecnusAPI.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;
using TecnusAPI.Data;
using TecnusAPI.Models;


namespace TecnusAPI.Repositorio
{
    public class CursoRepositorio : ICursoRepositorio
    {
        private readonly TecnusDBContext _dbContext;

        public CursoRepositorio(TecnusDBContext tecnusdbcontext)
        {
            _dbContext = tecnusdbcontext;

        }
        public async Task<CursoModel> BuscarPorId(int id)
        {
            return await _dbContext.Tbl_Curso.FirstOrDefaultAsync(x => x.Id_Curso == id);
        }

        public async Task<List<CursoModel>> BuscarTodosCursos()
        {
            return await _dbContext.Tbl_Curso.ToListAsync();
        }
        public async Task<CursoModel> Adicionar(CursoModel curso)
        {
            await _dbContext.Tbl_Curso.AddAsync(curso);
            await _dbContext.SaveChangesAsync();

            return curso;
        }
        public async Task<CursoModel> Atualizar(CursoModel curso, int id)
        {
            CursoModel cursoPorId = await BuscarPorId(id);

            if (cursoPorId == null)
            {
                throw new Exception($"Curso para o ID: {id} não foi encontrado no banco de dados.");
            }
            cursoPorId.Nome_Curso = curso.Nome_Curso;
            cursoPorId.Descricao_Curso = curso.Descricao_Curso;
            cursoPorId.Duracao_Curso = curso.Duracao_Curso;
            cursoPorId.Professor_Curso = curso.Professor_Curso;


            _dbContext.Tbl_Curso.Update(cursoPorId);
            await _dbContext.SaveChangesAsync();
            return cursoPorId;
        }

        public async Task<bool> Apagar(int id)
        {
            CursoModel cursoPorId = await BuscarPorId(id);

            if (cursoPorId == null)
            {
                throw new Exception($"curso para o ID: {id} não foi encontrado no banco de dados.");

            }
            _dbContext.Tbl_Curso.Remove(cursoPorId);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
