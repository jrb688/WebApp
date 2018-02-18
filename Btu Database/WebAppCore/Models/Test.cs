using System;
using System.Collections.Generic;

namespace WebAppCore.Models
{
    public partial class Test
    {
        public Test()
        {
            BatchTest = new HashSet<BatchTest>();
            Requirement = new HashSet<Requirement>();
            TestProc = new HashSet<TestProc>();
        }

        public int TestId { get; set; }
        public int TestVersion { get; set; }
        public int UserId { get; set; }
        public int EcuId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateRun { get; set; }

        public Ecu Ecu { get; set; }
        public User User { get; set; }
        public ICollection<BatchTest> BatchTest { get; set; }
        public ICollection<Requirement> Requirement { get; set; }
        public ICollection<TestProc> TestProc { get; set; }
    }
}
