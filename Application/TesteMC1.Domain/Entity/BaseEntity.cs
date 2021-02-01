using System;
using System.Globalization;
using System.Collections.Generic;

namespace TesteMC1.Domain.Entity
{
    public abstract class BaseEntity : BaseDomain
    {
        public enum OperacoesCRUD { Create, Read, Update, Detele, None }

        public OperacoesCRUD OperacaoCRUD { get; set; } = OperacoesCRUD.None;
    }
}
