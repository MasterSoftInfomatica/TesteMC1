using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteMC1.Domain.Entity;

namespace TesteMC1.Domain.DTO
{
    public class PerfilUsuarioDTO : BaseDTO
    {
        public Usuario.Perfis Perfil { get; set; }
        public string Descricao { get; set; }
    }
}
