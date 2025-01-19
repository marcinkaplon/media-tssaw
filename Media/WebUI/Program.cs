using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("IdentityAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:IdentityAPI"]);
});

builder.Services.AddHttpClient("PeopleAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:PeopleAPI"]);
});

builder.Services.AddHttpClient("PostAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:PostAPI"]);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login";
        //options.LogoutPath = "/auth/logout";
    });


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews()
    .AddMvcOptions(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "postDelete",
//    pattern: "post/delete/{id}",
//    defaults: new { controller = "Post", action = "Delete" });

//app.MapControllerRoute(
//    name: "profile",
//    pattern: "profile/{username}",
//    defaults: new { controller = "Profile", action = "Details" });

//app.MapControllerRoute(
//    name: "profileEdit",
//    pattern: "profile/{username}/edit",
//    defaults: new { controller = "Profile", action = "Edit" });

app.Run();

