using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mini_GPT.DTOs.Account;
using Mini_GPT.Interfaces;
using Mini_GPT.Models;
using System.Net;
using System.Transactions;

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


        [HttpGet("sendVerficationEmail")]
        public async Task<IActionResult> SendVerificationEmail(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    return BadRequest("Invalid Request!");
                }

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = token }, Request.Scheme);

                var emailBody = await GetEmailTemplate("emailVerification");

                emailBody = emailBody.Replace("{{confirmationLink}}", confirmationLink!);

                await _emailService.SendEmailAsync(user.Email!, "Confirm your email", emailBody);


                return Ok("Verification email has been sent to your email");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    var user = new AppUser
                    {
                        UserName = registerDto.Username,
                        Email = registerDto.Email,
                    };

                    // Create the user
                    var createdUser = await _userManager.CreateAsync(user, registerDto.Password);

                    if (!createdUser.Succeeded)
                        return BadRequest(createdUser.Errors);

                    // Add the user to the role
                    var roleResult = await _userManager.AddToRoleAsync(user, "USER");

                    if (!roleResult.Succeeded)
                        return BadRequest(roleResult.Errors);

                    // Queue the email sending as a background job
                    BackgroundJob.Enqueue(() => SendVerificationEmail(registerDto.Email!));

                    // Commit the transaction
                    scope.Complete();

                    // Return 201 Created with the new user data
                    return CreatedAtAction(nameof(Register), new { id = user.Id });
                }
                catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
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


        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok("User has been logged out");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("sendResetPasswordEmail")]
        [Authorize]
        public async Task<IActionResult> SendResetPassword([FromBody] SendResetPasswordDto resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(resetPassword.Email!);

                if (user == null)
                {
                    return BadRequest("Invalid Request!");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var resetLink = Url.Action("ResetPassword", "Auth", new { userEmail = user.Email, token = token}, Request.Scheme);
                
                var emailBody = await GetEmailTemplate("forgotPassword");

                emailBody = emailBody.Replace("{{resetLink}}", resetLink!);

                await _emailService.SendEmailAsync(user.Email!, "Reset Your Password", emailBody);

                return Ok("Reset password link has been sent to your email");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("resetPassword")]
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(resetPassword.Email!);

                if (user == null)
                {
                    return BadRequest("Invalid Request!");
                }

                if (resetPassword.NewPassword != resetPassword.ConfirmPassword)
                {
                    return BadRequest("Passwords do not match!");
                }

                var decodedToken = WebUtility.UrlDecode(resetPassword.Token);

                var result = await _userManager.ResetPasswordAsync(user, decodedToken!, resetPassword.NewPassword!);

                if (result.Succeeded)
                {
                    return Ok("Password has been reset successfully");
                }
                else
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return BadRequest(new { Errors = errors });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        internal async Task<string> GetEmailTemplate(string templateName)
        {
            var filePath = Path.Combine("Email Templates", $"{templateName}.html");
            return await System.IO.File.ReadAllTextAsync(filePath);
        }

    }
}
