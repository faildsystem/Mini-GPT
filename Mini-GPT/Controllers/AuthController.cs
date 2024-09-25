using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Mini_GPT.DTOs.Account;
using Mini_GPT.Interfaces;
using Mini_GPT.Models;
using System.Text.Encodings.Web;

namespace Mini_GPT.Controllers
{
    [ApiController]
    [Route(("api/[controller]"))]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;



        public AuthController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _emailService = emailService;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                Console.WriteLine("lkjdflasjljla");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,

                };

                var createdUser = await _userManager.CreateAsync(user, registerDto.Password);
                if (!createdUser.Succeeded)
                    return BadRequest(createdUser.Errors);

                var roleResult = await _userManager.AddToRoleAsync(user, "USER");

                if (!roleResult.Succeeded)
                    return BadRequest(roleResult.Errors);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = token }, Request.Scheme);

                var emailBody = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>clicking here</a>.";
                await _emailService.SendEmailAsync(registerDto.Email, "Confirm your email", emailBody);

                // Return 201 Created with the new user data
                return CreatedAtAction(nameof(Register), new { id = user.Id });

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Find user by username
                var result = await _signInManager.PasswordSignInAsync(loginDto.Username, loginDto.Password, isPersistent: false, lockoutOnFailure: true);

                if (result.IsLockedOut)
                    return Unauthorized(new
                    {
                        message = "Your account is locked out due to too many failed attempts."
                    });

                if (!result.Succeeded)
                    return Unauthorized(new
                    {
                        message = "Invalid username or password."
                    });

                var user = await _userManager.FindByNameAsync(loginDto.Username);


                // Generate JWT token
                var token = _tokenService.CreateToken(user);



                // Return the login response with user data and token
                return Ok(new LoginResponseDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = token
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }


        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(result);
        }
    
    }
}
