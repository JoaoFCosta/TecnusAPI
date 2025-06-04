using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TecnusAPI.Models;

namespace TecnusAPI.Data.Mapa
{
    public class FeedbackMap : IEntityTypeConfiguration<FeedbackModel>
    {
        public void Configure(EntityTypeBuilder<FeedbackModel> builder)
        {
            builder.HasKey(x => x.Id_Feedback);
            builder.Property(x => x.Nome_Feedback).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Curso_Feedback).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Assunto_Feedback).IsRequired().HasMaxLength(600);
            builder.Property(x => x.Aula_Feedback).IsRequired().HasMaxLength(100);
        }
    }
}
