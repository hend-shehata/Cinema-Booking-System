
using Cinema_Booking_System.Data;
using Cinema_Booking_System.Models;
using Cinema_Booking_System.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Cinema_Booking_System.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _context = context;
        }

       

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            await SendConfirmationEmail(user);

            return RedirectToAction(nameof(ConfirmationSent));
        }


        private async Task SendConfirmationEmail(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = Url.Action(
                nameof(ConfirmEmail),
                "Account",
                new { area = "Identity", userId = user.Id, token },
                Request.Scheme);

            var message = $"""
                <h2>Welcome to CINÉMA!</h2>
                <p>Please confirm your email address.</p>
                <a href="{confirmationLink}">Confirm Email</a>
                """;

            await _emailSender.SendEmailAsync(
                user.Email!,
                "Confirm Your Email",
                message);
        }

        [HttpGet]
        public IActionResult ConfirmationSent() => View();

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return View("EmailConfirmed");

            return View("EmailConfirmationFailed");
        }


        [HttpGet]
        public IActionResult ResendEmailConfirmation()
            => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendEmailConfirmation(
            ResendEmailConfirmationVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && !user.EmailConfirmed)
                await SendConfirmationEmail(user);

            return View("ConfirmationSent");
        }


        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.UserNameOrEmail)
                       ?? await _userManager.FindByNameAsync(model.UserNameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username/email or password.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home", new { area = "Client" });

            if (result.IsNotAllowed)
            {
                ModelState.AddModelError("",
                    "Please confirm your email before signing in.");

                return View(model);
            }

            ModelState.AddModelError("", "Invalid username/email or password.");
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home", new { area = "Client" });
        }

        // FORGET PASSWORD - SEND OTP

        [HttpGet]
        public IActionResult ForgetPassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !user.EmailConfirmed)
                return View("OtpSent");

            var oldOtps = await _context.ApplicationUserOtps
                .Where(x => x.ApplicationUserId == user.Id && x.IsValid)
                .ToListAsync();

            foreach (var oldOtp in oldOtps)
                oldOtp.IsValid = false;

            var otp = RandomNumberGenerator.GetInt32(100000, 1000000)
                .ToString();

            var newOtp = new ApplicationUserOtp(user.Id, otp);

            _context.ApplicationUserOtps.Add(newOtp);
            await _context.SaveChangesAsync();

            var message = $"""
                <h2>CINÉMA Password Reset</h2>
                <p>Your verification code is:</p>
                <h1>{otp}</h1>
                <p>This code expires in 10 minutes.</p>
                """;

            await _emailSender.SendEmailAsync(
                user.Email!,
                "Password Reset OTP",
                message);

            return View("OtpSent");
        }

        // VERIFY OTP

        [HttpGet]
        public IActionResult VerifyOTP(string email)
        {
            return View(new VerifyOTPVM { Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyOTP(VerifyOTPVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid or expired OTP.");
                return View(model);
            }

            var otpRecord = await _context.ApplicationUserOtps
                .Where(x =>
                    x.ApplicationUserId == user.Id &&
                    x.OTP == model.OTP &&
                    x.IsValid)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            if (otpRecord == null || otpRecord.ValidTo <= DateTime.UtcNow)
            {
                ModelState.AddModelError("", "Invalid or expired OTP.");
                return View(model);
            }

            otpRecord.IsValid = false;
            await _context.SaveChangesAsync();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return RedirectToAction(nameof(ResetPassword), new
            {
                email = user.Email,
                token
            });
        }


        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            return View(new ResetPasswordVM
            {
                Email = email,
                Token = token
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return NotFound();

            var result = await _userManager.ResetPasswordAsync(
                user,
                model.Token,
                model.NewPassword);

            if (result.Succeeded)
                return RedirectToAction(nameof(Login));

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }
    }
}
