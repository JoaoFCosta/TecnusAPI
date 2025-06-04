using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TecnusAPI.Models;

namespace TecnusAPI.Data.Mapa
{
    public class UsuarioMap : IEntityTypeConfiguration<UsuarioModel>
    {
        public void Configure(EntityTypeBuilder<UsuarioModel> builder)
        {
            builder.HasKey(x => x.Id_Usuario);
            builder.Property(x => x.Nome_Usuario).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Telefone_Usuario).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Email_Usuario).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Endereco_Usuario).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Complemento_Usuario).IsRequired().HasMaxLength(80);
            builder.Property(x => x.CPF_Usuario).IsRequired().HasMaxLength(14);
            builder.Property(x => x.CEP_Usuario).IsRequired().HasMaxLength(15);
            builder.Property(x => x.Senha_Usuario).IsRequired().HasMaxLength(255);
        }
    }
}