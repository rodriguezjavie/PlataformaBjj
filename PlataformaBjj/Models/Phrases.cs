using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaBjj.Models
{
    public class Phrases
    {
        public int Id { get; set; }
        [Display(Name ="Frase")]
        public string Phrase { get; set; }
        [Display(Name = "Autor")]
        public string Author { get; set; }
        [Display(Name ="Imagen")]
        public string Image { get; set; }
    }
}
