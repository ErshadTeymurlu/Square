using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Square.Models;

namespace Square.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private UserManager<AppUser> _userManager { get; }
        private SignInManager<AppUser> _singinManager { get; }
        private RoleManager<IdentityRole> _roleManager { get; }
        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _singinManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }
        public async Task<IActionResult> Block(string Id)
        {
            AppUser userTobeBlocked = await _userManager.FindByIdAsync(Id);
            userTobeBlocked.Status = false;
            await _userManager.UpdateAsync(userTobeBlocked);
            return RedirectToAction("index", "User");
        }
        public async Task<IActionResult> UnBlock(string Id)
        {
            AppUser userTobeUnblocked = await _userManager.FindByIdAsync(Id);
            userTobeUnblocked.Status = true;
            await _userManager.UpdateAsync(userTobeUnblocked);
            return RedirectToAction("index", "User");
        }
        public async Task<IActionResult> ManageRoles(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> ManageRoles(string id, bool setAdmin, bool setMember)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            if (setAdmin)
            {
                await _userManager.AddToRoleAsync(user, "Admin");

            }
            if (setMember)
            {
                await _userManager.AddToRoleAsync(user, "Member");
            }
            return RedirectToAction("Index", "User");
        }

        public async Task<IActionResult> ResetPassword(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string id, string newPassword)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded) return RedirectToAction("Index", "Home");
            else
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(user);
            }
        }
    }
}
