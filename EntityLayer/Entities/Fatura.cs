using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class Fatura
    {
        [Key]
        public int FaturaId { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan")]
        [StringLength(25, ErrorMessage = "Max 25 Karakter olabilir.")]

        public string ProductName { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        public DateTime FaturaKesimTarihi { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan")]
        public int Miktar { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan")]
        public int Adet { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan")]
        public int DovizId { get; set; }
        public virtual Doviz Doviz { get; set; }
        public double DovizFiyat { get; set; }
        public double Toplamtutar { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }



    }
}
