using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Inzynierka.Models
{
    public enum trade
    {
        [Description("Accounting")]
        Accounting,
        [Description("Administration & Secretarial")]
        AdministrationAndSecretarial,
        [Description("Advertising, Media, Arts & Entertainment")]
        AdvertisingMediaArtsAndEntertainment,
        [Description("Agriculture, Nature & Animal")]
        Agriculture,
        [Description("Banking & Finance")]
        Banking,
        [Description("Construction, Architecture & Interior Design")]
        Architecture,
        [Description("Customer Service & Call Centre")]
        CustomerService,
        [Description("Editional & Writing")]
        Editional,
        [Description("Education, Childcare & Training")]
        Education,
        [Description("Engineering")]
        Engineering,
        [Description("Executive & Strategic Managment")]
        Executive,
        [Description("Government, Defence & Emergency")]
        Government,
        [Description("HR & Recruitment")]
        Recruitment,
        [Description("Health, Medical & Pharmaceutical")]
        Health,
        [Description("Hospitality, Travel & Tourism")]
        Hospitality,
        [Description("IT")]
        It,
        [Description("Insurance & Superannuation")]
        Insurance,
        [Description("Legal")]
        Legal,
        [Description("Voluntary, Charity & Social Work")]
        VoluntaryCharityAndSocialWork
    }
    public class Company
    {
        public long Id { get; set; }
        public String Email { get; set; }
        [Display(Name = "Nazwa Firmy")]
        [Required]
        [StringLength(60, ErrorMessage = "Długość {0} musi wynosić co najmniej {2} znaków.", MinimumLength = 4)]
        public String CompanyName { get; set; }
        [Display(Name = "Pełna Nazwa Firmy")]
        public String CompanyFullName { get; set; }
        [Display(Name = "O Firmie")]
        public String CompanyAbout { get; set; }
        [Display(Name = "O Produktach")]
        public String ProductsAbout { get; set; }
        [Display(Name = "Logo Firmy")]
        public String CompanyLogo { get; set; }
        [Display(Name = "Zdjęcie Stoiska")]
        public String StandPhoto { get; set;  }
        [Display(Name = "Zdjęcia")]
        public String Photo1 { get; set; }
        [Display(Name = "Zdjęcie 2")]
        public String Photo2 { get; set; }
        [Display(Name = "Zdjęcie 3")]
        public String Photo3 { get; set; }
        [Display(Name = "Zdjęcie 4")]
        public String Photo4 { get; set; }
        [Display(Name = "Zdjęcie 5")]
        public String Photo5 { get; set; }
        [Display(Name = "Facebook")]
        public String Facebook { get; set; }
        [Display(Name = "Instagram")]
        public String Instagram { get; set; }
        [Display(Name = "Snapchat")]
        public String Snapchat { get; set; }
        [Display(Name = "Youtube")]
        public String Youtube { get; set; }
        [Display(Name = "Telefon Kontaktowy")]
        public long Phone { get; set; }
        [Display(Name = "Email Kontaktowy")]
        public String ContactEmail { get; set; }
        [Display(Name = "Strona WWW")]
        public String www { get; set; }
        [Display(Name = "Adres")]
        public String Adress { get; set; }
        [Display(Name = "NIP")]
        [Required]
        public String NIP { get; set; }
        [Display(Name = "Imie i Nazwisko")]
        [Required]
        public String ForName_And_SurName { get; set; }
        [Display(Name = "Prywatny Telefon")]
        public long UserPhone { get; set; }
        [Display(Name = "Branża")]
        public trade Trade { get; set; }
        [ScriptIgnore]
        public virtual ICollection<Files> Files{ get; set; }
        [ScriptIgnore]
        public virtual ICollection<Expo> Expo { get; set; }
    }
}