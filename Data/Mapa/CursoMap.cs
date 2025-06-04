using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnusAPI.Models;

namespace TecnusAPI.Data.Mapa
{
    public class CursoMap : IEntityTypeConfiguration<CursoModel>
    {
        public void Configure(EntityTypeBuilder<CursoModel> builder)
        {
            builder.HasKey(x => x.Id_Curso);
            builder.Property(x => x.Nome_Curso).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Descricao_Curso).IsRequired().HasMaxLength(800);
            builder.Property(x => x.Duracao_Curso).IsRequired().HasMaxLength(300);
            builder.Property(x => x.Professor_Curso).IsRequired().HasMaxLength(200);

        }
    }
}
