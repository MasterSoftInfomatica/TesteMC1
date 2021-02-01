using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using TesteMC1.Domain.Entity;
using TesteMC1.Domain.DTO;

namespace TesteMC1.Application.Services
{
    public class MovimentacaoItemService : BaseService, IDisposable
    {
        public MovimentacaoItem Obter(long idMovimentacao, int numeroItem)
        {
            try
            {
                return DbContext.MovimentacoesItens.FirstOrDefault(w => w.IdMovimentacao == idMovimentacao &
                                                                        w.NumeroItem == numeroItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MovimentacaoItem> ObterTodos(long idMovimentacao)
        {
            try
            {
                return DbContext.MovimentacoesItens.Where(w => w.IdMovimentacao == idMovimentacao).OrderBy(o => o.NumeroItem).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Incluir(MovimentacaoItem movimentacaoItem)
        {
            try
            {
                movimentacaoItem.OperacaoCRUD = BaseEntity.OperacoesCRUD.Create;

                movimentacaoItem.AjustarPropriedades();
                if (movimentacaoItem.PossuiErrosValidacao()) throw new Exception(movimentacaoItem.ObterMensagensErrosValidacao());

                DbContext.MovimentacoesItens.Add(movimentacaoItem);
                DbContext.Entry(movimentacaoItem).State = EntityState.Added;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Atualizar(MovimentacaoItem movimentacaoItem)
        {
            try
            {
                MovimentacaoItem movimentacaoItemExistente = Obter(movimentacaoItem.IdMovimentacao, movimentacaoItem.NumeroItem);
                if (movimentacaoItemExistente == null) return;

                movimentacaoItemExistente.OperacaoCRUD = BaseEntity.OperacoesCRUD.Update;

                movimentacaoItemExistente.IdProduto = movimentacaoItem.IdProduto;
                movimentacaoItemExistente.Quantidade = movimentacaoItem.Quantidade;
                movimentacaoItemExistente.ValorUnitario = movimentacaoItem.ValorUnitario;

                movimentacaoItem.AjustarPropriedades();
                if (movimentacaoItemExistente.PossuiErrosValidacao()) throw new Exception(movimentacaoItemExistente.ObterMensagensErrosValidacao());

                DbContext.MovimentacoesItens.Attach(movimentacaoItemExistente);
                DbContext.Entry(movimentacaoItemExistente).State = EntityState.Modified;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Excluir(MovimentacaoItem movimentacaoItem)
        {
            try
            {
                if (movimentacaoItem == null) return;
                Excluir(movimentacaoItem.IdMovimentacao, movimentacaoItem.NumeroItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Excluir(long idMovimentacao, int numeroItem)
        {
            try
            {
                MovimentacaoItem movimentacaoItem = Obter(idMovimentacao, numeroItem);
                if (movimentacaoItem == null) return;

                movimentacaoItem.OperacaoCRUD = BaseEntity.OperacoesCRUD.Detele;

                DbContext.MovimentacoesItens.Remove(movimentacaoItem);
                DbContext.Entry(movimentacaoItem).State = EntityState.Deleted;
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
