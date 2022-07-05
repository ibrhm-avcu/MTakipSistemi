using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            //Ziyaret = new HashSet<Ziyaret>();
            ZiyaretGecmis = new HashSet<ZiyaretGecmis>();
        }

        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan")]
        [StringLength(25, ErrorMessage = "Max 25 Karakter olabilir.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan")]
        [StringLength(25, ErrorMessage = "Max 25 Karakter olabilir.")]
        public string SurName { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan")]
        public string Password { get; set; }
        public string Role { get; set; }
        public bool Dogrulama { get; set; }
        public string BackPassword { get; set; }
        public virtual List<Ziyaret> Ziyarets { get; set; }
        public virtual List<Fatura> Faturas { get; set; }

        public virtual ICollection<ZiyaretGecmis> ZiyaretGecmis { get; set; }

    }
}
