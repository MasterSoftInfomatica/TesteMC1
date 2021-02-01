using System;
using System.Linq;
using System.Web.Http;
using System.Collections.Generic;
using TesteMC1.Domain.Entity;
using TesteMC1.Domain.DTO;
using TesteMC1.Application.Services;

namespace TesteMC1.APIRESTful.Controllers
{
    public class UsuarioController : BaseApiController
    {
        [HttpGet, HttpPost]
        [Route("Usuario/CriarConta/{email},{nome},{sobrenome},{senha},{perfil}")]
        public IHttpActionResult CriarConta(string email, string nome, string sobrenome, string senha, string perfil)
        {
            try
            {
                Usuario usuario = ObterUsuario(null, email, nome, sobrenome, senha, perfil);
                _usuarioService.Incluir(usuario);
                
                return Json(new { Erro = false, Mensagem = "Usuário criado com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Usuario/GerarCodigoAtivacao/{email}")]
        public IHttpActionResult GerarNovoCodigoAtivacao(string email)
        {
            try
            {
                _usuarioService.GerarCodigoAtivacao(email);

                return Json(new { Erro = false, Mensagem = "Código de ativação gerado com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Usuario/ValidarCodigoAtivacao/{email},{codigoAtivacao}")]
        public IHttpActionResult ValidarCodigoAtivacao(string email, string codigoAtivacao)
        {
            try
            {
                _usuarioService.ValidarCodigoAtivacao(email, codigoAtivacao);

                return Json(new { Erro = false, Mensagem = "Código de ativação validado com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Usuario/Autenticar/{email},{senha}")]
        public IHttpActionResult Autenticar(string email, string senha)
        {
            try
            {
                string tokenSessao = _usuarioService.Autenticar(email, senha);

                return Json(new { Erro = false, Mensagem = "Usuário autenticado com sucesso!", Token = tokenSessao });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Usuario/AtualizarConta/{token},{email},{nome},{sobrenome}")]
        public IHttpActionResult AtualizarConta(string token, string email, string nome, string sobrenome)
        {
            try
            {
                //Obtem os dados do usuário do token de sessão
                ObterUsuarioSessao(token);

                //Ajusta as propriedades para atualziação
                Usuario usuario = _usuarioSessao;
                usuario.Email = email;
                usuario.Nome = nome;
                usuario.Sobrenome = sobrenome;
                
                _usuarioService.Atualizar(usuario);

                return Json(new { Erro = false, Mensagem = "Os dados da sua conta foram atualizados com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Usuario/AlterarSenha/{token},{senhaAtual},{novaSenha},{novaSenhaConfirmacao}")]
        public IHttpActionResult AlterarSenha(string token, string senhaAtual, string novaSenha, string novaSenhaConfirmacao)
        {
            try
            {
                //Obtem os dados do usuário do token de sessão
                ObterUsuarioSessao(token);

                _usuarioService.AlterarSenha(_usuarioSessao.Email, senhaAtual, novaSenha, novaSenhaConfirmacao);

                return Json(new { Erro = false, Mensagem = "Senha alterada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet]
        [Route("Usuario/ObterTodos/{token},{status}")]
        public IHttpActionResult ObterTodos(string token, string status)
        {
            try
            {
                //Valida se o usuário do token de sessão informado possui autorização necessária para utilizar esta funcionalidade
                List<Usuario.Perfis> Perfis = new List<Usuario.Perfis>() { Usuario.Perfis.AdministradorGeral };
                ValidarAutorizacao(token, Perfis);

                //Ajusta os parâmetros informados
                UsuarioService.Status statusPesquisa = _utilitariosService.ObterValorEnum<UsuarioService.Status>(status, "O status informado não é válido!");

                //Obtem os dados e retorna em formato Json
                List<Usuario> usuarios = _usuarioService.ObterTodos(statusPesquisa);

                return Json(usuarios);
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Usuario/Incluir/{token},{email},{nome},{sobrenome},{senha},{perfil}")]
        public IHttpActionResult Incluir(string token, string email, string nome, string sobrenome, string senha, string perfil)
        {
            try
            {
                //Valida se o usuário do token de sessão informado possui autorização necessária para utilizar esta funcionalidade
                List<Usuario.Perfis> Perfis = new List<Usuario.Perfis>() { Usuario.Perfis.AdministradorGeral };
                ValidarAutorizacao(token, Perfis);

                //Executa a ação
                Usuario usuario = ObterUsuario(null, email, nome, sobrenome, senha, perfil);
                _usuarioService.Incluir(usuario);

                return Json(new { Erro = false, Mensagem = "A inclusão dos dados foi processada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Usuario/Atualizar/{token},{id},{email},{nome},{sobrenome},{senha},{perfil}")]
        public IHttpActionResult Atualizar(string token, string id, string email, string nome, string sobrenome, string senha, string perfil)
        {
            try
            {
                //Valida se o usuário do token de sessão informado possui autorização necessária para utilizar esta funcionalidade
                List<Usuario.Perfis> Perfis = new List<Usuario.Perfis>() { Usuario.Perfis.AdministradorGeral };
                ValidarAutorizacao(token, Perfis);

                //Executa a ação
                Usuario usuario = ObterUsuario(id, email, nome, sobrenome, senha, perfil);
                _usuarioService.Atualizar(usuario);

                return Json(new { Erro = false, Mensagem = "A atualização dos dados foi processada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Usuario/Excluir/{token},{id}")]
        public IHttpActionResult Excluir(string token, string id)
        {
            try
            {
                //Valida se o usuário do token de sessão informado possui autorização necessária para utilizar esta funcionalidade
                List<Usuario.Perfis> Perfis = new List<Usuario.Perfis>() { Usuario.Perfis.AdministradorGeral };
                ValidarAutorizacao(token, Perfis);

                //Ajusta os parâmetros informados
                long idExclusao = _utilitariosService.ObterValorLong(id, "O código do usuário informado não é válido!");
                
                //Executa a ação
                _usuarioService.Excluir(idExclusao);

                return Json(new { Erro = false, Mensagem = "A exclusão dos dados foi processada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Usuario/Ativar/{token},{id}")]
        public IHttpActionResult Ativar(string token, string id)
        {
            try
            {
                //Valida se o usuário do token de sessão informado possui autorização necessária para utilizar esta funcionalidade
                List<Usuario.Perfis> Perfis = new List<Usuario.Perfis>() { Usuario.Perfis.AdministradorGeral };
                ValidarAutorizacao(token, Perfis);

                //Ajusta os parâmetros informados
                long idExcluxao = _utilitariosService.ObterValorLong(id, "O código do usuário informado não é válido!");
                
                //Executa a ação
                _usuarioService.Ativar(idExcluxao);

                return Json(new { Erro = false, Mensagem = "A exclusão dos dados foi processada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Usuario/Desativar/{token},{id}")]
        public IHttpActionResult Desativar(string token, string id)
        {
            try
            {
                //Valida se o usuário do token de sessão informado possui autorização necessária para utilizar esta funcionalidade
                List<Usuario.Perfis> Perfis = new List<Usuario.Perfis>() { Usuario.Perfis.AdministradorGeral };
                ValidarAutorizacao(token, Perfis);

                //Ajusta os parâmetros informados
                long idExcluxao = _utilitariosService.ObterValorLong(id, "O código do usuário informado não é válido!");

                //Executa a ação
                _usuarioService.Desativar(idExcluxao);

                return Json(new { Erro = false, Mensagem = "A exclusão dos dados foi processada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        private Usuario ObterUsuario(string id, string email, string nome, string sobrenome, string senha, string perfil)
        {
            try
            {
                Usuario usuario = new Usuario();

                long idUsuario = 0;
                if (!string.IsNullOrEmpty(id)) idUsuario = _utilitariosService.ObterValorLong(id, "O código do usuário informado não é válido!");

                usuario.Id = idUsuario;
                usuario.Email = _utilitariosService.ObterValorString(email);
                usuario.Nome = _utilitariosService.ObterValorString(nome);
                usuario.Sobrenome = _utilitariosService.ObterValorString(sobrenome);
                usuario.Senha = _utilitariosService.ObterValorString(senha);
                usuario.PerfilUsuario = _utilitariosService.ObterValorEnum<Usuario.Perfis>(perfil, "O perfil do usuário informado não é válido!", true);

                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}