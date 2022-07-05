using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class Doviz
    {
        [Key]
        public int DovizId { get; set; }
        public string DovizName { get; set; }
        public ICollection<Fatura>  Faturas { get; set; }
    }
}
