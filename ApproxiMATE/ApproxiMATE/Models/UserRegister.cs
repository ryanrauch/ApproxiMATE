using System;
using System.Collections.Generic;
using System.Text;

namespace ApproxiMATE.Models
{
    public class UserRegister
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateofBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
