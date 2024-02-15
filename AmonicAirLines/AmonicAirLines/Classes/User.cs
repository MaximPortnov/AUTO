using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonicAirLines.Classes
{
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int OfficeId { get; set; }
        public DateTime Birthdate { get; set; }
        public bool Active { get; set; }
        public object Office { get; set; }
        public object Role { get; set; }
    }

}
