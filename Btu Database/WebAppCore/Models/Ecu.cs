using System;
using System.Collections.Generic;

namespace WebAppCore.Models
{
    public partial class Ecu
    {
        public Ecu()
        {
            Simulator = new HashSet<Simulator>();
            Test = new HashSet<Test>();
        }

        public int EcuId { get; set; }
        public string EcuModel { get; set; }

        public ICollection<Simulator> Simulator { get; set; }
        public ICollection<Test> Test { get; set; }
    }
}
