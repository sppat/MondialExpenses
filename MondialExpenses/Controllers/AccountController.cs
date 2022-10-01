using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MondialExpenses.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace MondialExpenses.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public IActionResult Users() => View(_userManager.Users);

        [AllowAnonymous]
        public IActionResult Login() => View(new LoginVM());

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Cashiers");
                }
                ModelState.AddModelError("", "Invalid User Name or Password");
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied() => View();

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        public IActionResult Create() => View(new RegisterVM());

        public IActionResult CookieDetails()
        {
            
            ViewBag.Cookie = Request.Cookies[".AspNetCore.Identity.Application"];
            
            return View();
        }

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
