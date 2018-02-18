using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Models;

namespace WebAppCore.Data
{
    public class DbInitializer
    {
        public static void Initialize(Btu_DatabaseContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.User.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
            new User{FirstName="Carson",LastName="Alexander",Password="123",Email="hi@me.com", Privilege="tester"}
            };
            foreach (User s in users)
            {
                context.User.Add(s);
            }
            context.SaveChanges();
        }
    }
}