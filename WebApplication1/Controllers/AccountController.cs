using DataAccessLayer.Context;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        DataContext db = new DataContext();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User data)
        {
            var bilgiler = db.Users.FirstOrDefault(x => x.Mail == data.Mail && x.Password == data.Password);


            if (bilgiler != null)
            {
                if (bilgiler.Dogrulama == true)
                {
                    Session["mail1"] = bilgiler.Mail.ToString();
                    Session["Ad1"] = bilgiler.Name.ToString();
                    Session["Soyad1"] = bilgiler.SurName.ToString();
                    Session["userid1"] = bilgiler.UserId.ToString(); ;
                    Session["Role1"] = bilgiler.Role.ToString();
                    var mail = bilgiler.Mail;

                    Random random = new Random();
                    int kodd = random.Next(100000, 999999);
                    string kod = kodd.ToString();
                    Session["kod"] = kod;

                    WebMail.SmtpServer = "smtp.gmail.com";
                    WebMail.EnableSsl = true;
                    WebMail.UserName = "bavcu292@gmail.com";
                    WebMail.Password = "davrhokopgbsrmfb";
                    WebMail.SmtpPort = 587;
                    WebMail.Send(mail, subject: "Doğrulama Şifresi", body: "Doğrulama şifreniz &nbsp; &nbsp;" + kod);


                    return RedirectToAction("Dogrula");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(bilgiler.Mail, false);
                    Session["Mail"] = bilgiler.Mail.ToString();
                    Session["Ad"] = bilgiler.Name.ToString();
                    Session["Soyad"] = bilgiler.SurName.ToString();
                    Session["userid"] = bilgiler.UserId.ToString();
                    Session["Role"] = bilgiler.Role.ToString();
                    return RedirectToActionPermanent("Index", "Home");
                }

            }
            ViewBag.hata = "Mail veya şifreniz hatalı";


            return View(data);
        }

        public ActionResult Dogrula()
        {

            int hak = 3;

            ViewBag.hak = hak;
            return View();
        }
        [HttpPost]
        public ActionResult Dogrula(string kod, int hak)
        {
            if (hak > 3)
            {
                hak = 1;
            }
            if (Session["kod"].ToString() == null)
            {
                if (hak == 1)
                {
                    ViewBag.doldu = 1;
                    return View("Login");
                }

                hak--;
                ViewBag.hak = hak;
                ViewBag.hata = "Kodu Yanlış Girdiniz";
            }
            if (Session["kod"].ToString() == kod)
            {

                FormsAuthentication.SetAuthCookie(Session["Mail1"].ToString(), false);
                Session["Mail"] = Session["Mail1"].ToString();
                Session["Ad"] = Session["Ad1"].ToString();
                Session["Soyad"] = Session["Soyad1"].ToString();
                Session["userid"] = Session["userid1"].ToString();
                Session["Role"] = Session["Role1"].ToString();
                return RedirectToActionPermanent("Index", "Home");
            }

            else
            {
                if (hak == 1)
                {
                    ViewBag.doldu = 1;
                    return View("Login");
                }

                hak--;
                ViewBag.hak = hak;
                ViewBag.hata = "Kodu Yanlış Girdiniz";
            }

            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToActionPermanent("Login", "Account");

        }
    }
}