using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inzynierka.Models
{
    public class Event
    {
        public int Id { get; set; }
        [Display(Name = "Data Rozpoczęcia")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Data Zakończenia")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Nazwa Wydarzenia")]
        public string Name { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Miejsce")]
        public string Place { get; set; }
    }
}