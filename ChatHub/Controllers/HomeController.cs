using ChatHub.Hubs;
using ChatHub.Models;
using ChatHub.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ChatHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHubContext<ChatHubs> _hubContext;

        public HomeController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IHubContext<ChatHubs> hubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Chat()
        {
            List<AppUser> users = _userManager.Users.ToList();
            return View(users);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser dbUser = _userManager.FindByNameAsync(login.Username).Result;
            if (dbUser == null)
            {
                ModelState.AddModelError("", "userrname or pasword is not valid");
            }
            Microsoft.AspNetCore.Identity.SignInResult result = _signInManager.PasswordSignInAsync(dbUser, login.Password, true, true).Result;

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "userrname or pasword is not valid");
            }
            await _signInManager.SignInAsync(dbUser, true);
            return RedirectToAction("chat", "home");
        }

        public IActionResult Register()
        {
            AppUser user1 = new AppUser { Fullname = "Elgun", UserName = "_elgun" };
            AppUser user2 = new AppUser { Fullname = "Ilkin", UserName = "_ilkin" };
            AppUser user3 = new AppUser { Fullname = "Qezenfer", UserName = "_qezenfer" };
            AppUser user4 = new AppUser { Fullname = "Ismayil", UserName = "_ismayil" };

            var resut1 = _userManager.CreateAsync(user1, "User@12345").Result;
            var resut2 = _userManager.CreateAsync(user2, "User@12345").Result;
            var resut3 = _userManager.CreateAsync(user3, "User@12345").Result;
            var resut4 = _userManager.CreateAsync(user4, "User@12345").Result;

            return Content("Users created");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("chat", "home");
        }

        public async Task<IActionResult> SendSpecificUser(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);

            await _hubContext.Clients.Client(user.ConnectionId).SendAsync("testweb", user.Fullname);

            return View();
        }
    }
}
