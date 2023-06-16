using AspNetCoreHero.ToastNotification.Abstractions;
using BizcomTaskMVC.Entities;
using BizcomTaskMVC.Models.ViewModels;
using BizcomTaskMVC.Repository.IRepository;
using BizcomTaskMVC.Statics;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BizcomTaskMVC.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly INotyfService _notyfService;
    private readonly IUnitOfWork _unitOfWork;

    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<AppRole> roleManager,
        INotyfService notyfService,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _notyfService = notyfService;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Register() => View(new RegisterUserVM());

    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserVM registerUserVM)
    {
        if (!ModelState.IsValid) return View(registerUserVM);

        var isExistEmail = IsEmailExist(registerUserVM.Email!);
        var isExistPhoneNumber = IsPhoneNumberExist(registerUserVM.PhoneNumber!);

        if (isExistEmail || isExistPhoneNumber)
        {
            if(isExistEmail) 
                ModelState.AddModelError("Email", "Email already used");
            if(isExistPhoneNumber) 
                ModelState.AddModelError("PhoneNumber", "Phone Number already used");

            return View(registerUserVM);
        }

        var newUser = registerUserVM.Adapt<AppUser>();
        var response = await _userManager.CreateAsync(newUser, registerUserVM.Password!);

        if (response.Succeeded)
        {
            if (await _roleManager.RoleExistsAsync(registerUserVM.Role!) == false)
                await _roleManager.CreateAsync(new AppRole { Name = registerUserVM.Role });

            await _userManager.AddToRoleAsync(newUser, registerUserVM.Role!);
            
            _notyfService.Success("Registration completed successfully");

            if (registerUserVM.Role == UserRoles.Student)
            {
                newUser.StudentRegNumber = await GetStudentRegNumberAsync();
                await _unitOfWork.SaveChangesAsync();
            }

            return View(nameof(Login));
        }

        foreach (var identityError in response.Errors)
        {
            ModelState.AddModelError("Registration Error", identityError.Description);
        }

        return View(registerUserVM);
    }

    public IActionResult Login() => View(new LoginUserVM());

    [HttpPost]
    public async Task<IActionResult> Login(LoginUserVM loginUserVM)
    {
        if (!ModelState.IsValid) return View(loginUserVM);

        var user = await _userManager.FindByEmailAsync(loginUserVM.Email!);

        if (user != null)
        {
            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginUserVM.Password!);

            if (passwordCheck)
            {
                var result = await _signInManager
                    .PasswordSignInAsync(user, loginUserVM.Password!, false, false);

                if (result.Succeeded)
                {
                    _notyfService.Success("Loggedin successfully");

                    return RedirectToAction(nameof(Profile));
                }
            }

            _notyfService.Error("Wrong credentials. Please, try again!");
            
            return View(loginUserVM);
        }

        _notyfService.Error("Wrong credentials. Please, try again!");

        return View(loginUserVM);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);

        return View(user ?? new AppUser());
    }

    private bool IsEmailExist(string email)
    {
        var user = _unitOfWork.AppUserRepository.Get(u => u.Email == email);
        
        return user != null;
    }

    private bool IsPhoneNumberExist(string phoneNumber)
    {
        var user = _unitOfWork.AppUserRepository.Get(u => u.PhoneNumber == phoneNumber);

        return user != null;
    }

    private async Task<int> GetStudentRegNumberAsync()
    {
        var count = 1;
        var users = _unitOfWork.AppUserRepository.GetAll();

        foreach (var user in users)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            if(userRoles.Contains(UserRoles.Student))
                count++;
        }

        return count;
    }
}