using BusinessLayer.Concrete;
using DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Web.Helpers;
using System.Data.SqlClient;
using EntityLayer.Entities;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AdminZiyaretController : Controller
    {
        // GET: AdminZiyaret
        ZiyaretRepository ZiyaretRepository = new ZiyaretRepository();
        ZiyaretGecmisRepository ziyaretGecmisRepository = new ZiyaretGecmisRepository();

        DataContext db = new DataContext();
        public ActionResult Index(int sayfa = 1)
        {
          
            return View(ZiyaretRepository.List().ToPagedList(sayfa, 25));
        }

        public ActionResult Ara(string p)
        {
            var ziyarets = from x in db.Ziyarets select x;
            if (!string.IsNullOrEmpty(p))
            {
                ziyarets = ziyarets.Where(x=> x.Name.ToLower().Contains(p.ToLower()) || x.ZiyaretId.ToString().Contains(p.ToLower()) || x.Location.ToLower().Contains(p.ToLower())
                || x.Durumogeleri.Durum.ToLower().Contains(p.ToLower()) || x.User.Name.ToLower().Contains(p.ToLower()) || x.Sube.Name.ToLower().Contains(p.ToLower()));
            }


            return View(ziyarets.ToList());
        }
        public ActionResult Filtre()
        {
            List<SelectListItem> personel = (from i in db.Users.Where(x => x.Role == "Personel").ToList()

                                             select new SelectListItem
                                             {
                                                 Text = i.Name,
                                                 Value = i.UserId.ToString(),
                                             }).ToList();


            ViewBag.ktgr = personel;
            List<SelectListItem> sube = (from i in db.Subes.ToList()
                                         select new SelectListItem
                                         {
                                             Text = i.Name,
                                             Value = i.SubeId.ToString()
                                         }).ToList();
            ViewBag.ktgr1 = sube;
            List<SelectListItem> durum = (from i in db.durumogeleris.ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.Durum,
                                              Value = i.DurumId.ToString()
                                          }).ToList();
            ViewBag.ktgr2 = durum;


            return View();
        }

        [HttpPost]
        public ActionResult FiltreList(Ziyaret data, int? SubeId, int? UserId, int? DurumId, DateTime? CreateDate, DateTime? ZiyaretDate, int sayfa = 1)
        {
            List<SelectListItem> personel = (from i in db.Users.Where(x => x.Role == "Personel").ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.Name,
                                                 Value = i.UserId.ToString()
                                             }).ToList();
            ViewBag.ktgr = personel;
            List<SelectListItem> sube = (from i in db.Subes.ToList()
                                         select new SelectListItem
                                         {
                                             Text = i.Name,
                                             Value = i.SubeId.ToString()
                                         }).ToList();
            ViewBag.ktgr1 = sube;
            List<SelectListItem> durum = (from i in db.durumogeleris.ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.Durum,
                                              Value = i.DurumId.ToString()
                                          }).ToList();
            ViewBag.ktgr2 = durum;
            


            SqlConnection baglanti = new SqlConnection(@"Data Source = .; Initial Catalog = MTakipSistemiv1; Trusted_Connection=True");
            string sql = $"Select * from Ziyarets where  ";
            string a = "";
            string b = "";
            if (CreateDate!= null)
            {
               a = data.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (ZiyaretDate != null)
            {
                b = data.ZiyaretDate.ToString("yyyy-MM-dd HH:mm:ss");
            }


            int filt = 0;

            if (UserId != null)
            {
                filt = 1;
                sql += $" UserId={UserId}";
            }

            if (SubeId != null)
            {
                if (filt == 0)
                {
                    sql += $" SubeId={SubeId}";
                    filt = 1;
                }
                else
                {
                    sql += $" and SubeId={SubeId}";
                }
            }

            if (DurumId != null)
            {
                if (filt == 0)
                {
                    sql += $"DurumId={DurumId}";
                    filt = 1;
                }
                else
                {
                    sql += $" and DurumId={DurumId}";
                }
            }

            if (CreateDate != null)
            {
                if (filt == 0)
                {
                    sql += $"CreateDate>='{a}'";
                    filt = 1;
                }
                else
                {
                    sql += $" and CreateDate>='{a}'";
                }
            }

            if (ZiyaretDate != null)
            {
                if (filt == 0)
                {
                    sql += $"ZiyaretDate<='{b}'";
                    filt = 1;
                }
                else
                {
                    sql += $" and ZiyaretDate<='{b}'";
                }
            }
            if (UserId == null && SubeId == null && DurumId == null && CreateDate == null && ZiyaretDate == null)
            {
                sql = $"Select * from Ziyarets";
            }
            var ls = db.Ziyarets.SqlQuery(sql).ToPagedList(sayfa, 25);
            return View(ls);

        }
        public ActionResult Delete(int ZiyaretId)
        {
            var delete = ZiyaretRepository.GetById(ZiyaretId);
            ZiyaretRepository.Delete(delete);
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {

            List<SelectListItem> deger1 = (from i in db.Users.Where(x => x.Role == "Personel").ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.Name,
                                               Value = i.UserId.ToString()


                                           }).ToList();
            ViewBag.ktgr = deger1;
            List<SelectListItem> deger2 = (from i in db.Subes.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.Name,
                                               Value = i.SubeId.ToString()


                                           }).ToList();
            ViewBag.ktgr1 = deger2;
            return View();

        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Ziyaret p, ZiyaretGecmis zg)
        {
            List<SelectListItem> deger1 = (from i in db.Users.Where(x => x.Role == "Personel").ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.Name,
                                               Value = i.UserId.ToString()
                                           }).ToList();
            ViewBag.ktgr = deger1;
            List<SelectListItem> deger2 = (from i in db.Subes.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.Name,
                                               Value = i.SubeId.ToString()
                                           }).ToList();
            ViewBag.ktgr1 = deger2;
            if (ModelState.IsValid)
            {
                p.UpdateDate = DateTime.Now;
                p.CreateDate = DateTime.Now;
                p.DurumId = 1;

                ZiyaretRepository.Insert(p);



                var personel = db.Users.Where(x => x.UserId == p.UserId).SingleOrDefault();
                var mail = personel.Mail;

                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "bavcu292@gmail.com";
                WebMail.Password = "davrhokopgbsrmfb";
                WebMail.SmtpPort = 587;
                WebMail.Send(mail, subject: "Ziyaret Atandı", body: "<html><body><div style='max-width:756px;'><center>Yönetici Size Ziyare Atadı</center><br>" +
                  "Sevgili &nbsp; &nbsp;" + personel.Name + " &nbsp; &nbsp;" + p.CreateDate + "&nbsp; &nbsp; Tarihinde Yönetici size ziyaret atadı ziyaret ismi &nbsp; &nbsp;" + p.Name + "&nbsp; &nbsp; Ziyaret Gerçekleştirme Tarihi &nbsp; &nbsp;" + p.ZiyaretDate + "&nbsp; &nbsp;.</div></body> </html>");


                zg.ZiyaretId = p.ZiyaretId;
                zg.Not = p.Not;
                zg.DurumId = p.DurumId;
                zg.SubeId = p.SubeId;
                zg.Name = p.Name;
                zg.UserId = p.UserId;
                zg.GerceklesmeTarihi = DateTime.Now;
                zg.ZiyaretDate = p.ZiyaretDate;
                zg.Location = p.Location;
                ziyaretGecmisRepository.Insert(zg);

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Bir hata oluştu.");
            return View();


        }
        public ActionResult Update(int id)
        {
            List<SelectListItem> deger1 = (from i in db.Users.Where(x => x.Role == "Personel").ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.Name,
                                               Value = i.UserId.ToString()


                                           }).ToList();
            ViewBag.ktgr = deger1;
            List<SelectListItem> deger2 = (from i in db.Subes.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.Name,
                                               Value = i.SubeId.ToString()


                                           }).ToList();
            ViewBag.ktgr1 = deger2;

            List<SelectListItem> deger3 = (from i in db.durumogeleris.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.Durum,
                                               Value = i.DurumId.ToString()

                                           }).ToList();
            ViewBag.ktgr2 = deger3;
            var update = ZiyaretRepository.GetById(id);
            //var trh = update.ZiyaretDate;
            ViewBag.trh = update.ZiyaretDate.ToString("yyyy-MM-dd HH:mm:ss");
            //ViewBag.trh = update.ZiyaretDate;
            return View(update);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Update(Ziyaret data, ZiyaretGecmis zg)
        {
            List<SelectListItem> deger1 = (from i in db.Users.Where(x => x.Role == "Personel").ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.Name,
                                               Value = i.UserId.ToString()


                                           }).ToList();
            ViewBag.ktgr = deger1;
            List<SelectListItem> deger2 = (from i in db.Subes.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.Name,
                                               Value = i.SubeId.ToString()


                                           }).ToList();
            ViewBag.ktgr1 = deger2;
            List<SelectListItem> deger3 = (from i in db.durumogeleris.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.Durum,
                                               Value = i.DurumId.ToString()

                                           }).ToList();
            ViewBag.ktgr2 = deger3;
            var update = ZiyaretRepository.GetById(data.ZiyaretId);

            if (ModelState.IsValid)
            {
                update.UpdateDate = DateTime.Now;
                zg.GerceklesmeTarihi = DateTime.Now;
                update.Name = data.Name;
                zg.Name = data.Name;
                update.DurumId = data.DurumId;
                zg.DurumId = data.DurumId;
                DateTime t = new DateTime(1, 1, 1);

                if (data.ZiyaretDate == t)
                { //boş
                    update.ZiyaretDate = update.ZiyaretDate;
                    zg.ZiyaretDate = update.ZiyaretDate;
                }
                else
                {
                    update.ZiyaretDate = data.ZiyaretDate;
                    zg.ZiyaretDate = data.ZiyaretDate;
                }
                update.CreateDate = update.CreateDate;
                update.Location = data.Location;
                zg.Location = data.Location;
                update.UserId = data.UserId;
                zg.UserId = data.UserId;
                update.SubeId = data.SubeId;
                zg.SubeId = data.SubeId;
                update.Not = data.Not;
                zg.Not = data.Not;
                ZiyaretRepository.Update(update);
                ziyaretGecmisRepository.Insert(zg);

                var personel = db.Users.Where(x => x.UserId == update.UserId).SingleOrDefault();
                var mail = personel.Mail;

                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "bavcu292@gmail.com";
                WebMail.Password = "davrhokopgbsrmfb";
                WebMail.SmtpPort = 587;
                WebMail.Send(mail, subject: "Ziyaret Gücellendi", body: "<html><body><div style='max-width:756px;'><center>Yönetici Ziyareti Güncelledi</center><br>" +
                  "Sevgili &nbsp; &nbsp;" + personel.Name + " &nbsp; &nbsp;" + update.UpdateDate + "&nbsp; &nbsp; Tarihinde Yönetici Ziyareti Güncelledi ziyaret ismi &nbsp; &nbsp;" + update.Name + "&nbsp; &nbsp; Ziyaret Gerçekleştirme Tarihi &nbsp; &nbsp;" + update.ZiyaretDate + "&nbsp; &nbsp; Ziyaret Durumu" + update.Durumogeleri.Durum + ".</div></body> </html>");



                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Bir hata oluştu.");
            return View(update);
        }

        public ActionResult ZiyaretEdilecek(int sayfa = 1)
        {
            return View(ZiyaretRepository.List().ToPagedList(sayfa, 25));
        }
        public ActionResult OnayBekliyor(int sayfa = 1)
        {
            return View(ZiyaretRepository.List().ToPagedList(sayfa, 25));
        }
        public ActionResult OnaylanmadiZiyaretEdilecek(int sayfa = 1)
        {
            return View(ZiyaretRepository.List().ToPagedList(sayfa, 25));
        }
        public ActionResult Onaylandı(int sayfa = 1)
        {
            return View(ZiyaretRepository.List().ToPagedList(sayfa, 25));
        }

        ZiyaretGecmisRepository ZiyaretGecmisRepository = new ZiyaretGecmisRepository();
        public ActionResult History(int id, int sayfa = 1)
        {
            var list = db.ZiyaretGecmis.Where(x => x.ZiyaretId == id).ToList();

            return View(list.ToPagedList(sayfa, 25));
        }



    }
}