using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecnusAPI.Models;

namespace TecnusAPI.Data.Mapa
{
    public class VideoVisualizacaoMap : IEntityTypeConfiguration<VisualizacaoVideoModel>
    {
        public void Configure(EntityTypeBuilder<VisualizacaoVideoModel> builder)
        {
            builder.ToTable("VisualizacoesVideos");

            // Chave primária
            builder.HasKey(vv => vv.Id_Visualizacao);

            // Relacionamento com Video (1:N)
            builder.HasOne(vv => vv.Video)
                .WithMany() // Video não tem coleção de VisualizacaoVideo
                .HasForeignKey(vv => vv.VideoId_Visualizacao)
                .OnDelete(DeleteBehavior.Cascade); // Opcional: se deletar o vídeo, deleta as visualizações

            // Propriedades
            builder.Property(vv => vv.UsuarioId_Visualizacao)
                .IsRequired()
                .HasColumnType("varchar(36)"); // Ou o tipo do seu ID de usuário

            builder.Property(vv => vv.TempoAssistidoSegundos_Visualizacao)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(vv => vv.Concluido)
                .IsRequired()
                .HasColumnType("bit");

            builder.Property(vv => vv.Ultima_Visuailizacao)
                .IsRequired()
                .HasColumnType("datetime");

            // Índices para consultas rápidas
            builder.HasIndex(vv => new { vv.UsuarioId_Visualizacao, vv.VideoId_Visualizacao }, "IX_Visualizacoes_UsuarioVideo");

        }
    }
}
