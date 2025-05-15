using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using RASPortal.Models;
using System.Threading.Tasks;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace RASPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _env;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return string.IsNullOrEmpty(returnUrl)
                        ? RedirectToAction("Index", "Home")
                        : LocalRedirect(returnUrl);
                }
                ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
                return View(model);
            }
            return View(model);
        }

        // POST: /Account/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if a user with the same username exists
                var existingUser = await _userManager.FindByNameAsync(model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Bu kullanıcı adı zaten kullanılıyor.");
                    return View(model);
                }

                var newUser = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    // Initially set file paths to empty strings.
                    ProfilePictureFilePath = string.Empty,
                    ResumeFilePath = string.Empty
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    // Optionally add the default role "User"
                    await _userManager.AddToRoleAsync(newUser, "User");
                    return RedirectToAction("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // GET: /Account/Profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            // Check if the stored file paths point to existing files.
            bool updated = false;
            if (!string.IsNullOrEmpty(user.ProfilePictureFilePath))
            {
                var picFullPath = Path.Combine(_env.WebRootPath, user.ProfilePictureFilePath.TrimStart('/'));
                if (!System.IO.File.Exists(picFullPath))
                {
                    user.ProfilePictureFilePath = string.Empty;
                    updated = true;
                }
            }
            if (!string.IsNullOrEmpty(user.ResumeFilePath))
            {
                var resumeFullPath = Path.Combine(_env.WebRootPath, user.ResumeFilePath.TrimStart('/'));
                if (!System.IO.File.Exists(resumeFullPath))
                {
                    user.ResumeFilePath = string.Empty;
                    updated = true;
                }
            }
            if (updated)
            {
                await _userManager.UpdateAsync(user);
            }

            // Map ApplicationUser to ProfileViewModel
            var model = new ProfileViewModel
            {
                // Note: IdentityUser's Id is a string. Adjust ProfileViewModel.Id type if needed.
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                ProfilePictureFilePath = user.ProfilePictureFilePath,
                ResumeFilePath = user.ResumeFilePath
            };

            return View(model);
        }

        // POST: /Account/Profile
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            // Update allowed fields (Username and Email are read-only)
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.DateOfBirth = model.DateOfBirth;
            user.Gender = model.Gender;

            // Process profile picture upload
            if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
            {
                // Delete existing profile picture file if exists
                if (!string.IsNullOrEmpty(user.ProfilePictureFilePath))
                {
                    var existingPicPath = Path.Combine(_env.WebRootPath, user.ProfilePictureFilePath.TrimStart('/'));
                    if (System.IO.File.Exists(existingPicPath))
                        System.IO.File.Delete(existingPicPath);
                }
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "profile_pictures");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfilePictureFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePictureFile.CopyToAsync(stream);
                }
                user.ProfilePictureFilePath = "/uploads/profile_pictures/" + uniqueFileName;
            }

            // Process resume file upload
            if (model.ResumeFile != null && model.ResumeFile.Length > 0)
            {
                // Delete existing resume file if exists
                if (!string.IsNullOrEmpty(user.ResumeFilePath))
                {
                    var existingResumePath = Path.Combine(_env.WebRootPath, user.ResumeFilePath.TrimStart('/'));
                    if (System.IO.File.Exists(existingResumePath))
                        System.IO.File.Delete(existingResumePath);
                }
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "resumes");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ResumeFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ResumeFile.CopyToAsync(stream);
                }
                user.ResumeFilePath = "/uploads/resumes/" + uniqueFileName;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }
            return RedirectToAction("Profile");
        }
    }
}
