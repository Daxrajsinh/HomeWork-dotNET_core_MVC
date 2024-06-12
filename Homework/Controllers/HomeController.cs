using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Homework.Models;
using Homework.Models.Repositories;
using Homework.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Homework.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClassroomRepository _classRepo;
        private readonly IInviteRepository _inviteRepo;
        private readonly IClassroomUserRepository _classroomUserRepo;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IClassroomRepository classRepo, IInviteRepository inviteRepo, IClassroomUserRepository classroomUserRepo, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _classRepo = classRepo;
            _classroomUserRepo = classroomUserRepo;
            _signInManager = signInManager;
            _userManager = userManager;
            _inviteRepo = inviteRepo;
            //this.userID = Convert.ToInt32(_userManager.GetUserId(User));
        }
        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                var id = _userManager.GetUserId(HttpContext.User);
                string user_email = _userManager.FindByIdAsync(id).Result.Email;
                HomeViewModel hvm = new HomeViewModel
                {
                    Invites = _inviteRepo.GetUserInvites(user_email),
                    UserClassrooms = _classroomUserRepo.GetUserClassrooms(id)
                };
                return View(hvm);
            }
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
    }
}
