using System;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
	public class PostController : Controller
    {
        private readonly HttpClient _postApiClient;
        private readonly HttpClient _peopleApiClient;

        public PostController(IHttpClientFactory httpClientFactory)
        {
            _postApiClient = httpClientFactory.CreateClient("PostAPI");
            _peopleApiClient = httpClientFactory.CreateClient("PeopleAPI");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var postResponse = await _postApiClient.GetAsync($"/post/get{id}");
            var post = await postResponse.Content.ReadFromJsonAsync<Post>();
            if (post == null)
            {
                return NotFound();
            }

            var userResponse = await _peopleApiClient.GetAsync($"/user/get{post.UserId}");
            var user = await userResponse.Content.ReadFromJsonAsync<User>();
            if (user == null)
            {
                return NotFound();
            }

            if (user.UsernameUnique != HttpContext.User.Identity.Name)
            {
                return Forbid();
            }

            await _postApiClient.DeleteAsync($"/post/remove{id}");

            string referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var postResponse = await _postApiClient.GetAsync($"/post/get{id}");
            var post = await postResponse.Content.ReadFromJsonAsync<Post>();
            if (post == null)
            {
                return NotFound();
            }

            var postEditSend = new PostEditSend { Content = post.Content };

            return View(postEditSend);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, PostEditSend model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var postResponse = await _postApiClient.GetAsync($"/post/get{id}");
            var post = await postResponse.Content.ReadFromJsonAsync<Post>();
            if (post == null)
            {
                return NotFound();
            }

            var userResponse = await _peopleApiClient.GetAsync($"/user/get{post.UserId}");
            var user = await userResponse.Content.ReadFromJsonAsync<User>();
            if (user == null)
            {
                return NotFound();
            }

            if (user.UsernameUnique != HttpContext.User.Identity.Name)
            {
                return Forbid();
            }

            var postEditSend = new PostEditSend
            {
                Content = model.Content
            };

            var response = await _postApiClient.PutAsJsonAsync($"/post/update{post.Id}", postEditSend);
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "Profile", routeValues: new { name = user.UsernameUnique });
        }
    }
}

