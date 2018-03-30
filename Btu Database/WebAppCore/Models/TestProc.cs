using System;
using System.Collections.Generic;

namespace WebAppCore.Models
{
    public partial class TestProc
    {
        public int BatchId { get; set; }
        public int BatchVersion { get; set; }
        public int TestId { get; set; }
        public int TestVersion { get; set; }
        public int ProcId { get; set; }
        public int? ReqId { get; set; }
        public string Parameters { get; set; }
        public int? Passed { get; set; }
        public int Order { get; set; }

        public Batch Batch { get; set; }
        public Procedure Proc { get; set; }
        public Requirement Req { get; set; }
        public Test Test { get; set; }
    }
}
