using BusinessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
namespace WebApplication1.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminSatisController : Controller
    {
        // GET: AdminSatis
        SatisRepository satisRepository = new SatisRepository();
        public ActionResult Index(int sayfa = 1)
        {
            return View(satisRepository.List().ToPagedList(sayfa, 25));
        }
    }
}