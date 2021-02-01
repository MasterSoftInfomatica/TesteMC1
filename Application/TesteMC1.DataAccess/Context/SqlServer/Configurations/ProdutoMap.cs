using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TesteMC1.Domain.Entity;

namespace TesteMC1.DataAccess.Context.SqlServer.Configurations
{
    public class ProdutoMap : EntityTypeConfiguration<Produto>
    {
        public ProdutoMap()
        {
            ToTable("Produtos", "dbo");

            HasKey(pk => new { pk.Id });
            Property(p => p.Id).HasColumnName("Id").HasColumnType("BIGINT").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.Descricao).HasColumnName("Descricao").HasColumnType("VARCHAR").HasMaxLength(100).IsRequired();
            Property(p => p.IdCategoria).HasColumnName("IdCategoria").HasColumnType("BIGINT").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.CodigoInterno).HasColumnName("CodigoInterno").HasColumnType("VARCHAR").HasMaxLength(50);
            Property(p => p.CodigoBarras).HasColumnName("CodigoBarras").HasColumnType("VARCHAR").HasMaxLength(50);
            Property(p => p.UnidadeMedida).HasColumnName("UnidadeMedida").HasColumnType("VARCHAR").HasMaxLength(10).IsRequired();
            Property(p => p.QtdEstoque).HasColumnName("QtdEstoque").HasColumnType("DECIMAL").HasPrecision(30, 5).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.ValorUnitarioCusto).HasColumnName("ValorUnitarioCusto").HasColumnType("DECIMAL").HasPrecision(30, 2).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.ValorUnitarioVenda).HasColumnName("ValorUnitarioVenda").HasColumnType("DECIMAL").HasPrecision(30, 2).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.DataUltimaMovimentacao).HasColumnName("DataUltimaMovimentacao").HasColumnType("DATETIME");
            Property(p => p.EstaAtivo).HasColumnName("EstaAtivo").HasColumnType("BIT").IsRequired();

            HasRequired(r => r.Categoria)
               .WithMany(m => m.Produtos)
               .HasForeignKey(fk => new { fk.IdCategoria });

            Ignore(i => i.OperacaoCRUD);
            Ignore(i => i.IdDescricao);
            Ignore(i => i.CodigoInternoDescricao);
            Ignore(i => i.CodigoBarrasDescricao);
            Ignore(i => i.ValorTotalCusto);
            Ignore(i => i.ValorTotalVenda);
            Ignore(i => i.DescricaoEstaAtivo);
        }
    }
}
