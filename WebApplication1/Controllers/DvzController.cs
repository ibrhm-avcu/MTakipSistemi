using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using static EntityLayer.Kur;

namespace WebApplication1.Controllers
{
    public class DvzController : Controller
    {
        // GET: Dvz
        public ActionResult Index()
        {
            XmlDocument xml = new XmlDocument(); // yeni bir XML dökümü oluşturuyoruz.
            xml.Load("https://www.tcmb.gov.tr/kurlar/today.xml"); // bağlantı kuruyoruz.
            var Tarih_Date_Nodes = xml.SelectSingleNode("//Tarih_Date"); // Count değerini olmak için ana boğumu seçiyoruz.
            var CurrencyNodes = Tarih_Date_Nodes.SelectNodes("//Currency"); // ana boğum altındaki kur boğumunu seçiyoruz.
            int CurrencyLength = CurrencyNodes.Count; // toplam kur boğumu sayısını elde ediyor ve for döngüsünde kullanıyoruz.

            List<_Doviz> dovizler = new List<_Doviz>(); // Aşağıda oluşturduğum public class ile bir List oluşturuyoruz.
            for (int i = 0; i < CurrencyLength; i++) // for u çalıştırıyoruz.
            {
                var cn = CurrencyNodes[i]; // kur boğumunu alıyoruz.
                // Listeye kur bilgirini ekliyoruz.
                dovizler.Add(new _Doviz
                {
                    Kod = cn.Attributes["Kod"].Value,
                    CrossOrder = cn.Attributes["CrossOrder"].Value,
                    CurrencyCode = cn.Attributes["CurrencyCode"].Value,
                    Unit = cn.ChildNodes[0].InnerXml,
                    Isim = cn.ChildNodes[1].InnerXml,
                    CurrencyName = cn.ChildNodes[2].InnerXml,
                    ForexBuying = cn.ChildNodes[3].InnerXml,
                    ForexSelling = cn.ChildNodes[4].InnerXml,
                    BanknoteBuying = cn.ChildNodes[5].InnerXml,
                    BanknoteSelling = cn.ChildNodes[6].InnerXml,
                    CrossRateOther = cn.ChildNodes[8].InnerXml,
                    CrossRateUSD = cn.ChildNodes[7].InnerXml,
                });
            }

            ViewData["dovizler"] = dovizler; // dovizler List değerini data ya atıyoruz ön tarafta viewbag ile çekeceğiz.
            return View();
        }
 

        [HttpGet]
        public ActionResult KurDegeri(string paraBirimi)
        {
            string deger = "";
            if (paraBirimi == "TL")
                deger = "1";
            else
            {
                XmlDocument xml = new XmlDocument(); // yeni bir XML dökümü oluşturuyoruz.
                xml.Load("https://www.tcmb.gov.tr/kurlar/today.xml"); // bağlantı kuruyoruz.
                var Tarih_Date_Nodes = xml.SelectSingleNode("//Tarih_Date"); // Count değerini olmak için ana boğumu seçiyoruz.
                var CurrencyNodes = Tarih_Date_Nodes.SelectNodes("//Currency"); // ana boğum altındaki kur boğumunu seçiyoruz.
                int CurrencyLength = CurrencyNodes.Count; // toplam kur boğumu sayısını elde ediyor ve for döngüsünde kullanıyoruz.
                List<_Doviz> dovizler = new List<_Doviz>(); // Aşağıda oluşturduğum public class ile bir List oluşturuyoruz.
                for (int i = 0; i < CurrencyLength; i++) // for u çalıştırıyoruz.
                {
                    var cn = CurrencyNodes[i]; // kur boğumunu alıyoruz.
                                               // Listeye kur bilgirini ekliyoruz.

                    if (cn.Attributes["CurrencyCode"].Value == paraBirimi)
                    {
                        deger = cn.ChildNodes[3].InnerXml;
                        return Json(deger, JsonRequestBehavior.AllowGet);

                    }
                }
            }
            return Json(deger, JsonRequestBehavior.AllowGet);
        }
    }
}