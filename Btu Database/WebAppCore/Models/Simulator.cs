using System;
using System.Collections.Generic;

namespace WebAppCore.Models
{
    public partial class Simulator
    {
        public Simulator()
        {
            Batch = new HashSet<Batch>();
        }

        public int SimId { get; set; }
        public int EcuId { get; set; }

        public Ecu Ecu { get; set; }
        public ICollection<Batch> Batch { get; set; }
    }
}
