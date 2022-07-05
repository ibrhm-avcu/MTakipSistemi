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
    public class CustomerRepository:GenericRepository<Customer>
    {
        DataContext db = new DataContext();
        public List<Customer> Customers(int CustomerId)
        {
            return db.Customers.Where(x => x.CustomerId == CustomerId).ToList();
        }
    }
}
