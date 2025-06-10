using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TecnusAPI.Models;

namespace TecnusAPI.Data
{
    public class TecnusDBContext : IdentityDbContext<AppUsuario>
    {
        public TecnusDBContext(DbContextOptions<TecnusDBContext> options)
            : base(options)
        {
        }
        public DbSet<AppUsuario> AppUsuarios { get; set; }
        public DbSet<CursoModel> CursosModel { get; set; }
        public DbSet<FeedbackModel> FeedbacksModel { get; set; }
        public DbSet<QuizModel> QuizzesModel { get; set; }
        public DbSet<PerguntaModel> PerguntasModel { get; set; }
        public DbSet<RespostaModel> RespostasModel { get; set; }
        public DbSet<VideoModel> VideoModel { get; set; }
        public DbSet<VisualizacaoVideoModel> VisualizacaoVideosModel { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Essa chamada é obrigatória para Identity funcionar corretamente.
            base.OnModelCreating(builder);

            builder.Entity<CursoModel>().HasKey(c => c.Id_Curso);
            builder.Entity<FeedbackModel>().HasKey(c => c.Id_Feedback);
            builder.Entity<PerguntaModel>().HasKey(c => c.Id_Pergunta);
            builder.Entity<QuizModel>().HasKey(c => c.Id_Quiz);
            builder.Entity<RespostaModel>().HasKey(c => c.Id_Resposta);
            builder.Entity<VideoModel>().HasKey(c => c.Id_Video);
            builder.Entity<VisualizacaoVideoModel>().HasKey(c => c.Id_Visualizacao);


            builder.Entity<CursoModel>().ToTable("Cursos");
            builder.Entity<FeedbackModel>().ToTable("Feedbacks");
            builder.Entity<QuizModel>().ToTable("Quizzes");
            builder.Entity<PerguntaModel>().ToTable("Perguntas");
            builder.Entity<RespostaModel>().ToTable("Respostas");
            builder.Entity<VideoModel>().ToTable("Videos");
            builder.Entity<VisualizacaoVideoModel>().ToTable("VisualizacaoVideos");
            builder.Entity<AppUsuario>().ToTable("Usuarios");
        }
    }
}