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
    public class ZiyaretRepository:GenericRepository<Ziyaret>
    {
        DataContext db = new DataContext();
        public List<Ziyaret> Ziyarets(int id)
        {
            return db.Ziyarets.Where(x=> x.ZiyaretId == id).ToList();

        }
    }
}
