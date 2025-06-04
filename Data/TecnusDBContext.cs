using Microsoft.EntityFrameworkCore;
using TecnusAPI.Models;
using TecnusAPI.Data.Mapa;

namespace TecnusAPI.Data
{
    public class TecnusDBContext : DbContext
    {
        public TecnusDBContext(DbContextOptions<TecnusDBContext> options)
            : base(options)
        {
        }

        public DbSet<UsuarioModel> Tbl_Usuario { get; set; }
        public DbSet<CursoModel> Tbl_Curso { get; set; }
        public DbSet<FeedbackModel> Tbl_Feedback { get; set; }
        public DbSet<QuizModel> Tbl_Quiz { get; set; }
        public DbSet<PerguntaModel> Tbl_Pergunta { get; set; }
        public DbSet<RespostaModel> Tbl_Resposta { get; set; }
        public DbSet<VideoModel> Tbl_Video { get; set; }

        public DbSet<VisualizacaoVideoModel> Tbl_VisualizacaoVideo { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new CursoMap());
            modelBuilder.ApplyConfiguration(new FeedbackMap());
            modelBuilder.ApplyConfiguration(new QuizMap());
            modelBuilder.ApplyConfiguration(new PerguntaMap());
            modelBuilder.ApplyConfiguration(new RespostaMap());
            modelBuilder.ApplyConfiguration(new VideoVisualizacaoMap());
            modelBuilder.ApplyConfiguration(new VideoMap());


            base.OnModelCreating(modelBuilder);
        }

    }
}
