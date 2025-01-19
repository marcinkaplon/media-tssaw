using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _identityApiClient;
        private readonly HttpClient _peopleApiClient;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _identityApiClient = httpClientFactory.CreateClient("IdentityAPI");
            _peopleApiClient = httpClientFactory.CreateClient("PeopleAPI");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var auth_response = await _identityApiClient.PostAsJsonAsync("/auth/register", new
            {
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                Username = model.UsernameUnique
            });

            if (auth_response.IsSuccessStatusCode)
            {

                var auth_response_data = await auth_response.Content.ReadFromJsonAsync<RegistrationResponse>();
                if (auth_response_data is null)
                {
                    throw new BadHttpRequestException("Wrong user");
                }

                var people_response = await _peopleApiClient.PostAsJsonAsync("/user/create", new
                {
                    AuthUserId = auth_response_data.Id,
                    Username = model.Username,
                    UsernameUnique = model.UsernameUnique,
                    Bio = ""
                });

                if (people_response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    var confirmPassword = model.Password;

                    await _identityApiClient.PostAsJsonAsync("/auth/delete-account", new
                    {
                        Email = model.Email,
                        Password = model.Password,
                        ConfirmPassword = confirmPassword
                    });
                    return RedirectToAction("Login");
                }

            }
            else
            {
                var errors = await auth_response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

                if (errors != null)
                {
                    foreach (var error in errors.Errors)
                    {
                        foreach (var errorMessage in error.Value)
                        {
                            ModelState.AddModelError(error.Key, errorMessage);
                        }
                    }
                }
            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _identityApiClient.PostAsJsonAsync("/auth/login", model);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var userResponse = await _peopleApiClient.GetAsync($"/user/getByAuthId/{result.userAuthId}");
                var user = userResponse.IsSuccessStatusCode
                    ? await userResponse.Content.ReadFromJsonAsync<User>()
                    : new User();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UsernameUnique),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("AccessToken", result.Token)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home");

            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.Message))
            {
                ModelState.AddModelError("", errorResponse.Message);
            }
            else
            {
                ModelState.AddModelError("", "Login failed. Please try again.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

