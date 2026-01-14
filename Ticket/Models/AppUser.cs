using Microsoft.AspNetCore.Identity;

namespace Ticket.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
