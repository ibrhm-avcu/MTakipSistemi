using DataAccessLayer.Context;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HesapController : Controller
    {
        // GET: Hesap
        DataContext db = new DataContext();
        public ActionResult Update()
        {
            var mail = (string)Session["Mail"];
            var degerler = db.Users.FirstOrDefault(x => x.Mail == mail);
            return View(degerler);
        }

        [HttpPost]
        public ActionResult Update(User data)
        {
            var username = (string)Session["Mail"];
            var user = db.Users.Where(x => x.Mail == username).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (user.BackPassword == data.BackPassword)
                {
                    user.Name = data.Name;
                    user.SurName = data.SurName;
                    user.Mail = data.Mail;
                    user.Password = data.Password;
                    user.BackPassword = data.Password;
                    user.Dogrulama = data.Dogrulama;
                    db.SaveChanges();
                    ViewBag.basarili = "Bilgileriniz başarıyla Güncellendi";
                }
                else
                {
                    ViewBag.uyari = "Eski Şifreniz Hatalı.";

                }
            }
            else
            {
                ModelState.AddModelError("", "Bir hata oluştu.");
                return View(user);
            }
            return View("Update", data);
        }
    }
}