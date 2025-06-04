using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnusAPI.Models;

namespace TecnusAPI.Data.Mapa
{
    public class RespostaMap : IEntityTypeConfiguration<RespostaModel>
    {
        public void Configure(EntityTypeBuilder<RespostaModel> builder)
        {
            builder.HasKey(r => r.Id_Resposta);
            builder.Property(r => r.Texto).IsRequired().HasMaxLength(500);
            builder.Property(r => r.Correta).IsRequired();

            builder.HasOne(r => r.Pergunta)
                   .WithMany(p => p.Respostas)
                   .HasForeignKey(r => r.PerguntaId);
        }
    }
}