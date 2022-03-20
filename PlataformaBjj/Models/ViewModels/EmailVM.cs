using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaBjj.Models.ViewModels
{
    public class EmailVM
    {
        public Email Email { get; set; }
        [Display(Name ="Todos los usuarios")]
        public bool AllUsers { get; set; }

        [Display(Name = "Alumnos")]
        public bool Students { get; set; }

        [Display(Name = "Profesores")]
        public bool Profesors { get; set; }

        [Display(Name = "Pendientes de Pago")]
        public bool Payment { get; set; }
    }
}
