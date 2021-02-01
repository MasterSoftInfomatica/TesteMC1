using System;
using System.Linq;
using System.Web.Http;
using System.Collections.Generic;
using TesteMC1.Domain.Entity;
using TesteMC1.Domain.DTO;
using TesteMC1.Application.Services;

namespace TesteMC1.APIRESTful.Controllers
{
    public class ProdutoController : BaseApiController
    {
        ProdutoService _produtoService;

        public ProdutoController()
        {
            _produtoService = new ProdutoService();
        }

        [HttpGet]
        [Route("Produto/ObterTodos/{token},{status}")]
        public IHttpActionResult ObterTodos(string token, string status)
        {
            try
            {
                //Valida se o usuário do token de sessão informado possui autorização necessária para utilizar esta funcionalidade
                List<Usuario.Perfis> Perfis = new List<Usuario.Perfis>() { Usuario.Perfis.AdministradorGeral, Usuario.Perfis.AdministradorEstoque };
                ValidarAutorizacao(token, Perfis);

                //Ajusta os parâmetros informados
                ProdutoService.Status statusPesquisa = _utilitariosService.ObterValorEnum<ProdutoService.Status>(status, "O status informado não é válido!");
                
                //Obtem os dados e retorna em formato Json
                List<Produto> produtos = _produtoService.ObterTodos(statusPesquisa);

                return Json(produtos);
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet]
        [Route("Produto/ObterTodos/{token},{idCategoria},{status}")]
        public IHttpActionResult ObterTodos(string token, string idCategoria, string status)
        {
            try
            {
                //Valida se o usuário do token de sessão informado possui autorização necessária para utilizar esta funcionalidade
                List<Usuario.Perfis> Perfis = new List<Usuario.Perfis>() { Usuario.Perfis.AdministradorGeral, Usuario.Perfis.AdministradorEstoque };
                ValidarAutorizacao(token, Perfis);

                //Ajusta os parâmetros informados
                ProdutoService.Status statusPesquisa = _utilitariosService.ObterValorEnum<ProdutoService.Status>(status, "O status informado não é válido!");
                long idCategoriaPesquisa = _utilitariosService.ObterValorLong(idCategoria, "O código da categoria informado não é válido!");
                
                //Obtem os dados e retorna em formato Json
                List<Produto> produtos = _produtoService.ObterTodos(idCategoriaPesquisa, statusPesquisa);

                return Json(produtos);
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Produto/Incluir/{token},{descricao},{idCategoria},{codigoInterno},{codigoBarras},{unidadeMedida},{qtdEstoque},{valorUnitarioCusto},{valorUnitarioVenda},{estaAtivo}")]
        public IHttpActionResult Incluir(string token, string descricao, string idCategoria, string codigoInterno, string codigoBarras, string unidadeMedida, string qtdEstoque, string valorUnitarioCusto, string valorUnitarioVenda, string estaAtivo)
        {
            try
            {
                //Valida se o usuário do token de sessão informado possui autorização necessária para utilizar esta funcionalidade
                List<Usuario.Perfis> Perfis = new List<Usuario.Perfis>() { Usuario.Perfis.AdministradorGeral, Usuario.Perfis.AdministradorEstoque };
                ValidarAutorizacao(token, Perfis);

                //Executa a ação
                Produto produto = ObterProduto(null, descricao, idCategoria, codigoInterno, codigoBarras, unidadeMedida, qtdEstoque, valorUnitarioCusto, valorUnitarioVenda, estaAtivo);
                _produtoService.Incluir(produto);

                return Json(new { Erro = false, Mensagem = "A inclusão dos dados foi processada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Produto/Atualizar/{token},{id},{descricao},{idCategoria},{codigoInterno},{codigoBarras},{unidadeMedida},{qtdEstoque},{valorUnitarioCusto},{valorUnitarioVenda},{estaAtivo}")]
        public IHttpActionResult Atualizar(string token, string id, string descricao, string idCategoria, string codigoInterno, string codigoBarras, string unidadeMedida, string qtdEstoque, string valorUnitarioCusto, string valorUnitarioVenda, string estaAtivo)
        {
            try
            {
                //Valida se o usuário do token de sessão informado possui autorização necessária para utilizar esta funcionalidade
                List<Usuario.Perfis> Perfis = new List<Usuario.Perfis>() { Usuario.Perfis.AdministradorGeral, Usuario.Perfis.AdministradorEstoque };
                ValidarAutorizacao(token, Perfis);

                //Executa a ação
                Produto produto = ObterProduto(id, descricao, idCategoria, codigoInterno, codigoBarras, unidadeMedida, qtdEstoque, valorUnitarioCusto, valorUnitarioVenda, estaAtivo);
                _produtoService.Atualizar(produto);

                return Json(new { Erro = false, Mensagem = "A atualização dos dados foi processada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Produto/AtualizarMovimentacao/{token},{id},{tipo},{quantidade},{valorUnitarioCusto}")]
        public IHttpActionResult AtualizarMovimentacao(string token, string id, string tipo, string quantidade, string valorUnitarioCusto)
        {
            try
            {
                //Valida se o usuário do token de sessão informado possui autorização necessária para utilizar esta funcionalidade
                List<Usuario.Perfis> Perfis = new List<Usuario.Perfis>() { Usuario.Perfis.AdministradorGeral, Usuario.Perfis.AdministradorEstoque };
                ValidarAutorizacao(token, Perfis);

                //Ajusta os parâmetros informados
                long idAtualizacao = _utilitariosService.ObterValorLong(id, "O código do produto informado não é válido!");
                Movimentacao.TiposMovimentacao tipoAtualizacao = _utilitariosService.ObterValorEnum<Movimentacao.TiposMovimentacao>(tipo, "O tipo informado não é válido!");
                decimal quantidadeAtualizacao = _utilitariosService.ObterValorDecimal(quantidade, "A quantidade informada não é válida!");
                decimal valorUnitarioCustoAtualizacao = _utilitariosService.ObterValorDecimal(valorUnitarioCusto, "O valor unitário de custo informado não é válido!");

                //Executa a ação
                _produtoService.AtualizarMovimentacao(idAtualizacao, tipoAtualizacao, quantidadeAtualizacao, valorUnitarioCustoAtualizacao);

                return Json(new { Erro = false, Mensagem = "A atualização dos dados foi processada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Produto/AtualizarValorVenda/{token},{idCategoria},{percentualCorrecao}")]
        public IHttpActionResult AtualizarValorVenda(string token, string idCategoria, string percentualCorrecao)
        {
            try
            {
                //Valida se o usuário do token de sessão informado possui autorização necessária para utilizar esta funcionalidade
                List<Usuario.Perfis> Perfis = new List<Usuario.Perfis>() { Usuario.Perfis.AdministradorGeral, Usuario.Perfis.AdministradorEstoque };
                ValidarAutorizacao(token, Perfis);

                //Ajusta os parâmetros informados
                long idCategoriaAtualizacao = _utilitariosService.ObterValorLong(idCategoria, "O código da categoria informado não é válido!");
                decimal percentualCorrecaoAtualizacao = _utilitariosService.ObterValorDecimal(percentualCorrecao, "O valor unitário de custo informado não é válido!");

                //Executa a ação
                _produtoService.AtualizarValorVenda(idCategoriaAtualizacao, percentualCorrecaoAtualizacao);

                return Json(new { Erro = false, Mensagem = "A atualização dos dados foi processada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        [HttpGet, HttpPost]
        [Route("Produto/Excluir/{token},{id}")]
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
                _produtoService.Excluir(idExclusao);

                return Json(new { Erro = false, Mensagem = "A exclusão dos dados foi processada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = _utilitariosService.ObterMensagemErroDetalhada(ex), TipoException = ex.GetType().ToString(), StackTrace = ex.StackTrace });
            }
        }

        private Produto ObterProduto(string id, string descricao, string idCategoria, string codigoInterno, string codigoBarras, string unidadeMedida, string qtdEstoque, string valorUnitarioCusto, string valorUnitarioVenda, string estaAtivo)
        {
            try
            {
                Produto produto = new Produto();

                long idProduto = 0;
                if (!string.IsNullOrEmpty(id)) idProduto = _utilitariosService.ObterValorLong(id, "O código do produto informado não é válido!");

                produto.Id = idProduto;
                produto.Descricao = _utilitariosService.ObterValorString(descricao);
                produto.IdCategoria = _utilitariosService.ObterValorLong(idCategoria, "O código da categoria informado não é válido!");
                produto.CodigoInterno = _utilitariosService.ObterValorString(codigoInterno);
                produto.CodigoBarras = _utilitariosService.ObterValorString(codigoBarras);
                produto.UnidadeMedida = _utilitariosService.ObterValorString(unidadeMedida);
                produto.QtdEstoque = _utilitariosService.ObterValorDecimal(qtdEstoque, "A quantidade em estoque informada não é válida!");
                produto.ValorUnitarioCusto = _utilitariosService.ObterValorDecimal(valorUnitarioCusto, "O valor unitário de custo informado não é válido!");
                produto.ValorUnitarioVenda = _utilitariosService.ObterValorDecimal(valorUnitarioVenda, "O valor unitário de venda informado não é válido!");
                produto.EstaAtivo = produto.EstaAtivo;
                produto.EstaAtivo = _utilitariosService.ObterValorBoolean(estaAtivo, "O flag de status informado não é válido!");

                return produto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}