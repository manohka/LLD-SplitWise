using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace splitwisescaler.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public User(string name, string email)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Email = email;
        }
    }
}
