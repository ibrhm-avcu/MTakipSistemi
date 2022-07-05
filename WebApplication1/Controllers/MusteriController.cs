using BusinessLayer.Concrete;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using DataAccessLayer.Context;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MusteriController : Controller
    {
        DataContext db = new DataContext();
        // GET: Musteri
        CustomerRepository customerRepository = new CustomerRepository();
        public ActionResult Index(int sayfa = 1)
        {
            return View(customerRepository.List().ToPagedList(sayfa, 25));
        }
        public ActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Customer p)
        {
            if (ModelState.IsValid)
            {
                customerRepository.Insert(p);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Bir hata oluştu.");
            return View();
        }
        public ActionResult Delete(int CustomerId)
        {
            var delete = customerRepository.GetById(CustomerId);
            customerRepository.Delete(delete);
            return RedirectToAction("Index");
        }

        public ActionResult Update(int id)
        {
            var update = customerRepository.GetById(id);
            return View(update);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Update(Customer data)
        {
            var update = customerRepository.GetById(data.CustomerId);
            if (ModelState.IsValid)
            {
                update.Name = data.Name;
                customerRepository.Update(update);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Bir hata oluştu.");
            return View(update);

        }


        public ActionResult Import()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Lütfen excel dosyasını seçiniz.";
                return View("Import");
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    var uzanti = Path.GetExtension(excelfile.FileName);
                    string yeniad = Guid.NewGuid() + uzanti;
                    string path = Server.MapPath("~/Content/import/" + yeniad);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelfile.SaveAs(path);
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    List<Customer> importcustomers = new List<Customer>();
                    int i = 0;
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        Customer p = new Customer();
                        p.Name = ((Excel.Range)range.Cells[row, 1]).Text;
                        if (p.Name == "")
                        {
                            
                            i++;
                        }
                        else
                        {
                            customerRepository.Insert(p);
                        }

                    }
                    int kayıtsayisiartıbir = range.Rows.Count;
                    int kayıtsayısı = kayıtsayisiartıbir - 1;

                    ViewBag.basarili = kayıtsayısı + " Adet Kayıt Başarıyla Eklendi.";
                    if (i > 0)
                    {
                        ViewBag.Error = i + "adet kayıt hatalı format yüzündfen eklenemedi;";
                    }

                    return View();
                }
                else
                {
                    ViewBag.Error = "Lütfen xls veya xlsx uzantılı bir excel dosyası seçiniz.";
                    return View("Import");
                }
            }
        }
    }
}