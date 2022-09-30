using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MondialExpenses.ViewModels;

namespace MondialExpenses.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index() => View(_roleManager.Roles);

        public IActionResult Create() => View(new CreateRoleVM());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleVM createRoleVM)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = createRoleVM.Name
                };

                IdentityResult result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(role);

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);

            IList<IdentityUser> members = await _userManager.GetUsersInRoleAsync(role.Name);
            IEnumerable<IdentityUser> nonMembers = _userManager.Users.ToList().Except(members);

            EditRoleVM editRoleVM = new EditRoleVM
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };

            return View(editRoleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string userId)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            IdentityResult result = null;

            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                result = await _userManager.RemoveFromRoleAsync(user, role.Name);
            }
            else
            {
                result = await _userManager.AddToRoleAsync(user, role.Name);
            }

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }
    }
}
