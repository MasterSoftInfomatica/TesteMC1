using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using TesteMC1.Domain.Entity;
using TesteMC1.Domain.DTO;

namespace TesteMC1.Application.Services
{
    public class UsuarioService : BaseService, IDisposable
    {
        CriptografiaService _criptografiaService;
        EmailSmtpService _emailSmtpService;

        public UsuarioService()
        {
            _criptografiaService = new CriptografiaService();
            _emailSmtpService = new EmailSmtpService();
        }

        private readonly string dados1 = "_Cr1pt0gr4f14#!_";
        private readonly string dados2 = "#T0k3n!*S3ss40!#";

        public enum Status { Ativos, Inativos, Todos }
        public enum PorId { PorId }
        public enum PorEmail { PorEmail }
        public enum PorTokenSessao { PorTokenSessao }

        public Usuario Obter(long id, PorId tipoPesquisa)
        {
            try
            {
                return DbContext.Usuarios.FirstOrDefault(w => w.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Usuario Obter(string email, PorEmail tipoPesquisa)
        {
            try
            {
                return DbContext.Usuarios.FirstOrDefault(w => w.Email == email.ToLower());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Usuario Obter(string tokenSessao, PorTokenSessao tipoPesquisa)
        {
            try
            {
                TokenDTO token = ObterDadosTokenSessao(tokenSessao);

                return DbContext.Usuarios.FirstOrDefault(w => w.Email == token.Email.ToLower());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PerfilUsuarioDTO> ObterTodosPerfis()
        {
            List<PerfilUsuarioDTO> perfisUsuario = new List<PerfilUsuarioDTO>();
            Usuario usuario = new Usuario();

            usuario.PerfilUsuario = Usuario.Perfis.AdministradorGeral;
            perfisUsuario.Add(new PerfilUsuarioDTO() { Perfil = Usuario.Perfis.AdministradorGeral, Descricao = usuario.DescricaoPerfil });
            usuario.PerfilUsuario = Usuario.Perfis.AdministradorEstoque;
            perfisUsuario.Add(new PerfilUsuarioDTO() { Perfil = Usuario.Perfis.AdministradorEstoque, Descricao = usuario.DescricaoPerfil });
            usuario.PerfilUsuario = Usuario.Perfis.ConsultasRelatorios;
            perfisUsuario.Add(new PerfilUsuarioDTO() { Perfil = Usuario.Perfis.ConsultasRelatorios, Descricao = usuario.DescricaoPerfil });

            return perfisUsuario;
        }

        public List<Usuario> ObterTodos(Status status)
        {
            try
            {
                if (status == Status.Todos)
                {
                    return DbContext.Usuarios.OrderBy(o => o.Nome).ThenBy(o => o.Sobrenome).ToList();
                }
                else
                {
                    bool statusPesquisa = status == Status.Ativos;

                    return DbContext.Usuarios.Where(w => w.EstaAtivo == statusPesquisa).OrderBy(o => o.Nome).ThenBy(o => o.Sobrenome).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long Incluir(Usuario usuario)
        {
            try
            {
                usuario.OperacaoCRUD = BaseEntity.OperacoesCRUD.Create;

                usuario.AjustarPropriedades();
                if (usuario.PossuiErrosValidacao()) throw new Exception(usuario.ObterMensagensErrosValidacao());

                //Valida se o endereço de e-mail informado já existe no banco de dados
                if (DbContext.Usuarios.Any(w => w.Email == usuario.Email)) throw new Exception(string.Format("O endereço de e-mail '{0}' já existe em nosso cadastro!", usuario.Email));

                //Criptografa a senha antes da gravação
                usuario.Senha = _criptografiaService.Criptografar(usuario.Senha, dados1);

                DbContext.Usuarios.Add(usuario);
                DbContext.Entry(usuario).State = EntityState.Added;
                DbContext.SaveChanges();

                return usuario.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Atualizar(Usuario usuario)
        {
            try
            {
                Usuario usuarioExistente = Obter(usuario.Id, PorId.PorId);
                if (usuarioExistente == null) return;

                if (usuarioExistente.Email != usuario.Email.ToLower())
                {
                    //Valida se o endereço de e-mail informado já existe no cadadstro de usuários
                    if (DbContext.Usuarios.Any(w => w.Email == usuario.Email.ToLower())) throw new Exception(string.Format("O endereço de e-mail '{0}' já existe em nosso cadastro!", usuario.Email));
                }

                usuarioExistente.OperacaoCRUD = BaseEntity.OperacoesCRUD.Update;

                usuarioExistente.Nome = usuario.Nome;
                usuarioExistente.Sobrenome = usuario.Sobrenome;
                usuarioExistente.Email = usuario.Email;
                usuarioExistente.Perfil = usuario.Perfil;

                usuario.AjustarPropriedades();
                if (usuarioExistente.PossuiErrosValidacao()) throw new Exception(usuarioExistente.ObterMensagensErrosValidacao());

                DbContext.Usuarios.Attach(usuarioExistente);
                DbContext.Entry(usuarioExistente).State = EntityState.Modified;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Excluir(Usuario usuario)
        {
            try
            {
                if (usuario == null) return;
                Excluir(usuario.Id);
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
                Usuario usuario = Obter(id, PorId.PorId);
                if (usuario == null) return;

                usuario.OperacaoCRUD = BaseEntity.OperacoesCRUD.Detele;

                DbContext.Usuarios.Remove(usuario);
                DbContext.Entry(usuario).State = EntityState.Deleted;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GerarCodigoAtivacao(string email)
        {
            try
            {
                Usuario usuarioExistente = Obter(email, PorEmail.PorEmail);
                if (usuarioExistente == null) throw new Exception(string.Format("O endereço de e-mail '{0}' não foi encontrado em nosso cadastro!", email.ToLower()));

                usuarioExistente.OperacaoCRUD = BaseEntity.OperacoesCRUD.Update;

                //Cria o código de ativação a ser enviado via e-mail
                Random random = new Random();
                usuarioExistente.CodigoAtivacao = random.Next(1, 99).ToString("00", Cultura);
                usuarioExistente.CodigoAtivacao += random.Next(1, 99).ToString("00", Cultura);
                usuarioExistente.CodigoAtivacao += random.Next(1, 99).ToString("00", Cultura);

                //Atualiza as datas de controle para ativação da conta do usuário
                usuarioExistente.DataCriacaoCodigoAtivacao = DateTime.Now;
                usuarioExistente.DataValidadeCodigoAtivacao = usuarioExistente.DataCriacaoCodigoAtivacao.Value.AddDays(1);
                usuarioExistente.DataAtivacao = null;
                usuarioExistente.EstaAtivo = false;

                DbContext.Usuarios.Attach(usuarioExistente);
                DbContext.Entry(usuarioExistente).State = EntityState.Modified;
                DbContext.SaveChanges();

                //Envia a mensagem de e-mail com o código de ativação criado
                _emailSmtpService.EnviarMensagem(Properties.Settings.Default.EnderecoEmailDe, email, "", "Teste MC1 - Chave de Ativação de Conta de Usuário", "Recentemente você criou uma conta no aplicativo 'Teste MC1' estamos enviando o código de ativação desta conta: " + usuarioExistente.CodigoAtivacao);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ValidarCodigoAtivacao(string email, string codigoAtivacao)
        {
            try
            {
                Usuario usuarioExistente = Obter(email, PorEmail.PorEmail);

                if (usuarioExistente == null) throw new Exception(string.Format("O endereço de e-mail '{0}' não foi encontrado em nosso cadastro!", email.ToLower()));

                //Valida se existe algum código de ativação cadastrado
                if (string.IsNullOrEmpty(usuarioExistente.CodigoAtivacao))
                {
                    GerarCodigoAtivacao(email);
                    throw new Exception(string.Format("Nenhum código de ativação foi encontrado para o endereço de e-mail '{0} em nosso cadastro'! Um novo código de ativação foi enviado para sua caixa de e-mail!", email.ToLower()));
                }

                //Valida se a data de validade do código de ativação está expirada
                if (usuarioExistente.DataValidadeCodigoAtivacao.Value < DateTime.Now)
                {
                    GerarCodigoAtivacao(email);
                    throw new Exception(string.Format("O código de ativação para o endereço de e-mail '{0} está expirado! Um novo código de ativação foi enviado para sua caixa de e-mail!", email.ToLower()));
                }

                //Valida se o código de ativação informado está correto
                if (usuarioExistente.CodigoAtivacao != codigoAtivacao) throw new Exception("O código de ativação informado não está correto!");

                usuarioExistente.OperacaoCRUD = BaseEntity.OperacoesCRUD.Update;

                //Atualiza as datas de controle para ativação da conta do usuário
                usuarioExistente.DataAtivacao = DateTime.Now;
                usuarioExistente.EstaAtivo = true;

                DbContext.Usuarios.Attach(usuarioExistente);
                DbContext.Entry(usuarioExistente).State = EntityState.Modified;
                DbContext.SaveChanges();

                //Envia a mensagem de e-mail confirmando a ativação da conta do usuário
                _emailSmtpService.EnviarMensagem(Properties.Settings.Default.EnderecoEmailDe, email, "", "Teste MC1 - Ativação de Conta de Usuário", "A sua conta de usuário foi ativada com sucesso no aplicativo 'Teste MC1'!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Autenticar(string email, string senha)
        {
            try
            {
                Usuario usuarioExistente = Obter(email, PorEmail.PorEmail);
                if (usuarioExistente == null) throw new Exception(string.Format("O endereço de e-mail '{0}' não foi encontrado em nosso cadastro!", email.ToLower()));
                if (!usuarioExistente.EstaAtivo) throw new Exception(string.Format("O endereço de e-mail '{0}' não está ativo em nosso cadastro!", email.ToLower()));
                if (_criptografiaService.Descriptografar(usuarioExistente.Senha, dados1) != senha) throw new Exception("A senha informada não é válida!");

                //Gerar o token de sessão a ser retornada para o aplicativo cliente
                return GerarTokenSessao(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AlterarSenha(string email, string senhaAtual, string novaSenha, string novaSenhaConfirmacao)
        {
            try
            {
                Usuario usuarioExistente = Obter(email, PorEmail.PorEmail);
                if (usuarioExistente == null) throw new Exception(string.Format("O endereço de e-mail '{0}' não foi encontrado em nosso cadastro!", email.ToLower()));

                usuarioExistente.OperacaoCRUD = BaseEntity.OperacoesCRUD.Update;

                //Valida se a senha atual informada está correta
                if (_criptografiaService.Descriptografar(usuarioExistente.Senha, dados1) != senhaAtual) throw new Exception("A senha atual informada não é válida!");

                //Valida se a nova senha é igual à confirmação
                if (novaSenha != novaSenhaConfirmacao) throw new Exception("A nova senha informada está diferente na confirmação!");

                //Criptografa a senha antes da gravação
                usuarioExistente.Senha = _criptografiaService.Criptografar(usuarioExistente.Senha, dados1);

                DbContext.Usuarios.Attach(usuarioExistente);
                DbContext.Entry(usuarioExistente).State = EntityState.Modified;
                DbContext.SaveChanges();

                //Enviar a mensagem de e-mail notificando sobre o evento de alteração de senha
                _emailSmtpService.EnviarMensagem(Properties.Settings.Default.EnderecoEmailDe, email, "", "Teste MC1 - Alteração da Senha da sua Conta de Usuário", "Esta é uma mensagem informativa para notificar que sua conta de usuário no aplicativo 'Teste MC1' teve a senha alterada! Caso você não tenho realizado esta alteração de senhas, entre em contato como administrador do sistema!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Ativar(long id)
        {
            try
            {
                Usuario usuarioExistente = Obter(id, PorId.PorId);
                if (usuarioExistente == null) return;

                usuarioExistente.OperacaoCRUD = BaseEntity.OperacoesCRUD.Update;

                usuarioExistente.EstaAtivo = true;

                DbContext.Usuarios.Attach(usuarioExistente);
                DbContext.Entry(usuarioExistente).State = EntityState.Modified;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Desativar(long id)
        {
            try
            {
                Usuario usuarioExistente = Obter(id, PorId.PorId);
                if (usuarioExistente == null) return;

                usuarioExistente.OperacaoCRUD = BaseEntity.OperacoesCRUD.Update;

                usuarioExistente.EstaAtivo = false;

                DbContext.Usuarios.Attach(usuarioExistente);
                DbContext.Entry(usuarioExistente).State = EntityState.Modified;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GerarTokenSessao(string email)
        {
            try
            {
                //Cria o token a ser criptografado
                string tokenSessao1 = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy HH:mm:ss", Cultura) + ";";
                tokenSessao1 += email + ";";
                tokenSessao1 += DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", Cultura);

                //Criptografa o token
                string tokenSessao2 = _criptografiaService.Criptografar(tokenSessao1, dados1);
                string tokenSessao3 = _criptografiaService.Criptografar(tokenSessao2, dados2);

                return tokenSessao3;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private TokenDTO ObterDadosTokenSessao(string tokenSessao)
        {
            try
            {
                string dadosToken1;
                string dadosToken2;

                try
                {
                    //Descriptografa o token
                    dadosToken1 = _criptografiaService.Descriptografar(tokenSessao, dados2);
                    dadosToken2 = _criptografiaService.Descriptografar(dadosToken1, dados1);
                }
                catch (Exception ex)
                {
                    throw new Exception("O token de sessão informado não é válido!", ex);
                }

                //Extrai os dados contidos no token e retorna para utilização
                List<string> dadosTokenRetorno = dadosToken2.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList().Distinct().ToList();

                TokenDTO token = new TokenDTO();
                token.DataValidade = Convert.ToDateTime(dadosTokenRetorno[0], Cultura);
                token.Email = dadosTokenRetorno[1];
                token.DataCriacao = Convert.ToDateTime(dadosTokenRetorno[2], Cultura);

                if (!token.SessaoValida) throw new Exception("O token de sessão informado está expirado!");

                return token;
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

            _criptografiaService = null;
            _emailSmtpService = null;
        }
    }
}
