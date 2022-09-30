using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MondialExpenses.ViewModels;

namespace MondialExpenses.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<IdentityUser> userManager, IMapper _mapper)
        {
            _userManager = userManager;
            this._mapper = _mapper;
        }

        public IActionResult Users() => View(_userManager.Users);

        public IActionResult Create() => View(new RegisterVM());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = registerVM.UserName,
                    Email = registerVM.Email,
                };

                IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Users));
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            EditUserVM editUserVm = _mapper.Map<EditUserVM>(user);

            return View(editUserVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserVM editUserVM)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (ModelState.IsValid)
            {
                user.UserName = editUserVM.UserName;
                user.Email = editUserVM.Email;
                IdentityResult result = await _userManager.UpdateAsync(user);

                if (result.Succeeded && !String.IsNullOrEmpty(editUserVM.Password))
                {
                    await _userManager.RemovePasswordAsync(user);
                    await _userManager.AddPasswordAsync(user, editUserVM.Password);
                }

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Users));
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
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction(nameof(Users));
        }

    }
}
