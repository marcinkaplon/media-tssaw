using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _peopleApiClient;
    private readonly HttpClient _postApiClient;

    public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _postApiClient = httpClientFactory.CreateClient("PostAPI");
        _peopleApiClient = httpClientFactory.CreateClient("PeopleAPI");
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new PostListViewModel();

        if (User.Identity.IsAuthenticated)
        {
            var postResponse = await _postApiClient.GetAsync($"/post/getAll");
            var posts = postResponse.IsSuccessStatusCode
                    ? await postResponse.Content.ReadFromJsonAsync<List<Post>>()
                    : new List<Post>();

            posts = posts.Take(10).ToList();
            foreach (var post in posts)
            {
                var userResponse = await _peopleApiClient.GetAsync($"/user/get{post.UserId}");
                var user = userResponse.IsSuccessStatusCode
                    ? await userResponse.Content.ReadFromJsonAsync<User>()
                    : new User();
                post.Author = user;
            }

            viewModel.Posts = posts;
        }

        return View(viewModel);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreatePost(string content)
    {
        if (User.Identity.IsAuthenticated)
        {
            var userResponse = await _peopleApiClient.GetAsync($"/user/getByUsername/{User.Identity.Name}");
            var user = await userResponse.Content.ReadFromJsonAsync<User>();

            var newPost = new Post
            {
                UserId = user.Id,
                Content = content
            };

            var response = await _postApiClient.PostAsJsonAsync("/post/create", newPost);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Błąd podczas publikowania posta.");
            }
        }

        return RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

