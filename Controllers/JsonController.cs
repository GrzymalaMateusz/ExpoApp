using Inzynierka.DAL;
using Inzynierka.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inzynierka.Controllers
{
    public class JsonController : Controller
    {
        // GET: Json
        public async System.Threading.Tasks.Task<JsonResult> Login(String email, String password)
        {
            bool tempbool=false;
            string temptype="";
            var result = await HttpContext.GetOwinContext().Get<ApplicationSignInManager>().PasswordSignInAsync(email, password, false, false);
            switch (result)
            {
                case SignInStatus.Success:
                    ExpoesContext db = new ExpoesContext();
                    bool exC = db.Companies.Any(p => p.Email == email);
                    bool exU = db.Users.Any(p => p.Email == email);
                    if (exC == true)
                    {
                        tempbool = true;
                        temptype = "Company";
                    }
                    else if (exU == true)
                    {
                        tempbool = true;
                        temptype = "User";
                    }
                    break;
                case SignInStatus.LockedOut:
                    tempbool = false;
                    temptype = "";
                    break;
                case SignInStatus.RequiresVerification:
                    tempbool = false;
                    temptype = "";
                    break;
                case SignInStatus.Failure:
                default:
                    tempbool = false;
                    temptype = "";
                    break;
            }
            MobileLogin l = new MobileLogin
            {
                UserAndPasswordCorrect = tempbool,
                UserType = temptype
            };

            return Json(l, JsonRequestBehavior.AllowGet);
        }
        public async System.Threading.Tasks.Task<JsonResult> Register(String Email, String Password,String UserType)
        {

            RegisterViewModelMobile model = new RegisterViewModelMobile()
            {
                Email = Email,
                Password = Password,
                ConfirmPassword = Password,
                AccountType = UserType
            };
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().CreateAsync(user, model.Password);
            if (model.AccountType.Contains("User"))
            {
                await HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().AddToRoleAsync(user.Id, "User");
                User u = new Models.User { Email = model.Email };
                ExpoesContext expo = new ExpoesContext();
                expo.Users.Add(u);
                expo.SaveChanges();
                var data = new { AccountCreated = true, UnValid = "" };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            if (model.AccountType.Contains("Company"))
            {
                await HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().AddToRoleAsync(user.Id, "Wystawca");
                Company c = new Models.Company { Email = model.Email };
                ExpoesContext expo = new ExpoesContext();
                expo.Companies.Add(c);
                expo.SaveChanges();
                var data = new { AccountCreated = true, UnValid = ""};
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            var data1 = new { AccountCreated = false, UnValid = "Email" };
            return Json(data1, JsonRequestBehavior.AllowGet);
        }
        public JsonResult User(String email)
        {
            ExpoesContext expo = new ExpoesContext();
            expo.Configuration.ProxyCreationEnabled = false;
            var c = expo.Users.Single(p => p.Email == email);
            c.Expo = null;
            return Json(c,JsonRequestBehavior.AllowGet);
        }
        public JsonResult UserExpos(String email)
        {
            ExpoesContext expo = new ExpoesContext();
            expo.Configuration.ProxyCreationEnabled = false;
            var c = expo.Expos.Where(p=>p.Users.Any(e=>e.Email==email)).ToList();
            return Json(c, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CompanyExpos(String email)
        {
            ExpoesContext expo = new ExpoesContext();
            expo.Configuration.ProxyCreationEnabled = false;
            var c = expo.Expos.Where(p => p.Company.Any(e => e.Email == email)).ToList();
            return Json(c, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Company(String email)
        {
            ExpoesContext expo = new ExpoesContext();
            expo.Configuration.ProxyCreationEnabled = false;
            var c = expo.Companies.Single(p => p.Email == email);
            return Json(c, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Expo(int id)
        {
            ExpoesContext expo = new ExpoesContext();
            expo.Configuration.ProxyCreationEnabled = false;
            var c = expo.Expos.Single(p => p.Id == id);
            return Json(c, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Expos()
        {
            ExpoesContext expo = new ExpoesContext();
            expo.Configuration.ProxyCreationEnabled = false;
            var c = expo.Expos;
            return Json(c, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Events(int id)
        {
            ExpoesContext expo = new ExpoesContext();
            var c = expo.Expos.Find(id).Events;
            expo.Configuration.ProxyCreationEnabled = false;
            return Json(c, JsonRequestBehavior.AllowGet);
        }
        public ActionResult History(string email)
        {
            ExpoesContext expo = new ExpoesContext();
            expo.Configuration.ProxyCreationEnabled = false;

            bool exC = expo.Companies.Any(p => p.Email == email);
            bool exU = expo.Users.Any(p => p.Email == email);
            List<HistoryUpload> lista = new List<HistoryUpload>();
            if (exU == true)
            {
                var list = expo.History.Where(p => p.User.Email == email).ToList();
                foreach (History i in list)
                {
                    HistoryUpload h = new HistoryUpload();
                    h.ID = i.ID;
                    h.Description = i.Description;
                    h.Wyszukiwanie = i.Search;
                    h.Expo = long.Parse(i.Expo.Id.ToString());
                    h.User = i.User.ID;
                    lista.Add(h);
                }
            }
            else if (exC == true)
            {
                var list = expo.HistoryW.Where(p => p.Company.Email == email).ToList();
                foreach (HistoryW i in list)
                {
                    HistoryUpload h = new HistoryUpload();
                    h.ID = i.ID;
                    h.Description = i.Description;
                    h.Wyszukiwanie = i.Search;
                    h.Expo = long.Parse(i.Expo.Id.ToString());
                    h.User = i.Company.Id;
                    lista.Add(h);
                }
            }

            
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult insertHistoryU(string Search,string Description, long Userid, int Expoid)
        {
            ExpoesContext expo = new ExpoesContext();
            History h = new History()
            {
                Search=Search,
                Description=Description,
                User=expo.Users.Single(p => p.ID ==Userid),
                Expo=expo.Expos.Single(p=>p.Id==Expoid)
            };
            expo.History.Add(h);
            expo.SaveChanges();
            expo.Configuration.ProxyCreationEnabled = false;
            var data = new { saved = true};
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult insertHistoryC(string Search, string Description, long Companyid, int Expoid)
        {
            ExpoesContext expo = new ExpoesContext();
            HistoryW h = new HistoryW()
            {
                Search = Search,
                Description = Description,
                Company = expo.Companies.Single(p => p.Id == Companyid),
                Expo = expo.Expos.Single(p => p.Id == Expoid)
            };
            expo.HistoryW.Add(h);
            expo.SaveChanges();
            expo.Configuration.ProxyCreationEnabled = false;
            var data = new { saved = true };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UploadPhotoUser()
        {
            ExpoesContext expo = new ExpoesContext();
            string email = Request["Email"].ToString();
            var c = expo.Users.Single(p => p.Email == email);
            HttpPostedFileBase file = Request.Files["photod"];
            if (file != null && file.ContentLength > 0)
            {
                var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                c.Photo = Filename;
                file.SaveAs(HttpContext.Server.MapPath("~/Images/User/") + Filename);
            }
            UpdateModel(c);
            expo.SaveChanges();
            var data = new { UploadPhoto = email, Uploadstat = c.Photo};
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public ActionResult EditProfileUser(string Email, string ForName, string SurName, long Phone, string Nationality)
        {
            ExpoesContext expo = new ExpoesContext();
            var check = expo.Users.Any(p => p.Email == Email);
            if (check == false)
            {
                var c = new User()
                {
                    Email = Email,
                    ForName = ForName,
                    SurName = SurName,
                    Phone = Phone,
                    Nationality = nationality.Polish,
                    
                };
                expo.Users.Add(c);
            }
            else
            {
                var c = expo.Users.Single(p => p.Email == Email);
                c.Email = Email;
                c.ForName = ForName;
                c.SurName = SurName;
                c.Phone = Phone;
                c.Nationality = nationality.Polish;
                UpdateModel(c);
            }
            expo.SaveChanges();
            var data = new { AccountChanged = true, UnValid = "" };
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public ActionResult EditProfileCompany(string Email, string ContactEmail, string CompanyName,
            string CompanyFullName, string CompanyAbout, string ProductsAbout, long Phone, long UserPhone,string Facebook,string Instagram, string Snapchat,
            string Youtube,string www, string Adress,string NIP, string Forname_And_Surname,string Trade)
        {
            ExpoesContext expo = new ExpoesContext();
            var check = expo.Companies.Any(p => p.Email == Email);
            if (check == false)
            {
                var c = new Company()
                {
                    Email = Email,
                    ContactEmail = ContactEmail,
                    CompanyName= CompanyName,
                    CompanyFullName= CompanyFullName,
                    CompanyAbout = CompanyAbout,
                    ProductsAbout = ProductsAbout,
                    Phone=Phone,
                    UserPhone=UserPhone,
                    Adress=Adress,
                    ForName_And_SurName=Forname_And_Surname,
                    Facebook=Facebook,
                    Instagram=Instagram,
                    Snapchat=Snapchat,
                    NIP=NIP,
                    Youtube=Youtube,
                    www=www,
                    Trade=trade.Banking
                };
                expo.Companies.Add(c);
            }
            else
            {
                var c = expo.Companies.Single(p => p.Email == Email);
                c.Email = Email;
                c.ContactEmail = ContactEmail;
                c.CompanyName = CompanyName;
                c.CompanyFullName = CompanyFullName;
                c.CompanyAbout = CompanyAbout;
                c.ProductsAbout = ProductsAbout;
                c.Phone = Phone;
                c.UserPhone = UserPhone;
                c.Adress = Adress;
                c.ForName_And_SurName = Forname_And_Surname;
                c.Facebook = Facebook;
                c.Instagram = Instagram;
                c.Snapchat = Snapchat;
                c.NIP = NIP;
                c.Youtube = Youtube;
                c.www = www;
                UpdateModel(c);
            }
            expo.SaveChanges();
            var data = new { AccountChanged = true, UnValid = "" };
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public ActionResult UploadPhotoCompany()
        {
            ExpoesContext expo = new ExpoesContext();
            string email = Request["Email"].ToString();
            var c = expo.Companies.Single(p => p.Email == email);
            HttpPostedFileBase logo = Request.Files["logo"];
            if (logo != null && logo.ContentLength > 0)
            {
                var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                c.CompanyLogo = Filename;
                logo.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
            }
            HttpPostedFileBase stand = Request.Files["stand"];
            if (stand != null && stand.ContentLength > 0)
            {
                var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                c.StandPhoto = Filename;
                stand.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
            }
            HttpPostedFileBase photo1 = Request.Files["photo1"];
            if (photo1 != null && photo1.ContentLength > 0)
            {
                var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                c.Photo1 = Filename;
                photo1.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
            }
            HttpPostedFileBase photo2 = Request.Files["photo2"];
            if (photo2 != null && photo2.ContentLength > 0)
            {
                var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                c.Photo2 = Filename;
                photo2.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
            }
            HttpPostedFileBase photo3 = Request.Files["photo3"];
            if (photo3 != null && photo3.ContentLength > 0)
            {
                var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                c.Photo3 = Filename;
                photo3.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
            }
            HttpPostedFileBase photo4 = Request.Files["photo4"];
            if (photo4 != null && photo4.ContentLength > 0)
            {
                var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                c.Photo4 = Filename;
                photo4.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
            }
            HttpPostedFileBase photo5 = Request.Files["photo5"];
            if (photo5 != null && photo5.ContentLength > 0)
            {
                var Filename = System.Guid.NewGuid().ToString() + ".jpg";
                c.Photo5 = Filename;
                photo5.SaveAs(HttpContext.Server.MapPath("~/Images/Company/") + Filename);
            }
            UpdateModel(c);
            expo.SaveChanges();
            var data = new { UploadPhoto = email, Uploadstat = "Upload" };
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public ActionResult insertCExpo(long Companyid,int Expoid)
        {
            ExpoesContext db = new ExpoesContext();
            bool exC = db.Companies.Any(p => p.Id == Companyid);
            if (exC == true)
            {
                var Usr = db.Companies.Single(p => p.Id == Companyid);
                var Expo = db.Expos.Single(p => p.Id == Expoid);
                Expo.Company.Add(Usr);
                Usr.Expo.Add(Expo);
                db.SaveChanges();
            }
            var data = new { JoinedC = true };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult insertUExpo(long Userid, int Expoid)
        {
            ExpoesContext db = new ExpoesContext();
            bool exC = db.Users.Any(p => p.ID == Userid);
            if (exC == true)
            {
                var Usr = db.Users.Single(p => p.ID == Userid);
                var Expo = db.Expos.Single(p => p.Id == Expoid);
                Expo.Users.Add(Usr);
                Usr.Expo.Add(Expo);
                db.SaveChanges();
            }
            var data = new { JoinedU = true };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
    
    public class RegisterViewModelMobile
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string AccountType { get; set; }
    }
    public class HistoryUpload
    {
        public long ID { get; set; }
        public String Wyszukiwanie { get; set; }
        public String Description { get; set; }
        public long Expo { get; set; }
        public long User { get; set; }
    }
}