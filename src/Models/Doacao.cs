using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace doee.Models
{
    public class Doacao
    {
        [Key]
        public int Id { get; set; }
        //[DataType(DataType.Date)]
        public DateTime Data { get; set; }
        public string NomeDoador { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string NomeCartao { get; set; }
        public string NumeroCartao { get; set; }
        public string Validade { get; set; }
        public int CodSeguranca { get; set; }
        public double Valor { get; set; }
        public string BeneficiarioCNPJ { get; set; }
        [ForeignKey("BeneficiarioCNPJ")]
        public Instituicao Instituicao { get; set; }

    }
}
