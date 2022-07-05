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
    public class ZiyaretGecmisRepository:GenericRepository<ZiyaretGecmis>
    {
        DataContext db = new DataContext();
        public List<ZiyaretGecmis> ZiyaretGecmis(int ZiyaretId)
        {
            return db.ZiyaretGecmis.Where(x => x.ZiyaretId == ZiyaretId).ToList();
        }
    }
}
