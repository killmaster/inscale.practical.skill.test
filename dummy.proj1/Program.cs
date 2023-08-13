// See https://aka.ms/new-console-template for more information

using dummy.Data;
using dummy.Models;
using dummy.proj1.Services.API;
using dummy.proj1.Services.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.UserSecrets;

string usersUrl = "https://dummyjson.com/users",
    postsUrl = "https://dummyjson.com/posts",
    todosUrl = "https://dummyjson.com/todos";
var _apiService = new DummyAPIService(usersUrl, postsUrl, todosUrl);
var _context = new DummyContext();
var _dbService = new DummyDBService(_context);

var users = await _apiService.GetUsers();
foreach (User user in users)
{
    await _dbService.AddUser(user);
}

var posts = await _apiService.GetPosts();
foreach (Post post in posts)
{
    await _dbService.AddPost(post);
}

var todos = await _apiService.GetTodos();
foreach (TodoModel todo in todos)
{
    await _dbService.AddTodo(todo);
}

Console.WriteLine(users);
Console.WriteLine(posts);
Console.WriteLine(todos);

var users2 = _context.Users
.Where(u => u.Posts.Count(p => p.Reactions > 0) >= 2 && u.Posts.Any(p => p.Tags.Contains("history")))
.ToList();

foreach (User user in users2)
{
    Console.WriteLine("User: " + user.FirstName);
}


/***
 *
 * This seems overly simple so I'm probably missing something
 * This should be my answer to point 3 of the test
 */
var postsPart2 = _context.Posts.ToList();

foreach (Post post in postsPart2)
{
    CustomPost customPost = new()
    {
        PostId = post.Id,
        UserName = post.User.Username,
        HasFrenchTag = post.Tags.Contains("french"),
        HasFictionTag = post.Tags.Contains("fiction"),
        HasMoreThanTwoReactions = post.Reactions > 2
    };
    await _dbService.AddCustomPost(customPost);
}