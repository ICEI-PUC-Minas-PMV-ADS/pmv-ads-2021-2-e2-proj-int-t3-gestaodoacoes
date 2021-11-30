using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Instituicao
    {
        private string perfil = "Ong";
        public string Perfil { get => this.perfil; }
        [Display(Name = "Cadastro no site")]
        public string DataRegistro { get; set; }
        [Required(ErrorMessage = "Informe o nome da instituição")]
        [StringLength(50)]
        public string Nome { get; set; }
        [Key]
        [Required(ErrorMessage = "Informe o CNPJ")]
        [MinLength(14, ErrorMessage = "CNPJ deve ter {1} caracteres, apenas números")]
        [MaxLength(14, ErrorMessage = "CNPJ deve ter {1} caracteres, apenas números")]
        public string CNPJ { get; set; }
        [Display(Name = "Setor de Atuação")]
        public int CategoriaId { get; set; }    
        public virtual Categoria Categoria { get; set; }
        [Required(ErrorMessage = "Descrição curta deve ser fornecida")]
        [Display(Name = "Descrição curta")]
        [MinLength(100, ErrorMessage = "Descrição curta deve ter no mínimo {1} caracteres")]
        public string DescricaoCurta { get; set; }
        [Display(Name = "Descrição Detalhada")]
        [Required(ErrorMessage = "O descrição detalhada deve ser fornecida")]
        [MinLength(400, ErrorMessage = "Descrição detalhada deve ter no mínimo {1} caracteres")]
        public string DescricaoDetalhada { get; set; }
        [Display(Name = "Meta de Arrecadação")]
        [Required(ErrorMessage = "Informe a meta de arrecadação")]
        public decimal MetaArrecadacao { get; set; }
        [Required(ErrorMessage = "Informe o Estado")]
        public Estado Estado { get; set; }
        [Required(ErrorMessage = "Informe a Cidade")]
        public string Cidade { get; set; }
        [Required(ErrorMessage = "Informe Logradouro (Rua e Número)")]
        public string Logradouro { get; set; }
        [Required(ErrorMessage = "Informe o CEP")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "Informe o email")]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "O {0} deve ter no mínimo {1} e no máximo {2} caracteres")]
        public string senha { get; set; }
        public ICollection<Doacao> Doacoes { get; set; }


    }
    public enum Estado
    {
        AC, AL, AP, AM, BA ,CE  ,DF  ,ES  ,GO  , MA, MT , MS ,MG  ,PA  ,PB  ,PR  ,PE , PI  ,RJ  ,RN  ,RS  ,RO  ,RR  ,SC  ,SP  ,SE,TO
    }

}
