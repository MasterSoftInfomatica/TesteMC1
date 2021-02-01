using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TesteMC1.Domain.Entity;

namespace TesteMC1.DataAccess.Context.SqlServer
{
    public class Context : DbContext
    {
        public Context()
        {
            //Seta a connection string de acordo com a configuração do arquivo .config da aplicação
            Database.Connection.ConnectionString = Properties.Settings.Default.ConnectionString;
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Movimentacao> Movimentacoes { get; set; }
        public DbSet<MovimentacaoItem> MovimentacoesItens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<NavigationPropertyNameForeignKeyDiscoveryConvention>();

            modelBuilder.Configurations.Add(new Configurations.UsuarioMap());
            modelBuilder.Configurations.Add(new Configurations.CategoriaMap());
            modelBuilder.Configurations.Add(new Configurations.ProdutoMap());
            modelBuilder.Configurations.Add(new Configurations.MovimentacaoMap());
            modelBuilder.Configurations.Add(new Configurations.MovimentacaoItemMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
