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
    public class SubeRepository:GenericRepository<Sube>
    {
        DataContext db = new DataContext();
        public List<Sube> Subes(int SubeId)
        {
            return db.Subes.Where(x => x.SubeId == SubeId).ToList();

        }
    }
}
