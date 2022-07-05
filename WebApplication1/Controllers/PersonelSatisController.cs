using BusinessLayer.Concrete;
using DataAccessLayer.Context;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using WebApplication1.Controllers;
using static EntityLayer.Kur;
using EntityLayer;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Personel")]

    public class PersonelSatisController : Controller
    {

        // GET: PersonelSatis
        SatisRepository satisRepository = new SatisRepository();
        DataContext db = new DataContext();

        public ActionResult Index(int sayfa = 1)
        {
            return View(satisRepository.List().ToPagedList(sayfa, 25));
        }
        public ActionResult Create()
        {

            List<SelectListItem> deger1 = (from i in db.Customers.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.Name,
                                               Value = i.CustomerId.ToString()
                                           }).ToList();
            ViewBag.ktgr = deger1;
            List<SelectListItem> deger2 = (from i in db.Dovizs.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.DovizName,
                                               Value = i.DovizId.ToString()
                                           }).ToList();
            ViewBag.ktgr1 = deger2;
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Fatura p)
        {

            List<SelectListItem> deger1 = (from i in db.Customers.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.Name,
                                               Value = i.CustomerId.ToString()
                                           }).ToList();
            ViewBag.ktgr = deger1;
            List<SelectListItem> deger2 = (from i in db.Dovizs.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.DovizName,
                                               Value = i.DovizId.ToString()
                                           }).ToList();
            ViewBag.ktgr1 = deger2;

            if (ModelState.IsValid)
            {
                p.Price = Convert.ToDouble(p.Price);
                var dovizkuru = db.Dovizs.Where(x=> x.DovizId == p.DovizId).First();
                double kurDergeri = Kur.Deger(dovizkuru.DovizName);
                p.FaturaKesimTarihi = DateTime.Now;
                p.UserId = Convert.ToInt32(Session["userid"]);
                var toplam = p.Miktar * p.Price;
                var toplamtutar = toplam * p.Adet;
                p.Toplamtutar = toplamtutar;
                p.DovizFiyat = kurDergeri;
                satisRepository.Insert(p);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Bir hata oluştu.");
            return View();
        }
        public ActionResult Update(int id)
        {
            List<SelectListItem> deger1 = (from i in db.Customers.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.Name,
                                               Value = i.CustomerId.ToString()


                                           }).ToList();
            ViewBag.ktgr = deger1;
            List<SelectListItem> deger2 = (from i in db.Dovizs.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.DovizName,
                                               Value = i.DovizId.ToString()
                                           }).ToList();
            ViewBag.ktgr1 = deger2;
            var update = satisRepository.GetById(id);
            return View(update);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Update(Fatura data)
        {
            List<SelectListItem> deger1 = (from i in db.Customers.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.Name,
                                               Value = i.CustomerId.ToString()


                                           }).ToList();
            ViewBag.ktgr = deger1;
            List<SelectListItem> deger2 = (from i in db.Dovizs.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.DovizName,
                                               Value = i.DovizId.ToString()
                                           }).ToList();
            ViewBag.ktgr1 = deger2;
            var update = satisRepository.GetById(data.FaturaId);

            if (ModelState.IsValid)
            {
                update.DovizId = data.DovizId;
                var dovizkuru = db.Dovizs.Where(x => x.DovizId == update.DovizId).First();
                double kurDergeri = Kur.Deger(dovizkuru.DovizName);
                update.DovizFiyat = kurDergeri;
                update.ProductName = data.ProductName;
                update.Price = Convert.ToDouble(data.Price);
                update.Miktar = data.Miktar;
                update.Adet = data.Adet;
                var toplam = data.Miktar * data.Price;
                var toplamtutar = toplam * data.Adet;
                update.Toplamtutar = toplamtutar;
                satisRepository.Update(update);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Bir hata oluştu.");
            return View(update);
        }

        public ActionResult Filtre(string p)
        {
            return View();
        }

        public ActionResult Ara(string p)
        {
            var ziyarets = from x in db.Faturas select x;
            if (!string.IsNullOrEmpty(p))
            {
                ziyarets = ziyarets.Where(x => x.Customer.Name.ToLower().Contains(p.ToLower()) || x.ProductName.ToLower().Contains(p.ToLower()) || x.Doviz.DovizName.ToLower().Contains(p.ToLower()));
            }
            return View(ziyarets.ToList());
        }

    }
}