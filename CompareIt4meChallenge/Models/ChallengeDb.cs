using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CompareIt4meChallenge.Models
{
    public class ChallengeDb : DbContext
    {
        public ChallengeDb()
            : base("name=DefaultConnection")
        {

        }
        public DbSet<Person> People { get; set; }
        public DbSet<CustomerPhone> CustomerPhones { get; set; }
    }
}