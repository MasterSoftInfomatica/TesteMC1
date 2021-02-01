using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace TesteMC1.Application.Services
{
    public class EmailSmtpService
    {
        public void EnviarMensagem(string de, string para, string cc, string assunto, string corpoMensagem, bool formatoHtml = false)
        {
            try
            {
                MailMessage mensagemEmail = new MailMessage();

                mensagemEmail.From = new MailAddress(de);

                List<string> paraLista = ObterItens(para);
                if (paraLista != null)
                {
                    foreach (var endereco in paraLista)
                    {
                        try
                        {
                            mensagemEmail.To.Add(new MailAddress(endereco));
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                List<string> ccLista = ObterItens(cc);
                if (ccLista != null)
                {
                    foreach (var endereco in ccLista)
                    {
                        try
                        {
                            mensagemEmail.CC.Add(new MailAddress(endereco));
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                mensagemEmail.Subject = assunto;
                mensagemEmail.Priority = MailPriority.Normal;
                mensagemEmail.IsBodyHtml = formatoHtml;
                mensagemEmail.Body = corpoMensagem;

                //Cria o cliente SMTP
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = Properties.Settings.Default.EnderecoServidorSmtp;
                smtpClient.Port = Properties.Settings.Default.PortaServidorSmtp;
                smtpClient.EnableSsl = Properties.Settings.Default.SslHabilitadoServidorSmtp;
                if (!string.IsNullOrEmpty(Properties.Settings.Default.UsuarioServidorSmtp) & !string.IsNullOrEmpty(Properties.Settings.Default.SenhaServidorSmtp))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(Properties.Settings.Default.UsuarioServidorSmtp, Properties.Settings.Default.SenhaServidorSmtp);
                }
                smtpClient.Send(mensagemEmail);
                smtpClient.Dispose();
            }
            catch (Exception ex)
            {
                string mensagemErro = ex.Message;
                if (ex.InnerException != null) mensagemErro += Environment.NewLine + ex.InnerException.Message;
                throw new Exception("Erro ao enviar a mensagem de e-mail! Mensagem recebida do servidor: " + mensagemErro, ex);
            }
        }

        private List<string> ObterItens(string origem)
        {
            try
            {
                if (string.IsNullOrEmpty(origem)) return null;
                return origem.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList().Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
