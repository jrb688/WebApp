using System;
using System.Collections.Generic;

namespace WebAppCore.Models
{
    public partial class BatchTest
    {
        public int BatchId { get; set; }
        public int BatchVersion { get; set; }
        public int TestId { get; set; }
        public int TestVersion { get; set; }
        public int? Passed { get; set; }

        public Batch Batch { get; set; }
        public Test Test { get; set; }
    }
}
