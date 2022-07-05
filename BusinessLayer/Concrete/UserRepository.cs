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
   public class UserRepository:GenericRepository<User>
    {
        DataContext db = new DataContext();
        public List<User> Users(int UserId)
        {
            return db.Users.Where(x => x.UserId == UserId).ToList();
        }
    }
}
