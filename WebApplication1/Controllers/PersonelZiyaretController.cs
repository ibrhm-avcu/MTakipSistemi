using BusinessLayer.Concrete;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Web.Helpers;
using DataAccessLayer.Context;
using System.Drawing;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Personel")]

    public class PersonelZiyaretController : Controller
    {
        DataContext db = new DataContext();
        // GET: PersonelZiyaret
        ZiyaretRepository ZiyaretRepository = new ZiyaretRepository();
        ZiyaretGecmisRepository ziyaretGecmisRepository = new ZiyaretGecmisRepository();
        public ActionResult Index(int sayfa = 1)
        {
            return View(ZiyaretRepository.List().ToPagedList(sayfa, 25));
        }
        public ActionResult Update(int id)
        {
            var update = ZiyaretRepository.GetById(id);
            return View(update);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Update(Ziyaret data, HttpPostedFileBase File , ZiyaretGecmis zg)
        {
            string[] resimUzantilari = new string[] { "image/jpeg", "image/png" };
            var update = ZiyaretRepository.GetById(data.ZiyaretId);
            if (ModelState.IsValid)
            {
                if (File == null)
                {

                }
                else
                {
                    if (!resimUzantilari.Contains(File.ContentType))
                    {
                        ViewBag.hata = "Sadece jpg ve png uzantılı resim ekleyebilirsiniz.";
                        return View(update);
                    }
                    else
                    {
                        if (File.ContentLength > 2097152)
                        {
                            ViewBag.hata = "En Fazla 2mb boyutunda bir dosya yükleyebilirsiniz.";
                            return View(update);
                        }
                        else
                        {
                            System.Drawing.Image resim = System.Drawing.Image.FromStream(File.InputStream);
                            if (resim.Width <= 1920 && resim.Height <= 1080)
                            {
                                var uzanti = Path.GetExtension(File.FileName);
                                string resimad = Guid.NewGuid() + uzanti;
                                update.image = resimad;
                                zg.image = resimad;
                                string path = Path.Combine("~/Content/images/" + resimad);
                                File.SaveAs(Server.MapPath(path));
                            }
                            else
                            {
                                var uzanti = Path.GetExtension(File.FileName);
                                string resimad = Guid.NewGuid() + uzanti;
                                update.image = resimad;
                                zg.image = resimad;
                                string path = Path.Combine("~/Content/images/" + resimad);

                                WebImage webImage = new WebImage(File.InputStream);
                                decimal width = webImage.Width;
                                decimal height = webImage.Height;
                                decimal rate = width / height;
                                if (rate < 1.70m || rate > 1.85m)
                                {
                                    var newHeight = width * (0.5625m);
                                    webImage.Resize((int)width, (int)newHeight, false, false);
                                }
                                webImage.Save(Server.MapPath(path));
                            }
                        }
                    }
                }
                update.UpdateDate = DateTime.Now;
                zg.GerceklesmeTarihi = DateTime.Now;
                update.DurumId = 2;
                zg.DurumId = 2;
                update.Not = data.Not;
                zg.Not = data.Not;
                update.Location = data.Location;
                zg.Location = data.Location;
                zg.ZiyaretDate = update.ZiyaretDate;
                ZiyaretRepository.Update(update);
                zg.UserId = update.UserId;
                zg.SubeId = update.SubeId;
                ziyaretGecmisRepository.Insert(zg);
                var yonetici = db.Users.Where(x => x.Role == "Admin").SingleOrDefault();
                var mail = yonetici.Mail;
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "bavcu292@gmail.com";
                WebMail.Password = "davrhokopgbsrmfb";
                WebMail.SmtpPort = 587;
                WebMail.Send(mail, subject:"Ziyaret Gerçekleştirildi", body:"<html><body><div style='max-width:756px;'><center>Personel Ziyareti Gerçekleştirdi</center><br>" +
                    update.User.Name + " &nbsp; &nbsp;Adlı Personel &nbsp; &nbsp;" + update.Name + "&nbsp; &nbsp; isimli ziyareti gerçekleştirdi onay vermenizi bekliyor.</div></body> </html>");
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

        public ActionResult Filtre()
        {
          
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
            if (CreateDate != null)
            {
                a = data.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (ZiyaretDate != null)
            {
                b = data.ZiyaretDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            int filt = 0;

    

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
            if (SubeId == null && DurumId == null && CreateDate == null && ZiyaretDate == null)
            {
                sql = $"Select * from Ziyarets  ";
            }
            var ls = db.Ziyarets.SqlQuery(sql).ToPagedList(sayfa, 25);
            return View(ls);
        }

        public ActionResult Ara(string p)
        {
            var ziyarets = from x in db.Ziyarets select x;
            if (!string.IsNullOrEmpty(p))
            {
                ziyarets = ziyarets.Where(x => x.Name.ToLower().Contains(p.ToLower()) || x.ZiyaretId.ToString().Contains(p.ToLower()) || x.Location.ToLower().Contains(p.ToLower())
                || x.Durumogeleri.Durum.ToLower().Contains(p.ToLower()) || x.Sube.Name.ToLower().Contains(p.ToLower()));
            }


            return View(ziyarets.ToList());
        }
    }
}