using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace CompareIt4meChallenge.Models
{
    public interface ICustomerPhoneRepository
    {
        IList<CustomerPhone> GetAllPhones();

        IList<CustomerPhone> GetAllPhoneByCustomer(int customerId);

        void ActivatePhoneNumber(int customerId,int phoneId);

        CustomerPhone GetCustomerPhone(int phoneId);

    }
}