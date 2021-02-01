using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using TesteMC1.Domain.Entity;
using TesteMC1.Domain.DTO;

namespace TesteMC1.Application.Services
{
    public class ProdutoService : BaseService, IDisposable
    {
        public enum Status { Ativos, Inativos, Todos }
        public enum PorId { PorId }
        public enum PorCodigoInterno { PorCodigoInterno }
        public enum PorCodigoBarras { PorCodigoBarras }

        public Produto Obter(long id, PorId tipoPesquisa)
        {
            try
            {
                return DbContext.Produtos.FirstOrDefault(w => w.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Produto Obter(string codigoInterno, PorCodigoInterno tipoPesquisa)
        {
            try
            {
                return DbContext.Produtos.FirstOrDefault(w => w.CodigoInterno == codigoInterno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Produto Obter(string codigoBarras, PorCodigoBarras tipoPesquisa)
        {
            try
            {
                return DbContext.Produtos.FirstOrDefault(w => w.CodigoBarras == codigoBarras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Produto> ObterTodos(Status status)
        {
            try
            {
                if (status == Status.Todos)
                {
                    return DbContext.Produtos.OrderBy(o => o.Descricao).ToList();
                }
                else
                {
                    bool statusPesquisa = status == Status.Ativos;

                    return DbContext.Produtos.Where(w => w.EstaAtivo == statusPesquisa).OrderBy(o => o.Descricao).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Produto> ObterTodos(long idCategoria, Status status)
        {
            try
            {
                if (status == Status.Todos)
                {
                    return DbContext.Produtos.Where(w => w.IdCategoria == idCategoria).OrderBy(o => o.Descricao).ToList();
                }
                else
                {
                    bool statusPesquisa = status == Status.Ativos;

                    return DbContext.Produtos.Where(w => w.IdCategoria == idCategoria &
                                                         w.EstaAtivo == statusPesquisa).OrderBy(o => o.Descricao).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long Incluir(Produto produto)
        {
            try
            {
                produto.OperacaoCRUD = BaseEntity.OperacoesCRUD.Create;

                produto.AjustarPropriedades();
                if (produto.PossuiErrosValidacao()) throw new Exception(produto.ObterMensagensErrosValidacao());

                if (!string.IsNullOrEmpty(produto.CodigoInterno))
                {
                    //Valida se o código interno informado já existe no banco de dados
                    if (DbContext.Produtos.Any(w => w.CodigoInterno == produto.CodigoInterno)) throw new Exception(string.Format("O código interno '{0}' já existe no cadastro de produtos!", produto.CodigoInterno));
                }

                if (!string.IsNullOrEmpty(produto.CodigoBarras))
                {
                    //Valida se o código de barras informado já existe no banco de dados
                    if (DbContext.Produtos.Any(w => w.CodigoBarras == produto.CodigoBarras)) throw new Exception(string.Format("O código de barras '{0}' já existe no cadastro de produtos!", produto.CodigoBarras));
                }
                
                DbContext.Produtos.Add(produto);
                DbContext.Entry(produto).State = EntityState.Added;
                DbContext.SaveChanges();

                return produto.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Atualizar(Produto produto)
        {
            try
            {
                Produto produtoExistente = Obter(produto.Id, PorId.PorId);
                if (produtoExistente == null) return;

                produtoExistente.OperacaoCRUD = BaseEntity.OperacoesCRUD.Update;

                produtoExistente.Descricao = produto.Descricao;
                produtoExistente.IdCategoria = produto.IdCategoria;
                produtoExistente.CodigoInterno = produto.CodigoInterno;
                produtoExistente.CodigoBarras = produto.CodigoBarras;
                produtoExistente.UnidadeMedida = produto.UnidadeMedida;
                produtoExistente.QtdEstoque = produto.QtdEstoque;
                produtoExistente.ValorUnitarioCusto = produto.ValorUnitarioCusto;
                produtoExistente.ValorUnitarioVenda = produto.ValorUnitarioVenda;
                produtoExistente.EstaAtivo = produto.EstaAtivo;

                produto.AjustarPropriedades();
                if (produtoExistente.PossuiErrosValidacao()) throw new Exception(produtoExistente.ObterMensagensErrosValidacao());

                DbContext.Produtos.Attach(produtoExistente);
                DbContext.Entry(produtoExistente).State = EntityState.Modified;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizarMovimentacao(long id, Movimentacao.TiposMovimentacao tipo, decimal quantidade, decimal? valorUnitarioCusto)
        {
            try
            {
                Produto produtoExistente = Obter(id, PorId.PorId);
                if (produtoExistente == null) return;

                produtoExistente.OperacaoCRUD = BaseEntity.OperacoesCRUD.Update;

                if (tipo == Movimentacao.TiposMovimentacao.Entrada)
                {
                    produtoExistente.QtdEstoque += quantidade;
                    if (valorUnitarioCusto != null) produtoExistente.ValorUnitarioCusto = valorUnitarioCusto.Value;
                }
                else
                {
                    produtoExistente.QtdEstoque -= quantidade;
                }
                produtoExistente.DataUltimaMovimentacao = DateTime.Now;

                produtoExistente.AjustarPropriedades();
                if (produtoExistente.PossuiErrosValidacao()) throw new Exception(produtoExistente.ObterMensagensErrosValidacao());

                DbContext.Produtos.Attach(produtoExistente);
                DbContext.Entry(produtoExistente).State = EntityState.Modified;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizarValorVenda(long idCategoria, decimal percentualCorrecao)
        {
            try
            {
                //Atualiza o valor de venda para todos os produtos da categoria informada
                List<Produto> produtos = ObterTodos(idCategoria, Status.Ativos);

                if (produtos != null)
                {
                    if (produtos.Count > 0)
                    {
                        foreach (var produto in produtos)
                        {
                            produto.ValorUnitarioVenda += (produto.ValorUnitarioVenda * (percentualCorrecao / 100));

                            DbContext.Produtos.Attach(produto);
                            DbContext.Entry(produto).State = EntityState.Modified;
                        }
                    }
                }

                //DbContext.Produtos.Attach(produtoExistente);
                //DbContext.Entry(produtoExistente).State = EntityState.Modified;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Excluir(Produto produto)
        {
            try
            {
                if (produto == null) return;
                Excluir(produto.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Excluir(long id)
        {
            try
            {
                Produto produto = Obter(id, PorId.PorId);
                if (produto == null) return;

                produto.OperacaoCRUD = BaseEntity.OperacoesCRUD.Detele;

                DbContext.Produtos.Remove(produto);
                DbContext.Entry(produto).State = EntityState.Deleted;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            if (DbContext != null)
            {
                DbContext.Dispose();
                DbContext = null;
            }
        }
    }
}
