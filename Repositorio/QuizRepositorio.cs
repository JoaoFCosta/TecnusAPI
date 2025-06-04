using Microsoft.EntityFrameworkCore;
using TecnusAPI.Data;
using TecnusAPI.Models;
using TecnusAPI.Repositorio.Interfaces;

namespace TecnusAPI.Repositorio
{
    public class QuizRepositorio : IQuizRepositorio
    {
        private readonly TecnusDBContext _dbContext;

        public QuizRepositorio(TecnusDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<QuizModel>> BuscarTodos()
        {
            return await _dbContext.Tbl_Quiz
                .Include(q => q.Perguntas)
                    .ThenInclude(p => p.Respostas)
                .ToListAsync();
        }

        public async Task<QuizModel> BuscarPorId(int id)
        {
            return await _dbContext.Tbl_Quiz
                .Include(q => q.Perguntas)
                    .ThenInclude(p => p.Respostas)
                .FirstOrDefaultAsync(q => q.Id_Quiz == id);
        }

        public async Task<QuizModel> Adicionar(QuizModel quiz)
        {
            await _dbContext.Tbl_Quiz.AddAsync(quiz);
            await _dbContext.SaveChangesAsync();
            return quiz;
        }

        public async Task<QuizModel> Atualizar(QuizModel quiz, int id)
        {
            var quizExistente = await BuscarPorId(id);
            if (quizExistente == null)
            {
                throw new Exception($"Quiz com ID {id} não encontrado.");
            }

            quizExistente.Titulo_Quizz = quiz.Titulo_Quizz;
            // Atualize outras propriedades conforme necessário

            _dbContext.Tbl_Quiz.Update(quizExistente);
            await _dbContext.SaveChangesAsync();
            return quizExistente;
        }

        public async Task<bool> Apagar(int id)
        {
            var quiz = await BuscarPorId(id);
            if (quiz == null)
            {
                throw new Exception($"Quiz com ID {id} não encontrado.");
            }

            _dbContext.Tbl_Quiz.Remove(quiz);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
