using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class durumogeleri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public durumogeleri()
        {
            ZiyaretGecmis = new HashSet<ZiyaretGecmis>();
        }


        [Key]
        public int DurumId { get; set; }
        [Required(ErrorMessage ="Zorunlu Alan")]
        public string Durum { get; set; }
        public virtual ICollection<Ziyaret> Ziyarets { get; set; }        
        public virtual ICollection<ZiyaretGecmis> ZiyaretGecmis { get; set; }
    }
}
