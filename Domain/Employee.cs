using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class Employee : IdentityUser
    {
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
    }
}