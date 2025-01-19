using System;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebUI.Controllers
{
	public class FeedController : Controller
    {
        private readonly HttpClient _peopleApiClient;
        private readonly HttpClient _postApiClient;

        public FeedController(IHttpClientFactory httpClientFactory)
		{
            _postApiClient = httpClientFactory.CreateClient("PostAPI");
            _peopleApiClient = httpClientFactory.CreateClient("PeopleAPI");
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new PostListViewModel();

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var myUserResponse = await _peopleApiClient.GetAsync($"/user/getByUsername/{HttpContext.User.Identity.Name}");
                var myUser = await myUserResponse.Content.ReadFromJsonAsync<User>();

                var followsReponse = await _peopleApiClient.GetAsync($"/user/getFollows/{myUser.Id}");
                var follows = followsReponse.IsSuccessStatusCode
                        ? await followsReponse.Content.ReadFromJsonAsync<List<User>>()
                        : new List<User>();

                List<Post> mainPostList = new List<Post>();

                foreach (var follow in follows)
                {
                    var postResponse = await _postApiClient.GetAsync($"/post/getAllByUser/{follow.Id}");
                    var posts = postResponse.IsSuccessStatusCode
                            ? await postResponse.Content.ReadFromJsonAsync<List<Post>>()
                            : new List<Post>();

                    foreach (var post in posts)
                    {
                        post.Author = follow;
                    }
                    mainPostList.AddRange(posts);
                    mainPostList = mainPostList.OrderByDescending(p => p.CreatedAt).ToList();
                }

                viewModel.Posts = mainPostList;
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(viewModel);
        }

    }
}

