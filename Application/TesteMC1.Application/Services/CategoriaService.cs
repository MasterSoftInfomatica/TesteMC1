using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using TesteMC1.Domain.Entity;
using TesteMC1.Domain.DTO;

namespace TesteMC1.Application.Services
{
    public class CategoriaService : BaseService, IDisposable
    {
        public enum Status { Ativas, Inativos, Todas }

        public Categoria Obter(long id)
        {
            try
            {
                return DbContext.Categorias.FirstOrDefault(w => w.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Categoria> ObterTodas(Status status)
        {
            try
            {
                if (status == Status.Todas)
                {
                    return DbContext.Categorias.OrderBy(o => o.Descricao).ToList();
                }
                else
                {
                    bool statusPesquisa = status == Status.Ativas;

                    return DbContext.Categorias.Where(w => w.EstaAtiva == statusPesquisa).OrderBy(o => o.Descricao).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long Incluir(Categoria categoria)
        {
            try
            {
                categoria.OperacaoCRUD = BaseEntity.OperacoesCRUD.Create;

                categoria.AjustarPropriedades();
                if (categoria.PossuiErrosValidacao()) throw new Exception(categoria.ObterMensagensErrosValidacao());

                DbContext.Categorias.Add(categoria);
                DbContext.Entry(categoria).State = EntityState.Added;
                DbContext.SaveChanges();

                return categoria.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Atualizar(Categoria categoria)
        {
            try
            {
                Categoria categoriaExistente = Obter(categoria.Id);
                if (categoriaExistente == null) return;

                categoriaExistente.OperacaoCRUD = BaseEntity.OperacoesCRUD.Update;

                categoriaExistente.Descricao = categoria.Descricao;
                categoriaExistente.EstaAtiva = categoria.EstaAtiva;

                categoria.AjustarPropriedades();
                if (categoriaExistente.PossuiErrosValidacao()) throw new Exception(categoriaExistente.ObterMensagensErrosValidacao());

                DbContext.Categorias.Attach(categoriaExistente);
                DbContext.Entry(categoriaExistente).State = EntityState.Modified;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Excluir(Categoria categoria)
        {
            try
            {
                if (categoria == null) return;
                Excluir(categoria.Id);
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
                Categoria categoria = Obter(id);
                if (categoria == null) return;

                categoria.OperacaoCRUD = BaseEntity.OperacoesCRUD.Detele;

                DbContext.Categorias.Remove(categoria);
                DbContext.Entry(categoria).State = EntityState.Deleted;
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
