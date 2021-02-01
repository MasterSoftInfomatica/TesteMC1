using System;
using System.Globalization;
using System.Collections.Generic;

namespace TesteMC1.Domain
{
    public class BaseDomain : Base
    {
        protected string MensagemErroPadraoPropriedadeObrigatoria { get { return "O campo '{0}' é de preenchimento obrigatório e não pode ser nulo!"; } }
        protected string MensagemErroPadraoValorInvalido { get { return "O valor informado no campo '{0}' é inválido!"; } }
        protected List<string> MensagensErroValidacao { get; set; }

        public void AjustarPropriedades()
        {
        }

        public bool PossuiErrosValidacao()
        {
            //Clear all existing error messages
            MensagensErroValidacao = new List<string>();

            //Validate the properties

            return MensagensErroValidacao.Count > 0;
        }

        public string ObterMensagensErrosValidacao()
        {
            if (MensagensErroValidacao == null) return null;
            if (MensagensErroValidacao.Count == 0) return null;

            string mensagemErroValidacaoSingular = "Um erro ocorreu durante a validação dos dados: ";
            string mensagemErroValidacaoPlural = "Os seguintes erros ocorreram durante a validação dos dados:";
            string completeMessage;
            string partialMessage;

            if (MensagensErroValidacao.Count == 1)
            {
                completeMessage = mensagemErroValidacaoSingular;
                partialMessage = "{0}";
            }
            else
            {
                completeMessage = mensagemErroValidacaoPlural + Environment.NewLine;
                partialMessage = "* {0}" + Environment.NewLine;
            }

            foreach (var errorMessage in MensagensErroValidacao)
            {
                completeMessage += string.Format(partialMessage, errorMessage);
            }

            return completeMessage;
        }
    }
}
