using System;
using System.Collections.Generic;

namespace WebAppCore.Models
{
    public partial class User
    {
        public User()
        {
            BatchAuthorUser = new HashSet<Batch>();
            BatchTesterUser = new HashSet<Batch>();
            Test = new HashSet<Test>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Privilege { get; set; }

        public ICollection<Batch> BatchAuthorUser { get; set; }
        public ICollection<Batch> BatchTesterUser { get; set; }
        public ICollection<Test> Test { get; set; }
    }
}
