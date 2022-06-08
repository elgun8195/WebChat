using ChatHub.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ChatHub.Hubs
{
    public class ChatHubs : Hub
    {
        private readonly UserManager<AppUser> _userManager;
        public ChatHubs(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync
                ("RecieveMesssage", user, message, DateTime.Now.ToString("dddd,dd MMMM yyyy"));
        }
        public override Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser user = _userManager.FindByNameAsync
                    (Context.User.Identity.Name).Result;
                user.ConnectionId = Context.ConnectionId;
                var result= _userManager.UpdateAsync(user).Result;
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser user = _userManager.FindByNameAsync
                    (Context.User.Identity.Name).Result;
                user.ConnectionId = null;
                var result = _userManager.UpdateAsync(user).Result;
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
