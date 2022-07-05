using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class Sube
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sube()
        {
            Ziyaret= new HashSet<Ziyaret>();
            ZiyaretGecmis = new HashSet<ZiyaretGecmis>();
        }

        [Key]
        public int SubeId { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan")]
        [StringLength(25, ErrorMessage = "Max 25 Karakter olabilir.")]
        public string Name { get; set; }

        //public virtual List<Ziyaret> Ziyarets { get; set; }        



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ZiyaretGecmis> ZiyaretGecmis { get; set; }
        public virtual ICollection<Ziyaret> Ziyaret { get; set; }


    }
}
