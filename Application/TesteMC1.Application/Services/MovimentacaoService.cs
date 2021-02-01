using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using TesteMC1.Domain.Entity;
using TesteMC1.Domain.DTO;

namespace TesteMC1.Application.Services
{
    public class MovimentacaoService : BaseService, IDisposable
    {
        public Movimentacao Obter(long id)
        {
            try
            {
                return DbContext.Movimentacoes.FirstOrDefault(w => w.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Movimentacao> ObterTodas(Movimentacao.TiposMovimentacao tipo, DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                if (dataFinal < dataInicial) throw new Exception("A data final deve ser maior do que a data inicial!");

                return DbContext.Movimentacoes.Where(w => w.TipoMovimentacao == tipo &
                                                          w.Data >= dataInicial &
                                                          w.Data <= dataFinal).OrderBy(o => o.Data).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Incluir(Movimentacao movimentacao)
        {
            try
            {
                movimentacao.OperacaoCRUD = BaseEntity.OperacoesCRUD.Create;

                movimentacao.AjustarPropriedades();
                if (movimentacao.PossuiErrosValidacao()) throw new Exception(movimentacao.ObterMensagensErrosValidacao());

                DbContext.Movimentacoes.Add(movimentacao);
                DbContext.Entry(movimentacao).State = EntityState.Added;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Atualizar(Movimentacao movimentacao)
        {
            try
            {
                Movimentacao movimentacaoExistente = Obter(movimentacao.Id);
                if (movimentacaoExistente == null) return;

                movimentacaoExistente.OperacaoCRUD = BaseEntity.OperacoesCRUD.Update;

                movimentacaoExistente.Data = movimentacao.Data;
                movimentacaoExistente.TipoMovimentacao = movimentacao.TipoMovimentacao;
                movimentacaoExistente.IdFornecedor = movimentacao.IdFornecedor;
                movimentacaoExistente.IdCliente = movimentacao.IdCliente;

                movimentacao.AjustarPropriedades();
                if (movimentacaoExistente.PossuiErrosValidacao()) throw new Exception(movimentacaoExistente.ObterMensagensErrosValidacao());

                DbContext.Movimentacoes.Attach(movimentacaoExistente);
                DbContext.Entry(movimentacaoExistente).State = EntityState.Modified;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Excluir(Movimentacao movimentacao)
        {
            try
            {
                if (movimentacao == null) return;
                Excluir(movimentacao.Id);
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
                Movimentacao movimentacao = Obter(id);
                if (movimentacao == null) return;

                movimentacao.OperacaoCRUD = BaseEntity.OperacoesCRUD.Detele;

                DbContext.Movimentacoes.Remove(movimentacao);
                DbContext.Entry(movimentacao).State = EntityState.Deleted;
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
