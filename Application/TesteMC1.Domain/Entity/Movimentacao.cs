using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TesteMC1.Domain.Entity
{
    public class Movimentacao : BaseEntity
    {
        public enum TiposMovimentacao { Entrada, Saida }

        public long Id { get; set; }
        public DateTime Data { get; set; }
        public string Tipo { get; set; }
        public TiposMovimentacao TipoMovimentacao
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(Tipo)) return TiposMovimentacao.Entrada;
                    return (TiposMovimentacao)Enum.Parse(typeof(TiposMovimentacao), Tipo, true);
                }
                catch (Exception)
                {
                    return TiposMovimentacao.Entrada;
                }
            }
            set
            {
                Tipo = value.ToString();
            }
        }
        public long IdFornecedor { get; set; }
        public long IdCliente { get; set; }

        public virtual List<MovimentacaoItem> MovimentacoesItens { get; set; }

        public new void AjustarPropriedades()
        {
        }

        public new bool PossuiErrosValidacao()
        {
            //Clear all existing error messages
            MensagensErroValidacao = new List<string>();

            //Validate the properties
            if (OperacaoCRUD == OperacoesCRUD.Update)
            {
                if (Id <= 0) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Id"));
            }
            if (Data == new DateTime()) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Data"));
            if (string.IsNullOrEmpty(Tipo)) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Tipo"));
            if (TipoMovimentacao == TiposMovimentacao.Entrada)
            {
                if (IdFornecedor <= 0) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Código do Fornecedor"));
            }
            else
            {
                if (IdCliente <= 0) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Código do Cliente"));
            }

            return (MensagensErroValidacao.Count > 0);
        }
    }
}