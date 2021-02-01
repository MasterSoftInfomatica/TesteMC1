using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TesteMC1.Domain.Entity;

namespace TesteMC1.DataAccess.Context.SqlServer.Configurations
{
    public class MovimentacaoMap : EntityTypeConfiguration<Movimentacao>
    {
        public MovimentacaoMap()
        {
            ToTable("Movimentacoes", "dbo");

            HasKey(pk => new { pk.Id });
            Property(p => p.Id).HasColumnName("Id").HasColumnType("BIGINT").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.Data).HasColumnName("Data").HasColumnType("DATETIME").IsRequired();
            Property(p => p.Tipo).HasColumnName("Tipo").HasColumnType("VARCHAR").HasMaxLength(10).IsRequired();
            Property(p => p.IdFornecedor).HasColumnName("IdFornecedor").HasColumnType("BIGINT").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.IdCliente).HasColumnName("IdCliente").HasColumnType("BIGINT").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Ignore(i => i.OperacaoCRUD);
        }
    }
}
