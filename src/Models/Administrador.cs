using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Administrador
    {
        private string perfil = "Adm";
        public string Perfil { get => this.perfil; }
        public string Nome { get; set; }
        [Key]
        public string CPF { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string senha { get; set; }
    }
}
