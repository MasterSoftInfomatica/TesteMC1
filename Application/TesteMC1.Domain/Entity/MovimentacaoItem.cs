using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TesteMC1.Domain.Entity
{
    public class MovimentacaoItem : BaseEntity
    {
        public long IdMovimentacao { get; set; }
        public int NumeroItem { get; set; }
        public long IdProduto { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get { return Quantidade * ValorUnitario; } }

        public virtual Movimentacao Movimentacao { get; set; }
        public virtual Produto Produto { get; set; }

        public new void AjustarPropriedades()
        {
        }

        public new bool PossuiErrosValidacao()
        {
            //Clear all existing error messages
            MensagensErroValidacao = new List<string>();

            //Validate the properties
            if (IdMovimentacao <= 0) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Número da Movimentação"));
            if (NumeroItem <= 0) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Número do Item"));
            if (IdProduto <= 0) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Código do Produto"));
            if (Quantidade <= 0) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Quantidade"));
            if (ValorUnitario <= 0) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Valor Unitário"));

            return (MensagensErroValidacao.Count > 0);
        }
    }
}