using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace CompareIt4meChallenge.Models
{
    public class CustomerRepository : ICustomerPhoneRepository, IDisposable
    {
        ChallengeDb _db = new ChallengeDb();
        public IList<CustomerPhone> GetAllPhones()
        {
            return _db.CustomerPhones.ToList();
        }

        public IList<CustomerPhone> GetAllPhoneByCustomer(int customerId)
        {
            var customerPhones = _db.CustomerPhones.Where(c => c.CustomerId == customerId);

            return customerPhones.ToList();
        }

        public void ActivatePhoneNumber(int customerId,int phoneId)
        {
            var customerPhones = _db.CustomerPhones.Where(c => c.CustomerId == customerId);
            
            List<CustomerPhone> phones = customerPhones.ToList();

            for (int i = 0; i < phones.ToList().Count; i++)
            {
                   phones[i].Active = phones[i].Id == phoneId ? true : false;
                  _db.CustomerPhones.AddOrUpdate(phones[i]);
            }
            _db.SaveChanges();
        }

        public CustomerPhone GetCustomerPhone(int phoneId)
        {
            return _db.CustomerPhones.Find(phoneId);
        }
        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
            }
        }
    }
}