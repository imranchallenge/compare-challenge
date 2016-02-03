using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompareIt4meChallenge.Models
{
    public class CustomerPhone
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string PhoneNumber { get; set; }
        public bool Active { get; set; }
    }
}