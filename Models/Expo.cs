using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Inzynierka.Models
{
    public class Expo
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa Targów")]
        [Required]
        [StringLength(70, ErrorMessage = "First name cannot be longer than 70 characters.")]
        public String Name_Expo { get; set; }
        [Display(Name = "Zdjęcie")]
        public String Photo { get; set; }
        [Display(Name = "Mapa")]
        public String MapPhoto { get; set; }
        [Display(Name = "Adres")]
        [StringLength(70, ErrorMessage = "First name cannot be longer than 70 characters.")]
        public String Adres { get; set; }
        [Display(Name = "Krótki Opis")]
        public String Description { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString ="{0:dd.MM.yyyy HH:mm}",ApplyFormatInEditMode =true)]
        [Display(Name = "Data Rozpoczęcia")]
        public DateTime ExpoStartData { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data Zakończenia")]
        public DateTime ExpoEndData { get; set; }
        [ScriptIgnore]
        public String pdflayout { get; set; }
        [ScriptIgnore]
        public virtual ICollection<User> Users { get; set; }
        [ScriptIgnore]
        public virtual ICollection<Company> Company { get; set; }
        [ScriptIgnore]
        public virtual ICollection<Invite> Invites { get; set; }
        [ScriptIgnore]
        public virtual ICollection<Event> Events { get; set; }
        [ScriptIgnore]
        public long Promoter { get; set; }
    }
}