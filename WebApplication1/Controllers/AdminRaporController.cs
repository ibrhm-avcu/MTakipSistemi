using BusinessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminRaporController : Controller
    {
        // GET: AdminRapor
        ZiyaretRepository ZiyaretRepository = new ZiyaretRepository();
        UserRepository userRepository = new UserRepository();
        CustomerRepository customerRepository = new CustomerRepository();   
        SatisRepository satisRepository = new SatisRepository();    
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexList()
        {
            return View(ZiyaretRepository.List());
        }
        public ActionResult ZiyaretEdilecek()
        {
            return View(ZiyaretRepository.List());
        }
        public ActionResult OnayBekliyor()
        {
            return View(ZiyaretRepository.List());
        }
        public ActionResult OnaylanmadiZiyaretEdilecek()
        {
            return View(ZiyaretRepository.List());
        }
        public ActionResult Onaylandı()
        {
            return View(ZiyaretRepository.List());
        }
        public ActionResult Personeller()
        {
            return View(userRepository.List());
        }
        public ActionResult Musteriler()
        {
            return View(customerRepository.List());
        }
        public ActionResult Faturalar()
        {
            return View(satisRepository.List());
        }
    
    }
}