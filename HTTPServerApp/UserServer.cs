using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServerApp
{
    public class UserServer
    {
        public static int staticID = 1;
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Age { get; set; }
        public UserServer()
        {
            Id = staticID++;
        }
        public override string ToString() => $"{Id} - {FirstName} - {LastName} - {Age}";

    }
}
