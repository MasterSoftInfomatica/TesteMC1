using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TesteMC1.Domain.Entity;

namespace TesteMC1.DataAccess.Context.SqlServer.Configurations
{
    public class UsuarioMap : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {
            ToTable("Usuarios", "dbo");

            HasKey(pk => new { pk.Id });
            Property(p => p.Id).HasColumnName("Id").HasColumnType("BIGINT").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.Nome).HasColumnName("Nome").HasColumnType("VARCHAR").HasMaxLength(20).IsRequired();
            Property(p => p.Sobrenome).HasColumnName("Sobrenome").HasColumnType("VARCHAR").HasMaxLength(100).IsRequired();
            Property(p => p.Email).HasColumnName("Email").HasColumnType("VARCHAR").IsRequired();
            Property(p => p.Senha).HasColumnName("Senha").HasColumnType("VARCHAR").IsRequired();
            Property(p => p.DataCriacao).HasColumnName("DataCriacao").HasColumnType("DATETIME").IsRequired();
            Property(p => p.CodigoAtivacao).HasColumnName("CodigoAtivacao").HasColumnType("VARCHAR").HasMaxLength(6);
            Property(p => p.DataCriacaoCodigoAtivacao).HasColumnName("DataCriacaoCodigoAtivacao").HasColumnType("DATETIME");
            Property(p => p.DataValidadeCodigoAtivacao).HasColumnName("DataValidadeCodigoAtivacao").HasColumnType("DATETIME");
            Property(p => p.DataAtivacao).HasColumnName("DataAtivacao").HasColumnType("DATETIME");
            Property(p => p.Perfil).HasColumnName("Perfil").HasColumnType("VARCHAR").HasMaxLength(30).IsRequired();
            Property(p => p.EstaAtivo).HasColumnName("EstaAtivo").HasColumnType("BIT").IsRequired();

            Ignore(i => i.OperacaoCRUD);
            Ignore(i => i.NomeCompleto);
            Ignore(i => i.PerfilUsuario);
            Ignore(i => i.DescricaoPerfil);
            Ignore(i => i.DescricaoEstaAtivo);
        }
    }
}
