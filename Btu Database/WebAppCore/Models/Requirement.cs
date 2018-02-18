using System;
using System.Collections.Generic;

namespace WebAppCore.Models
{
    public partial class Requirement
    {
        public Requirement()
        {
            TestProc = new HashSet<TestProc>();
        }

        public int ReqId { get; set; }
        public int TestId { get; set; }
        public int TestVersion { get; set; }
        public string Description { get; set; }

        public Test Test { get; set; }
        public ICollection<TestProc> TestProc { get; set; }
    }
}
