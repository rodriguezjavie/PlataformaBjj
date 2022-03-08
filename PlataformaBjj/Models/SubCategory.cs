using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaBjj.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="SubCategoria")]
        [Required]
        public string Name { get; set; }
        [Display(Name="Categoria")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
