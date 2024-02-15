using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonicAirLines.Classes
{
    public class AnotherUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string UserRole { get; set; }
        public string EmailAddress { get; set; }
        public string Office { get; set; }
        public string HiddenData { get; set; }
        public AnotherUser(User user, string Title)
        {
            Id = user.Id;
            int age = DateTime.Now.Year - user.Birthdate.Year;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Age = age;
            UserRole = user.RoleId == 1 ? "Admin" : "User";
            EmailAddress = user.Email;
            Office = Title;
            if (user.Active)
            {
                HiddenData = "0";
            }
            else
            {
                HiddenData = "1";
            }
        }
        public override string ToString()
        {
            return $"FirstName: {FirstName}, Last Name: {LastName}, Age: {Age}, User Role: {UserRole}, Email: {EmailAddress}, Office: {Office}";
        }
    }

}
