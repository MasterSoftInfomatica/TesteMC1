using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TesteMC1.Domain.Entity
{
    public class Produto : BaseEntity
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public string IdDescricao { get { return Id <= 0 | string.IsNullOrEmpty(Descricao) ? null : Id.ToString() + " - " + Descricao; } }
        public long IdCategoria { get; set; }
        public string CodigoInterno { get; set; }
        public string CodigoInternoDescricao { get { return string.IsNullOrEmpty(CodigoInterno) | string.IsNullOrEmpty(Descricao) ? null : CodigoInterno + " - " + Descricao; } }
        public string CodigoBarras { get; set; }
        public string CodigoBarrasDescricao { get { return string.IsNullOrEmpty(CodigoBarras) | string.IsNullOrEmpty(Descricao) ? null : CodigoBarras + " - " + Descricao; } }
        public string UnidadeMedida { get; set; }
        public decimal QtdEstoque { get; set; }
        public decimal ValorUnitarioCusto { get; set; }
        public decimal ValorTotalCusto { get { return QtdEstoque * ValorUnitarioCusto; } }
        public decimal ValorUnitarioVenda { get; set; }
        public decimal ValorTotalVenda { get { return QtdEstoque * ValorUnitarioVenda; } }
        public DateTime? DataUltimaMovimentacao { get; set; }
        public bool EstaAtivo { get; set; }
        public string DescricaoEstaAtivo { get { return EstaAtivo ? "Sim" : "Não"; } }

        public virtual Categoria Categoria { get; set; }
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
            if (string.IsNullOrEmpty(Descricao)) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Descrição"));
            if (IdCategoria <= 0) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Código da Categoria"));
            if (string.IsNullOrEmpty(UnidadeMedida)) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Unidade de Medida"));
            if (ValorTotalCusto < 0) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoValorInvalido, "Valor Unitário de Custo"));
            if (ValorUnitarioVenda < 0) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoValorInvalido, "Valor Unitário de Venda"));

            return (MensagensErroValidacao.Count > 0);
        }
    }
}