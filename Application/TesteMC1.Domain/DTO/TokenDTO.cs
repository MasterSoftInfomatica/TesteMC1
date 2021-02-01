using System;

namespace TesteMC1.Domain.DTO
{
    public class TokenDTO : BaseDTO
    {
        public string Email { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataValidade { get; set; }
        public bool SessaoValida { get { return DateTime.Now < DataValidade; } }
    }
}
