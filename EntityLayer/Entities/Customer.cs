using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
   public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required(ErrorMessage ="Zorunlu Alan")]
        [StringLength(25, ErrorMessage = "Max 25 Karakter olabilir.")]
        public string Name { get; set; }

        public virtual List<Fatura> Faturas { get; set; }

    }
}
