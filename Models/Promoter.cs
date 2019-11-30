using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inzynierka.Models
{
    public class promoter
    {
        public long Id { get; set; }
        [Display(Name = "Email")]
        public String Email { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Długość {0} musi wynosić co najmniej {2} znaków.", MinimumLength = 3)]
        [Display(Name = "Imię")]
        public String Forname { get; set; }
        [Display(Name = "Nazwisko")]
        [Required]
        [StringLength(30, ErrorMessage = "Długość {0} musi wynosić co najmniej {2} znaków.", MinimumLength = 3)]
        public String Surname { get; set; }
        [Required]
        [Display(Name = "Telefon")]
        public long phone { get; set; }
    }
}