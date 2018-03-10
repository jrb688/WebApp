using System;
using System.Collections.Generic;

namespace WebAppCore.Models
{
    public partial class Batch
    {
        public Batch()
        {
            BatchTest = new HashSet<BatchTest>();
            TestProc = new HashSet<TestProc>();
        }

        public int BatchId { get; set; }
        public int BatchVersion { get; set; }
        public int AuthorUserId { get; set; }
        public int? TesterUserId { get; set; }
        public int SimId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateRun { get; set; }
        public int Display { get; set; }

        public User AuthorUser { get; set; }
        public Simulator Sim { get; set; }
        public User TesterUser { get; set; }
        public ICollection<BatchTest> BatchTest { get; set; }
        public ICollection<TestProc> TestProc { get; set; }
    }
}
