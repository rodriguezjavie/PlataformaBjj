using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaBjj.Models
{
    public class UserType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Tipo de usuario")]
        public string Name { get; set; }

    }
}
