using System;
using System.Linq;
using System.Web.Http;
using System.Collections.Generic;
using TesteMC1.Domain.Entity;
using TesteMC1.Domain.DTO;
using TesteMC1.Application.Services;

namespace TesteMC1.APIRESTful.Controllers
{
    public class BaseApiController : ApiController
    {
        protected UtilitariosService _utilitariosService;
        protected UsuarioService _usuarioService;

        protected Usuario _usuarioSessao;

        public BaseApiController()
        {
            _utilitariosService = new UtilitariosService();
            _usuarioService = new UsuarioService();
        }

        protected void ObterUsuarioSessao(string tokenSessao)
        {
            try
            {
                _usuarioSessao = _usuarioService.Obter(tokenSessao, UsuarioService.PorTokenSessao.PorTokenSessao);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ValidarAutorizacao(string tokenSessao, List<Usuario.Perfis> perfis)
        {
            try
            {
                ObterUsuarioSessao(tokenSessao);
                if (!perfis.Any(w => w == _usuarioSessao.PerfilUsuario)) throw new Exception("Acesso negado! Você não está autorizado a acessar esta funcionalidade do aplicativo!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}