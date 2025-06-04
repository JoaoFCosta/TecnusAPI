using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnusAPI.Models;

namespace TecnusAPI.Data.Mapa
{
    public class VideoMap : IEntityTypeConfiguration<VideoModel>
    {
        public void Configure(EntityTypeBuilder<VideoModel> builder)
        {
            builder.HasKey(x => x.Id_Video);


            builder.Property(x => x.Titulo)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Url)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.DuracaoSegundos)
                   .IsRequired();


        }
    }
}

