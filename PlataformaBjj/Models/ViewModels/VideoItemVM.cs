using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaBjj.Models.ViewModels
{
    public class VideoItemVM
    {
        public VideoItem VideoItem { get; set; }
        public IEnumerable<Category> Category { get; set; }
        public IEnumerable<SubCategory> SubCategory { get; set; }
        [Display(Name ="Notificar por correo")]
        public bool EmailNotification { get; set; }

    }
}
