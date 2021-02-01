using System;
using System.Globalization;
using TesteMC1.Domain;
using TesteMC1.DataAccess;
using TesteMC1.DataAccess.Context.SqlServer;

namespace TesteMC1.Application.Services
{
    public class BaseService : Base
    {
        public BaseService()
        {
            try
            {
                DbContext = new Context();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public new string Idioma { get { return base.Idioma; } }
        public new CultureInfo Cultura { get { return base.Cultura; } }
        protected Context DbContext { get; set; }
    }
}
