using Inzynierka.DAL;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Inzynierka.Controllers
{
    [Authorize]
    public class PDFController : Controller
    {
        
        public void Index(string layout)
        {
            String text=null;
            String name = null;
            String company = null;
            ExpoesContext expo = new ExpoesContext();
            bool exC = expo.Companies.Any(p => p.Email == User.Identity.Name);
            if (exC == true)
            {
                var Usr = expo.Companies.Single(p => p.Email == User.Identity.Name);
                text = "Wystawca:" + User.Identity.Name;
                company = Usr.CompanyName;
            }
            bool exU = expo.Users.Any(p => p.Email == User.Identity.Name);
            if (exU == true)
            {
                var Usr = expo.Users.Single(p => p.Email == User.Identity.Name);
                text = "Uczestnik:" + User.Identity.Name;
                company = "";
                name = Usr.ForName+" "+Usr.SurName;
            }
            string filename = String.Format("{0}_tempfile.pdf", Guid.NewGuid().ToString("D").ToUpper());
            PdfDocument s_document = new PdfDocument();
            s_document.Info.Title = "Your QRCode";
            s_document.Info.Author = "ExpoApp";
            s_document.Info.Subject = "Grenerating your QRCODE";
            s_document.Info.Keywords = "QRcode";
            PdfPage page=s_document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            // GET: PDF
            QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)1;
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, eccLevel))
                {
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        Bitmap bitmap = new Bitmap(qrCode.GetGraphic(20, Color.Black, Color.White, true));
                        //pdf
                        //BitmapSource bitmapSource =
                        //  System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        //    bitmap.GetHbitmap(),
                        //      IntPtr.Zero,
                        //      Int32Rect.Empty,
                        //      BitmapSizeOptions.FromEmptyOptions());
                        //Image i=Image.
                        XImage image = XImage.FromGdiPlusImage((Image)bitmap);
                        XFont font = new XFont("Times New Roman", 20, XFontStyle.Bold);
                        XImage makieta = XImage.FromFile(HttpContext.Server.MapPath("~/Images/Expo/pdf/"+layout));
                        // Left position in point
                        gfx.DrawImage(makieta, 0, 0, page.Width, page.Height);
                        gfx.DrawImage(image, 120, 215,75,75);
                        gfx.DrawImage(image, 120, 640, 75, 75);
                        //gfx.DrawString(, font, XBrushes.Black,
                       // new XRect(0, 0, page.Width, page.Height),
                        //XStringFormats.Center);
                        gfx.DrawString(name,font,XBrushes.Black, new XRect(30, 175, 250, 20),
                        XStringFormats.Center);
                        gfx.DrawString(company, font, XBrushes.Black, new XRect(30, 195, 250, 20),
                        XStringFormats.Center);
                        gfx.DrawString(name, font, XBrushes.Black, new XRect(30, 600, 250, 20),
                        XStringFormats.Center);
                        gfx.DrawString(company, font, XBrushes.Black, new XRect(30, 620, 250, 20),
                        XStringFormats.Center);
                    }
                }
            }
            s_document.Save(HttpContext.Server.MapPath("~/temp/") + filename);
            Response.ContentType = "application/pdf"; 
            Response.TransmitFile(HttpContext.Server.MapPath("~/temp/") + filename);
            // ...and start a viewer
            //return Redirect("E:/temp/" + filename);
        }
    }
}