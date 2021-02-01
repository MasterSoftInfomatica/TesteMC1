using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TesteMC1.Application.Services
{
    public class UtilitariosService
    {
        public CultureInfo Cultura { get { return new CultureInfo("pt-BR"); } }

        public string ObterMensagemErroDetalhada(System.Data.Entity.Validation.DbEntityValidationException ex)
        {
            string mensagemErro = ObterMensagemErroDetalhada((Exception)ex);

            if (ex.EntityValidationErrors == null) return mensagemErro;
            if (ex.EntityValidationErrors.Count() == 0) return mensagemErro;

            mensagemErro += Environment.NewLine;

            foreach (var erro in ex.EntityValidationErrors)
            {
                foreach (var erroValidacao in erro.ValidationErrors)
                {
                    mensagemErro += erroValidacao.ErrorMessage + Environment.NewLine;
                }
            }

            return mensagemErro;
        }

        public string ObterMensagemErroDetalhada(Exception ex)
        {
            string mensagemErro = ex.Message;

            if (ex.InnerException != null)
            {
                mensagemErro += Environment.NewLine + ex.InnerException.Message;
                if (ex.InnerException.InnerException != null) mensagemErro += Environment.NewLine + ex.InnerException.InnerException.Message;
            }

            return mensagemErro;
        }

        public string ObterValorString(string valor)
        {
            string valorString = string.IsNullOrEmpty(valor) ? null : valor;
            return valorString;
        }

        public bool ObterValorBoolean(string valor, string mensagemErroValidacao)
        {
            try
            {
                bool valorBoolean = Convert.ToBoolean(valor, Cultura);
                return valorBoolean;
            }
            catch (Exception)
            {
                throw new Exception(mensagemErroValidacao);
            }
        }

        public bool? ObterValorBoolean(string valor, string mensagemErroValidacao, bool permiteNulo)
        {
            try
            {
                if (string.IsNullOrEmpty(valor) & permiteNulo) return null;
                return ObterValorBoolean(valor, mensagemErroValidacao);
            }
            catch (Exception)
            {
                throw new Exception(mensagemErroValidacao);
            }
        }

        public DateTime ObterValorDateTime(string valor, string mensagemErroValidacao)
        {
            try
            {
                DateTime valorDateTime = Convert.ToDateTime(valor, Cultura);
                return valorDateTime;
            }
            catch (Exception)
            {
                throw new Exception(mensagemErroValidacao);
            }
        }

        public DateTime? ObterValorDateTime(string valor, string mensagemErroValidacao, bool permiteNulo)
        {
            try
            {
                if (string.IsNullOrEmpty(valor) & permiteNulo) return null;
                return ObterValorDateTime(valor, mensagemErroValidacao);
            }
            catch (Exception)
            {
                throw new Exception(mensagemErroValidacao);
            }
        }

        public int ObterValorInt(string valor, string mensagemErroValidacao)
        {
            try
            {
                int valorInt = Convert.ToInt32(valor, Cultura);
                return valorInt;
            }
            catch (Exception)
            {
                throw new Exception(mensagemErroValidacao);
            }
        }

        public int? ObterValorInt(string valor, string mensagemErroValidacao, bool permiteNulo)
        {
            try
            {
                if (string.IsNullOrEmpty(valor) & permiteNulo) return null;
                return ObterValorInt(valor, mensagemErroValidacao);
            }
            catch (Exception)
            {
                throw new Exception(mensagemErroValidacao);
            }
        }

        public long ObterValorLong(string valor, string mensagemErroValidacao)
        {
            try
            {
                long valorLong = Convert.ToInt64(valor, Cultura);
                return valorLong;
            }
            catch (Exception)
            {
                throw new Exception(mensagemErroValidacao);
            }
        }

        public long? ObterValorLong(string valor, string mensagemErroValidacao, bool permiteNulo)
        {
            try
            {
                if (string.IsNullOrEmpty(valor) & permiteNulo) return null;
                return ObterValorLong(valor, mensagemErroValidacao);
            }
            catch (Exception)
            {
                throw new Exception(mensagemErroValidacao);
            }
        }

        public double ObterValorDouble(string valor, string mensagemErroValidacao)
        {
            try
            {
                valor = valor.Replace(".", Cultura.NumberFormat.NumberDecimalSeparator);
                double valorDouble = Convert.ToDouble(valor, Cultura);
                return valorDouble;
            }
            catch (Exception)
            {
                throw new Exception(mensagemErroValidacao);
            }
        }

        public double? ObterValorDouble(string valor, string mensagemErroValidacao, bool permiteNulo)
        {
            try
            {
                if (string.IsNullOrEmpty(valor) & permiteNulo) return null;
                return ObterValorDouble(valor, mensagemErroValidacao);
            }
            catch (Exception)
            {
                throw new Exception(mensagemErroValidacao);
            }
        }

        public decimal ObterValorDecimal(string valor, string mensagemErroValidacao)
        {
            try
            {
                valor = valor.Replace(".", Cultura.NumberFormat.NumberDecimalSeparator);
                decimal decimalValue = Convert.ToDecimal(valor, Cultura);
                return decimalValue;
            }
            catch (Exception)
            {
                throw new Exception(mensagemErroValidacao);
            }
        }

        public decimal? ObterValorDecimal(string valor, string mensagemErroValidacao, bool permiteNulo)
        {
            try
            {
                if (string.IsNullOrEmpty(valor) & permiteNulo) return null;
                return ObterValorDecimal(valor, mensagemErroValidacao);
            }
            catch (Exception)
            {
                throw new Exception(mensagemErroValidacao);
            }
        }

        public T ObterValorEnum<T>(string valor, string mensagemErroValidacao, bool ignorarMinusculasMaiusculas = true)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), valor, ignorarMinusculasMaiusculas);
            }
            catch (Exception)
            {
                throw new Exception(mensagemErroValidacao);
            }
        }

        public T ObterValorEnum<T>(string valor, string mensagemErroValidacao, bool permiteNulo, bool ignorarMinusculasMaiusculas = true)
        {
            try
            {
                if (string.IsNullOrEmpty(valor) & permiteNulo) return default;
                return ObterValorEnum<T>(valor, mensagemErroValidacao, ignorarMinusculasMaiusculas);
            }
            catch (Exception)
            {
                throw new Exception(mensagemErroValidacao);
            }
        }
    }
}
