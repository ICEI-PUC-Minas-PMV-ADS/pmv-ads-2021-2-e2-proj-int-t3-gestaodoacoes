using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }
        [Display(Name = "Nome da Categoria")]
        [Required]
        [StringLength(100)]
        public string CategoriaNome { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Descrição da categoria")]
        public string Descricao { get; set; }
        public virtual List<Instituicao> Instituicoes { get; set; }
    }
}