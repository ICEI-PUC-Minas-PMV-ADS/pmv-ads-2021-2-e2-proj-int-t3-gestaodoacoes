using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApplication2.Models
{
    public class Doacao
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Data da doação")]
        public string Data { get; set; }
        [Display(Name = "Nome do doador")]
        [Required(ErrorMessage = "Informe o nome")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Informe o email")]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "CPF")]
        [Required]
        [MinLength(11, ErrorMessage = "CPF deve ter {1} caracteres, apenas números")]
        [MaxLength(11, ErrorMessage = "CPF deve ter {1} caracteres, apenas números")]
        public string Cpf { get; set; }
        [Display(Name = "Nome C/Cartão")]
        [Required]
        public string NomeCartao { get; set; }
        [Display(Name = "Número do cartão")]
        [Required]
        [MinLength(16, ErrorMessage = "Numero {1} caracteres")]
        [MaxLength(16, ErrorMessage = "Numero {1} caracteres")]
        public string NumeroCartao { get; set; }
        [Display(Name = "Validade MM/AA")]
        [Required]
        public string ValidadeCartao { get; set; }
        [Display(Name = "Cód de Segurança 'XXX'")]
        [Required]
        public int CodSegurancaCartao { get; set; }

        [Display(Name = "Valor ")]
        [Required]
        public double Valor { get; set; }
        public string instituicaoCNPJ { get; set; }
        [ForeignKey("instituicaoCNPJ")]
        public Instituicao Instituicao { get; set; }

    }
}
