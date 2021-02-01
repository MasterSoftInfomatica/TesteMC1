using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TesteMC1.Domain.Entity;

namespace TesteMC1.DataAccess.Context.SqlServer.Configurations
{
    public class CategoriaMap : EntityTypeConfiguration<Categoria>
    {
        public CategoriaMap()
        {
            ToTable("Categorias", "dbo");

            HasKey(pk => new { pk.Id });
            Property(p => p.Id).HasColumnName("Id").HasColumnType("BIGINT").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.Descricao).HasColumnName("Descricao").HasColumnType("VARCHAR").HasMaxLength(50).IsRequired();
            Property(p => p.EstaAtiva).HasColumnName("EstaAtiva").HasColumnType("BIT").IsRequired();

            Ignore(i => i.OperacaoCRUD);
            Ignore(i => i.IdDescricao);
            Ignore(i => i.DescricaoEstaAtiva);
        }
    }
}
