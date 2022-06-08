using Microsoft.AspNetCore.Identity;

namespace ChatHub.ViewModels
{
    public class AppUser:IdentityUser
    {
        public string Fullname { get; set; }
        public string  ConnectionId { get; set; }
    }
}
