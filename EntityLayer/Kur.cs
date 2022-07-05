using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EntityLayer
{
    public static class Kur
    {
        public class _Doviz
        {
            public string CrossOrder { get; set; }
            public string Kod { get; set; }
            public string CurrencyCode { get; set; }
            public string Unit { get; set; }
            public string Isim { get; set; }
            public string CurrencyName { get; set; }
            public string ForexBuying { get; set; }
            public string ForexSelling { get; set; }
            public string BanknoteBuying { get; set; }
            public string BanknoteSelling { get; set; }
            public string CrossRateUSD { get; set; }
            public string CrossRateOther { get; set; }
        }
        public static double Deger(string paraBirimi)
        {
            double deger=0;
            if (paraBirimi == "TL")
                deger = 1;
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
                        deger = Convert.ToDouble(cn.ChildNodes[3].InnerXml.Replace(".",","));
                        return deger;

                    }
                }
            }
            return deger;
        }
    }
}
