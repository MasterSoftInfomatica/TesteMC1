using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TesteMC1.Domain.Entity;

namespace TesteMC1.DataAccess.Context.SqlServer.Configurations
{
    public class MovimentacaoItemMap : EntityTypeConfiguration<MovimentacaoItem>
    {
        public MovimentacaoItemMap()
        {
            ToTable("MovimentacoesItens", "dbo");

            HasKey(pk => new { pk.IdMovimentacao, pk.NumeroItem });
            Property(p => p.IdMovimentacao).HasColumnName("IdMovimentacao").HasColumnType("BIGINT").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.NumeroItem).HasColumnName("NumeroItem").HasColumnType("INT").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.IdProduto).HasColumnName("IdProduto").HasColumnType("BIGINT").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.Quantidade).HasColumnName("Quantidade").HasColumnType("DECIMAL").HasPrecision(30, 5).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.ValorUnitario).HasColumnName("ValorUnitario").HasColumnType("DECIMAL").HasPrecision(30, 2).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(r => r.Movimentacao)
               .WithMany(m => m.MovimentacoesItens)
               .HasForeignKey(fk => new { fk.IdMovimentacao });

            HasRequired(r => r.Produto)
               .WithMany(m => m.MovimentacoesItens)
               .HasForeignKey(fk => new { fk.IdProduto });

            Ignore(i => i.OperacaoCRUD);
            Ignore(i => i.ValorTotal);
        }
    }
}
