using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using TecnusAPI.Data;
using TecnusAPI.Models;
using TecnusAPI.Repositorio.Interfaces;

namespace TecnusAPI.Repositorio
{
    public class VideoRepositorio : IVideoRepositorio
    {
        private readonly TecnusDBContext _dbContext;

        public VideoRepositorio(TecnusDBContext tecnusdbcontext)
        {
            _dbContext = tecnusdbcontext;
        }

        public async Task<IEnumerable<VideoModel>> ObterTodosVideos()
        {
            return await _dbContext.Tbl_Video.ToListAsync();
        }

        public async Task<VideoModel> ObterVideoPorId(int id)
        {
            return await _dbContext.Tbl_Video.FindAsync(id);
        }

        public async Task<VideoModel> CriarVideo(VideoModel video)
        {
            _dbContext.Tbl_Video.Add(video);
            await _dbContext.SaveChangesAsync();
            return video;
        }

        public async Task AtualizarVideo(VideoModel video)
        {
            _dbContext.Entry(video).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletarVideoPorId(int id)
        {
            var video = await _dbContext.Tbl_Video.FindAsync(id);
            if (video != null)
            {
                _dbContext.Tbl_Video.Remove(video);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<VideoModel> BuscarPorId(int id)
        {
            return await _dbContext.Tbl_Video
                .FirstOrDefaultAsync(v => v.Id_Video == id);
        }
    }
}
