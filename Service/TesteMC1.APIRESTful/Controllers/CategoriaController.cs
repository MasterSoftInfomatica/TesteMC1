using System;
using System.Linq;
using System.Web.Http;
using System.Collections.Generic;
using TesteMC1.Domain.Entity;
using TesteMC1.Domain.DTO;
using TesteMC1.Application.Services;

namespace TesteMC1.APIRESTful.Controllers
{
    public class CategoriaController : BaseApiController
    {
        CategoriaService _categoriaService;

        public CategoriaController()
        {
            _categoriaService = new CategoriaService();
        }

        [HttpGet]
        [Route("Categoria/ObterTodas/{token},{status}")]
        public IHttpActionResult ObterTodas(string token, string status)
        {
            try
            {
                //Valida se o usuário do token de sessão informado possui autorização necessária para utilizar esta funcionalidade
                List<Usuario.Perfis> Perfis = new List<Usuario.Perfis>() { Usuario.Perfis.AdministradorGeral, Usuario.Perfis.AdministradorEstoque };
                ValidarAutorizacao(token, Perfis);

                //Ajusta os parâmetros informados
                CategoriaService.Status statusPesquisa = _utilitariosService.ObterValorEnum<CategoriaService.Status>(status, "O status informado não é válido!");
                
                //Obtem os dados e retorna em formato Json
                List<Categoria> categorias = _categoriaService.ObterTodas(statusPesquisa);

                return Json(categorias);
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Categoria/Incluir/{token},{Descricao},{EstaAtiva}")]
        public IHttpActionResult Incluir(string token, string descricao, string estaAtiva)
        {
            try
            {
                //Valida se o usuário do token de sessão informado possui autorização necessária para utilizar esta funcionalidade
                List<Usuario.Perfis> Perfis = new List<Usuario.Perfis>() { Usuario.Perfis.AdministradorGeral, Usuario.Perfis.AdministradorEstoque };
                ValidarAutorizacao(token, Perfis);

                //Executa a ação
                Categoria categoria = ObterCategoria(null, descricao, estaAtiva);
                _categoriaService.Incluir(categoria);

                return Json(new { Erro = false, Mensagem = "A inclusão dos dados foi processada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Categoria/Atualizar/{token},{id},{Descricao},{EstaAtiva}")]
        public IHttpActionResult Atualizar(string token, string id, string descricao, string estaAtiva)
        {
            try
            {
                //Valida se o usuário do token de sessão informado possui autorização necessária para utilizar esta funcionalidade
                List<Usuario.Perfis> Perfis = new List<Usuario.Perfis>() { Usuario.Perfis.AdministradorGeral, Usuario.Perfis.AdministradorEstoque };
                ValidarAutorizacao(token, Perfis);

                //Executa a ação
                Categoria categoria = ObterCategoria(id, descricao, estaAtiva);
                _categoriaService.Atualizar(categoria);

                return Json(new { Erro = false, Mensagem = "A atualização dos dados foi processada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Categoria/Excluir/{token},{id}")]
        public IHttpActionResult Excluir(string token, string id)
        {
            try
            {
                //Valida se o usuário do token de sessão informado possui autorização necessária para utilizar esta funcionalidade
                List<Usuario.Perfis> Perfis = new List<Usuario.Perfis>() { Usuario.Perfis.AdministradorGeral, Usuario.Perfis.AdministradorEstoque };
                ValidarAutorizacao(token, Perfis);

                //Ajusta os parâmetros informados
                long idExclusao = _utilitariosService.ObterValorLong(id, "O código do usuário informado não é válido!");
                
                //Executa a ação
                _categoriaService.Excluir(idExclusao);

                return Json(new { Erro = false, Mensagem = "A exclusão dos dados foi processada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        private Categoria ObterCategoria(string id, string descricao, string estaAtiva)
        {
            try
            {
                Categoria categoria = new Categoria();

                long idCategoria = 0;
                if (!string.IsNullOrEmpty(id)) idCategoria = _utilitariosService.ObterValorLong(id, "O código da categoria informado não é válido!");

                categoria.Id = idCategoria;
                categoria.Descricao = _utilitariosService.ObterValorString(descricao);
                categoria.EstaAtiva = _utilitariosService.ObterValorBoolean(estaAtiva, "O flag de status informado não é válido!");

                return categoria;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}