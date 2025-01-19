using System;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
	public class ProfileController : Controller
	{
        private readonly HttpClient _peopleApiClient;
        private readonly HttpClient _postApiClient;

        public ProfileController(IHttpClientFactory httpClientFactory)
        {
            _postApiClient = httpClientFactory.CreateClient("PostAPI");
            _peopleApiClient = httpClientFactory.CreateClient("PeopleAPI");
        }

        public async Task<IActionResult> Details(string name)
        {
            var userResponse = await _peopleApiClient.GetAsync($"/user/getByUsername/{name}");

            var user = await userResponse.Content.ReadFromJsonAsync<User>();

            if (user == null)
            {
                return NotFound();
            }

            var postResponse = await _postApiClient.GetAsync($"/post/getAllByUser/{user.Id}");
            var posts = postResponse.IsSuccessStatusCode
                    ? await postResponse.Content.ReadFromJsonAsync<List<Post>>()
                    : new List<Post>();

            bool is_followed = false;
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                is_followed = false;
            }
            else
            {
                var folResponse = await _peopleApiClient.GetAsync($"/user/doesUserFollow/{HttpContext.User.Identity.Name}/{user.UsernameUnique}");
                var followFullResponse = await folResponse.Content.ReadFromJsonAsync<FollowResponse>();
                is_followed = followFullResponse.DoesFollow;
            }

            var followerCountResponse = await _peopleApiClient.GetAsync($"/user/getFollowerCount/{user.Id}");
            var followerFullResponse = await followerCountResponse.Content.ReadFromJsonAsync<FollowCountResponse>();

            var followingCountResponse = await _peopleApiClient.GetAsync($"/user/getFollowingCount/{user.Id}");
            var followingFullResponse = await followingCountResponse.Content.ReadFromJsonAsync<FollowCountResponse>();

            var viewModel = new UserProfileViewModel
            {
                User = user,
                Posts = posts,
                IsFollowed = is_followed,
                FollowersCount = followerFullResponse.ResultCount,
                FollowingCount = followingFullResponse.ResultCount
            };

            return View(viewModel);
        }

        public IActionResult EditRedirect(string name)
        {
            return RedirectToAction("Edit", "Profile", routeValues: new { name = name });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string name)
        {
            if (User.Identity.Name != name)
            {
                return Forbid();
            }

            var userResponse = await _peopleApiClient.GetAsync($"/user/getByUsername/{name}");
            if (!userResponse.IsSuccessStatusCode)
            {
                return NotFound("User not found");
            }

            var user = await userResponse.Content.ReadFromJsonAsync<User>();



            var viewModel = new UserEditViewModel
            {
                Username = user.Username,
                Bio = user.Bio,
                ProfilePicturePath = user.ProfilePicturePath
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string name, UserEditViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Forbid();
            }

            var userResponse = await _peopleApiClient.GetAsync($"/user/getByUsername/{User.Identity.Name}");
            var user = await userResponse.Content.ReadFromJsonAsync<User>();
            if (user == null)
            {
                return NotFound();
            }

            string newProfilePicturePath = user.ProfilePicturePath;

            if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
            {
                string oldFilePath = $"wwwroot{user.ProfilePicturePath}";
                if (System.IO.File.Exists(oldFilePath) && user.ProfilePicturePath != "/images/profiles/default_pic.jpg")
                {
                    System.IO.File.Delete(oldFilePath);
                }

                string uniqueFileName = $"images/profiles/{user.UsernameUnique}_{Guid.NewGuid()}.jpg";
                string filePath = Path.Combine("wwwroot", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(stream);
                }
                newProfilePicturePath = $"/{uniqueFileName}";
            }

            var userEditSend = new UserEditSend
            {
                Username = string.IsNullOrEmpty(model.Username) ? user.Username : model.Username,
                Bio = string.IsNullOrEmpty(model.Bio) ? "" : model.Bio,
                ProfilePicturePath = newProfilePicturePath
            };

            var response = await _peopleApiClient.PutAsJsonAsync($"/user/update{user.Id}", userEditSend);
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "Profile", routeValues: new { name = user.UsernameUnique });
        }

        [HttpPost]
        public async Task<IActionResult> Follow(string name)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Forbid();
            }
            var userResponse = await _peopleApiClient.GetAsync($"/user/getByUsername/{name}");
            if (!userResponse.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var user = await userResponse.Content.ReadFromJsonAsync<User>();

            var myUserResponse = await _peopleApiClient.GetAsync($"/user/getByUsername/{HttpContext.User.Identity.Name}");
            if (!myUserResponse.IsSuccessStatusCode)
            {
                return NotFound(); 
            }
            var myUser = await myUserResponse.Content.ReadFromJsonAsync<User>();


            await _peopleApiClient.PostAsync($"/user/follow/{myUser.Id}/{user.Id}", content: null);

            return RedirectToAction("Details", "Profile", routeValues: new { name = name });
        }

        [HttpPost]
        public async Task<IActionResult> Unfollow(string name)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Forbid();
            }
            var userResponse = await _peopleApiClient.GetAsync($"/user/getByUsername/{name}");
            if (!userResponse.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var user = await userResponse.Content.ReadFromJsonAsync<User>();

            var myUserResponse = await _peopleApiClient.GetAsync($"/user/getByUsername/{HttpContext.User.Identity.Name}");
            if (!myUserResponse.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var myUser = await myUserResponse.Content.ReadFromJsonAsync<User>();


            await _peopleApiClient.PostAsync($"/user/unfollow/{myUser.Id}/{user.Id}", content: null);

            return RedirectToAction("Details", "Profile", routeValues: new { name = name });
        }

        public async Task<IActionResult> UserList()
        {
            var userListResponse = await _peopleApiClient.GetAsync($"/user/GetAll");
            var users = userListResponse.IsSuccessStatusCode
                    ? await userListResponse.Content.ReadFromJsonAsync<List<User>>()
                    : new List<User>();
            var viewModel = new UserListViewModel { Users = users };

            return View(viewModel);
        }
    }
}

