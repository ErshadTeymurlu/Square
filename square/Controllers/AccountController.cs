using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Square.Models;
using Square.ViewModels;

namespace Square.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager { get; }
        private SignInManager<AppUser> _signInManager { get; }
        private RoleManager<IdentityRole> _roleManager { get; }
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [Authorize]
        public IActionResult MyAccount()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser loggingUser = await _userManager.FindByEmailAsync(lvm.Email);
            if (!loggingUser.Status)
            {
                ModelState.AddModelError("", "This user have been blocked!");
                return View(lvm);
            }
            if (loggingUser == null)
            {
                ModelState.AddModelError("", "Email Or Password is not correct! Please, try again.");
                return View(lvm);
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loggingUser, lvm.Password,lvm.StayLoggedIn,true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "You are locked out! Please try again after 5 minutes");
                    return View(lvm);
                }
                ModelState.AddModelError("", "Email Or Password is not correct! Please, try again.");
                return View(lvm);
            }

            if((await _userManager.GetRolesAsync(loggingUser)).Count>0 &&(await _userManager.GetRolesAsync(loggingUser))[0] == "Admin")
            {
                return RedirectToAction("Index", "Home", new {area = "Admin"});
            }
            return RedirectToAction("Index","Home");

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel rvm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser appUser = new AppUser()
            {
                Name = rvm.Name,
                SurName = rvm.Surname,
                Age = rvm.Age,
                Email = rvm.Email,
                UserName = rvm.Email.Split('@')[0]
            };
            IdentityResult result = await _userManager.CreateAsync(appUser, rvm.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(rvm);
            }
            await _signInManager.SignInAsync(appUser, false);
            return RedirectToAction("Index", "Home");
        }
        //public async Task<IActionResult> CreateRoles()
        //{
        //    IdentityRole member = new IdentityRole()
        //    {
        //        Name = "Member"
        //    };
        //    IdentityRole admin = new IdentityRole()
        //    {
        //        Name = "Admin"
        //    };
        //    await _roleManager.CreateAsync(member);
        //    await _roleManager.CreateAsync(admin);
        //    return RedirectToAction("Index", "Home");
        //}

        public async Task<IActionResult> AddRoles()
        {
            AppUser userToBeMember = await _userManager.FindByNameAsync("HikmetMusayev");
            await _userManager.AddToRoleAsync(userToBeMember, "Admin");
            return Content("ok");
        }
    }
}
