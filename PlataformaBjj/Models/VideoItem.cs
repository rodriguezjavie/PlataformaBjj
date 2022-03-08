using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaBjj.Models
{
    public class VideoItem
    {
        public VideoItem()
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

        [Display(Name = "Dirección Web")]
        public string URL { get; set; }
        public bool IsActive { get; set; }


        [Display(Name = "Categoria")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [Display(Name = "Categoria")]
        public Category Category { get; set; }

        
        public int SubCategoryId { get; set; }

        [ForeignKey("SubCategoryId")]
        [Display(Name = "SubCategoria")]
        public SubCategory SubCategory { get; set; }

        [Display(Name ="Fecha de carga")]
        public DateTime UploadDate { get; set; }
    }
}
