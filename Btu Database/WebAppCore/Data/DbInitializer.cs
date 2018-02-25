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
            new User { FirstName = "Carson", LastName = "Alexander", Password = "123", Email = "hi@me.com", Privilege = "tester" }
            };
            foreach (User s in users)
            {
                context.User.Add(s);
            }

            var ecu = new Ecu[]
            {
                new Ecu{ EcuModel = "Tester" }
            };
            foreach (Ecu s in ecu)
            {
                context.Ecu.Add(s);
            }

            var simulators = new Simulator[]
            {
                new Simulator{EcuId=0}
            };
            foreach (Simulator s in simulators)
            {
                context.Simulator.Add(s);
            }

            var batches = new Batch[]
            {
                new Batch{AuthorUserId=0, TesterUserId=0, SimId=0, Name="Hello", Status="Queued", DateCreated=DateTime.Now, Display=1}
            };
            foreach (Batch s in batches)
            {
                context.Batch.Add(s);
            }
            context.SaveChanges();
            return;
        }
    }
}