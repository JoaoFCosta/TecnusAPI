using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnusAPI.Models;

namespace TecnusAPI.Data.Mapa
{
    public class QuizMap : IEntityTypeConfiguration<QuizModel>
    {
        public void Configure(EntityTypeBuilder<QuizModel> builder)
        {
            builder.HasKey(x => x.Id_Quiz);
            builder.Property(x => x.Titulo_Quizz).IsRequired().HasMaxLength(200);
            builder.HasMany(q => q.Perguntas).WithOne(p => p.Quiz).HasForeignKey(p => p.QuizId);
        }
    }
}
