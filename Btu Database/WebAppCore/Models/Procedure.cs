using System;
using System.Collections.Generic;

namespace WebAppCore.Models
{
    public partial class Procedure
    {
        public Procedure()
        {
            TestProc = new HashSet<TestProc>();
        }

        public int ProcId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Script { get; set; }

        public ICollection<TestProc> TestProc { get; set; }
    }
}
