using BusinessLayer.Abstract;
using DataAccessLayer.Context;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class SatisRepository:GenericRepository<Fatura>
    {
        DataContext db = new DataContext();
        public List<Fatura> Faturas(int FaturaId)
        {
            return db.Faturas.Where(x => x.FaturaId == FaturaId).ToList();
        }
    }
}
