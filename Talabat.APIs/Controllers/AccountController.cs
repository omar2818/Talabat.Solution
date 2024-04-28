using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthService _authService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAuthService authService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user =  await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                return Unauthorized(new ApiResponse(401, "Invalid Login"));
            }

            var Result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!Result.Succeeded)
            {
                return Unauthorized(new ApiResponse(401, "Invalid Login"));
            }

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var user = new ApplicationUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.Phone
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if(! result.Succeeded)
            {
                return BadRequest(new ApiVallidationErrorResponse() { Errors = result.Errors.Select(E => E.Description)});
            }

            return Ok(new UserDto()
            {
                DisplayName= model.DisplayName,
                Email = model.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }
    }
}
