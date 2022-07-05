using BusinessLayer.Concrete;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using DataAccessLayer.Context;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]

    public class UserController : Controller
    {
        DataContext db = new DataContext();
        // GET: User
        UserRepository UserRepository = new UserRepository();
        public ActionResult Index(int sayfa = 1)
        {
            return View(UserRepository.List().ToPagedList(sayfa, 25));
        }
        public ActionResult Delete(int UserId)
        {
            var delete = UserRepository.GetById(UserId);
            UserRepository.Delete(delete);
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(User p)
        {
            if (ModelState.IsValid)
            {
                var a = db.Users.Where(x => x.Mail == p.Mail);
                var b = a.Count();
                if (b == 1)
                {
                    ViewBag.hata = "Kullanmak İstediğiniz Mail Adresi Başka Bir Personel Tarafından kullanılmakltadır";
                    return View();
                }
                else
                {
                    p.Dogrulama = false;
                    p.Role = "Personel";
                    UserRepository.Insert(p);
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError("", "Bir hata oluştu.");
            return View();
        }
        public ActionResult Update(int id)
        {
            var update = UserRepository.GetById(id);
            return View(update);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Update(User data)
        {
            var update = UserRepository.GetById(data.UserId);
            if (ModelState.IsValid)
            {
                update.Name = data.Name;
                update.SurName = data.SurName;
                update.Mail = data.Mail;
                update.Password = data.Password;
                UserRepository.Update(update);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Bir hata oluştu.");
            return View(update);
        }
    }
}