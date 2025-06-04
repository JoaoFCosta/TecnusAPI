using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TecnusAPI.Data;
using TecnusAPI.Models;
using TecnusAPI.Repositorio.Interfaces;


namespace TecnusAPI.Repositorio
{
    public class VisualizacaoVideoRepositorio : IVisualizacaoVideoRepositorio
    {
        private readonly TecnusDBContext _dbContext;

        public async Task<VisualizacaoVideoModel> ObterPorUsuarioEVideo(string usuarioId, int videoId)
        {
            return await _dbContext.Tbl_VisualizacaoVideo
                .Include(v => v.Video)
                .FirstOrDefaultAsync(v => v.UsuarioId_Visualizacao == usuarioId && v.VideoId_Visualizacao == videoId);
        }

        public async Task<VisualizacaoVideoModel> Adicionar(VisualizacaoVideoModel visualizacao)
        {
            await _dbContext.Tbl_VisualizacaoVideo.AddAsync(visualizacao);
            await _dbContext.SaveChangesAsync();
            return visualizacao;
        }

        public async Task<VisualizacaoVideoModel> Atualizar(VisualizacaoVideoModel visualizacao)
        {
            _dbContext.Tbl_VisualizacaoVideo.Update(visualizacao);
            await _dbContext.SaveChangesAsync();
            return visualizacao;
        }

        public async Task<bool> SalvarAlteracoes()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }


    }
}
