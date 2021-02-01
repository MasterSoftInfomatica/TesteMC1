using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TesteMC1.Domain.Entity
{
    public class Usuario : BaseEntity
    {
        private string[] _descricoesPerfis = { "Administrador Geral", "Administrador de Estoque", "Consultas / Relatórios" };

        public enum Perfis {AdministradorGeral, AdministradorEstoque, ConsultasRelatorios }

        public long Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string NomeCompleto { get { return string.IsNullOrEmpty(Nome) | string.IsNullOrEmpty(Sobrenome) ? null : Nome + " " + Sobrenome; } }
        public string Email { get; set; }
        public string Senha { get; set; }
        public DateTime DataCriacao { get; set; }
        public string CodigoAtivacao { get; set; }
        public DateTime? DataCriacaoCodigoAtivacao { get; set; }
        public DateTime? DataValidadeCodigoAtivacao { get; set; }
        public DateTime? DataAtivacao { get; set; }
        public string Perfil { get; set; }
        public Perfis PerfilUsuario
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(Perfil)) return Perfis.ConsultasRelatorios;
                    return (Perfis)Enum.Parse(typeof(Perfis), Perfil, true);
                }
                catch (Exception)
                {
                    return Perfis.ConsultasRelatorios;
                }
            }
            set
            {
                Perfil = value.ToString();
            }
        }
        public string DescricaoPerfil { get { return _descricoesPerfis[Convert.ToInt32(PerfilUsuario)]; } }
        public bool EstaAtivo { get; set; }
        public string DescricaoEstaAtivo { get { return EstaAtivo ? "Sim" : "Não"; } }

        public new void AjustarPropriedades()
        {
            Email = Email.ToLower();
            
            if (OperacaoCRUD == OperacoesCRUD.Create)
            {
                DataCriacao = DateTime.Now;
                CodigoAtivacao = null;
                DataCriacaoCodigoAtivacao = null;
                DataValidadeCodigoAtivacao = null;
                DataAtivacao = null;
                EstaAtivo = false;
            }
        }

        public new bool PossuiErrosValidacao()
        {
            //Clear all existing error messages
            MensagensErroValidacao = new List<string>();

            //Validate the properties
            if (string.IsNullOrEmpty(Nome)) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Nome"));
            if (string.IsNullOrEmpty(Sobrenome)) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Sobrenome"));
            if (string.IsNullOrEmpty(Email)) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "E-mail"));
            if (string.IsNullOrEmpty(Senha)) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Senha"));
            if (OperacaoCRUD == OperacoesCRUD.Create)
            {
                if (DataCriacao == new DateTime()) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Data de Criação"));
            }
            else if (OperacaoCRUD == OperacoesCRUD.Update)
            {
                if (Id <= 0) MensagensErroValidacao.Add(string.Format(MensagemErroPadraoPropriedadeObrigatoria, "Id"));
            }

            return (MensagensErroValidacao.Count > 0);
        }
    }
}
