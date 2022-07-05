using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class ZiyaretGecmis
    {
     

        [Key]
        public int ZiyaretGecmisId { get; set; }
        public string Name { get; set; }
        public DateTime GerceklesmeTarihi { get; set; }
        public DateTime ZiyaretDate { get; set; }
        public string Location { get; set; }
        public string Not { get; set; }
        public string image { get; set; }

        public int DurumId { get; set; }
        public virtual durumogeleri Durumogeleri { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; } 

        public int SubeId { get; set; }
        public virtual Sube Sube { get; set; } 
        public int ZiyaretId { get; set; }
        public virtual Ziyaret Ziyaret { get; set; }
    }
}
