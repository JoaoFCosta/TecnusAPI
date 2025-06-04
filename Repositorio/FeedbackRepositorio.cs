using Microsoft.EntityFrameworkCore;
using TecnusAPI.Data;
using TecnusAPI.Models;
using TecnusAPI.Repositorio.Interfaces;

namespace TecnusAPI.Repositorio
{
    public class FeedbackRepositorio : IFeedbackRepositorio
    {
        private readonly TecnusDBContext _dbContext;

        public FeedbackRepositorio(TecnusDBContext tecnusdbcontext)
        {
            _dbContext = tecnusdbcontext;

        }
        public async Task<FeedbackModel> BuscarPorId(int id)
        {
            return await _dbContext.Tbl_Feedback.FirstOrDefaultAsync(x => x.Id_Feedback == id);
        }

        public async Task<List<FeedbackModel>> BuscarTodosCursos()
        {
            return await _dbContext.Tbl_Feedback.ToListAsync();
        }
        public async Task<FeedbackModel> Adicionar(FeedbackModel feedback)
        {
            await _dbContext.Tbl_Feedback.AddAsync(feedback);
            await _dbContext.SaveChangesAsync();

            return feedback;
        }
        public async Task<FeedbackModel> Atualizar(FeedbackModel feedback, int id)
        {
            FeedbackModel feedbackPorId = await BuscarPorId(id);

            if (feedbackPorId == null)
            {
                throw new Exception($"Feedback para o ID: {id} não foi encontrado no banco de dados.");
            }
            feedbackPorId.Nome_Feedback = feedback.Nome_Feedback;
            feedbackPorId.Curso_Feedback = feedback.Curso_Feedback;
            feedbackPorId.Aula_Feedback = feedback.Aula_Feedback;
            feedbackPorId.Assunto_Feedback = feedback.Assunto_Feedback;


            _dbContext.Tbl_Feedback.Update(feedbackPorId);
            await _dbContext.SaveChangesAsync();
            return feedbackPorId;
        }

        public async Task<bool> Apagar(int id)
        {
            FeedbackModel feedbackPorId = await BuscarPorId(id);

            if (feedbackPorId == null)
            {
                throw new Exception($"Feedback para o ID: {id} não foi encontrado no banco de dados.");

            }
            _dbContext.Tbl_Feedback.Remove(feedbackPorId);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
