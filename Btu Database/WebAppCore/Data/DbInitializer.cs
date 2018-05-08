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
            new User { FirstName = "Carson", LastName = "Alexander", Password = "123", Email = "Carson.Alexander@tester.com", Privilege = "Admin" }
            };
            foreach (User s in users)
            {
                context.User.Add(s);
            }

            var ecu = new Ecu[]
            {
                new Ecu{ EcuId = 0, EcuModel = "Model A" },
                new Ecu{ EcuId = 1, EcuModel = "Model B" },
                new Ecu{ EcuId = 2, EcuModel = "Model C" }
            };
            foreach (Ecu s in ecu)
            {
                context.Ecu.Add(s);
            }

            var simulators = new Simulator[]
            {
                new Simulator{SimId = 0, EcuId=0},
                new Simulator{SimId = 1, EcuId=1},
                new Simulator{SimId = 2, EcuId=2}
            };
            foreach (Simulator s in simulators)
            {
                context.Simulator.Add(s);
            }

            var batches = new Batch[]
            {
                new Batch{BatchId = 0, BatchVersion = 0, AuthorUserId=1, SimId=0, Name="Default", Status="Made", DateCreated=DateTime.Now, Display=0}
            };
            foreach (Batch s in batches)
            {
                context.Batch.Add(s);
            }

            var procedures = new Procedure[]
            {
                new Procedure{ProcId = 0, Description = "Take Off", Name = "Take Off", Script = "take_off.py"},
                new Procedure{ProcId = 1, Description = "Cruise", Name = "Cruise", Script = "cruise.py"},
                new Procedure{ProcId = 2, Description = "Land", Name = "Land", Script = "land.py"}
            };
            foreach (Procedure s in procedures)
            {
                context.Procedure.Add(s);
            }

            context.SaveChanges();
            return;
        }
    }
}