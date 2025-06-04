using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnusAPI.Models;

namespace TecnusAPI.Data.Mapa
{
    public class PerguntaMap : IEntityTypeConfiguration<PerguntaModel>
    {
        public void Configure(EntityTypeBuilder<PerguntaModel> builder)
        {
            builder.HasKey(x => x.Id_Pergunta);
            builder.Property(x => x.Texto).IsRequired().HasMaxLength(500);
            builder.HasOne(p => p.Quiz).WithMany(q => q.Perguntas).HasForeignKey(p => p.QuizId);
        }
    }
}
