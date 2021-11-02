using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace doee.Models
{
    public enum Setor
    {
        Alimentacao,
        Educacao,
        Esporte,
        Saude
    }
    public class Instituicao
    {
        private string perfil = "Ong";
        public string Perfil { get => this.perfil; }
        public string Nome { get; set; }
        [Key]
        public string CNPJ { get; set; }
        public Setor setor { get; set; }
        public string Descricao { get; set; }
        public decimal MetaArrecadacao { get; set; }
        //[EmailAddress]
        public DateTime DataRegistro { get; set; }
        [Required]
        public string Email { get; set; }
        ///[StringLength(15, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Required]
        public string senha { get; set; }
        public ICollection<Doacao> Doacoes { get; set; }
    }
  
}
