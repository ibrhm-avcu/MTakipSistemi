using BusinessLayer.Concrete;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]

    public class SubeController : Controller
    {
        // GET: Sube
        SubeRepository subeRepository = new SubeRepository();
        public ActionResult Index(int sayfa = 1)
        {
            return View(subeRepository.List().ToPagedList(sayfa, 25));
        }
        public ActionResult Delete(int SubeId)
        {
            var delete = subeRepository.GetById(SubeId);
            subeRepository.Delete(delete);
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Sube p)
        {
            if (ModelState.IsValid)
            {
                subeRepository.Insert(p);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Bir hata oluştu.");
            return View();
        }
            public ActionResult Update(int id)
            {
                var update = subeRepository.GetById(id);
                return View(update);
            }

            [ValidateAntiForgeryToken]
            [HttpPost]
            public ActionResult Update(Sube data)
            {
            var update = subeRepository.GetById(data.SubeId);
            if (ModelState.IsValid)
            {
                update.Name = data.Name;
                subeRepository.Update(update);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Bir hata oluştu.");
            return View(update);

        }


        }
    }