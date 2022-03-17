using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaBjj.Models
{
    public class Email
    {
        public int Id { get; set; }

        [Display(Name ="Dirección de correo")]
        public string EmailAddress { get; set; }
        [Display(Name = "Titulo del correo")]
        public string EmailSubject { get; set; }

        [Display(Name = "Mensaje")]
        public string EmailMessage { get; set; }
    }
}
