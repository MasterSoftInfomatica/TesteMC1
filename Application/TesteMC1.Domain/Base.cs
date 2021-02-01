using System;
using System.Globalization;
using System.Collections.Generic;

namespace TesteMC1.Domain
{
    public class Base
    {
        protected string Idioma { get { return "pt-BR"; } }
        protected CultureInfo Cultura { get { return new CultureInfo(Idioma); } }
    }
}
