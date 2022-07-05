using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class Ziyaret
    {
        [Key]
        public int ZiyaretId { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan")]
        [StringLength(25, ErrorMessage = "Max 25 Karakter olabilir.")]
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public DateTime ZiyaretDate { get; set; }

        public string image { get; set; }
        [StringLength(25, ErrorMessage = "Max 25 Karakter olabilir.")]
        [Required(ErrorMessage = "Zorunlu Alan")]
        public string Location { get; set; }
        public string Not { get; set; }
        public int DurumId { get; set; }

        public int UserId { get; set; }
        public int SubeId { get; set; }
        public virtual durumogeleri Durumogeleri { get; set; }
        public virtual Sube Sube { get; set; }
        public virtual User User { get; set; }
    }
}
