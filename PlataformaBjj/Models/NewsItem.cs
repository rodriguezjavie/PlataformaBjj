using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaBjj.Models
{
    public class NewsItem
    {
        public NewsItem()
        {
            UploadDate = new DateTime();
        }
        public int Id { get; set; }

        [Display(Name = "Titulo")]
        [Required]
        public string Title { get; set; }
        public string Image { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; } 
    [Display(Name ="Categoria")]
        public string Type { get; set; }
        public DateTime UploadDate { get; set; }

        public bool IsActive { get; set; }
    }
}
