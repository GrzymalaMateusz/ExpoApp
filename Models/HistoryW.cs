using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inzynierka.Models
{
    public class HistoryW
    {
        public long ID { get; set; }
        [Display(Name = "Wyszukiwanie")]
        public String Search { get; set; }
        [Display(Name = "Opis")]
        public String Description { get; set; }

        public virtual Expo Expo { get; set; }
        public virtual Company Company { get; set; }
    }
}