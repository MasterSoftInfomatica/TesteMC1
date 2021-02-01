using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TesteMC1.Domain.Entity
{
    public class Categoria : BaseEntity
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public string IdDescricao { get { return Id <= 0 | string.IsNullOrEmpty(Descricao) ? null : Id.ToString() + " - " + Descricao; } }
        public bool EstaAtiva { get; set; }
        public string DescricaoEstaAtiva { get { return EstaAtiva ? "Sim" : "Não"; } }

        public virtual List<Produto> Produtos { get; set; }

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

            return (MensagensErroValidacao.Count > 0);
        }
    }
}