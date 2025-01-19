using Microsoft.EntityFrameworkCore;
using PeopleAPI.Data;
using PeopleAPI.Repositories;
using PeopleAPI.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PeopleDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPeopleUnitOfWork, PeopleUnitOfWork>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IUserFollowsRepository, UserFollowsRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<DataSeeder>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<PeopleDbContext>();
//    context.Database.Migrate();
//}

using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    dataSeeder.Seed();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

