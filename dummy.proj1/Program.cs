// See https://aka.ms/new-console-template for more information

using dummy.Data;
using dummy.Models;
using dummy.proj1.Services.API;
using dummy.proj1.Services.DB;

string usersUrl = "https://dummyjson.com/users", 
    postsUrl = "https://dummyjson.com/posts", 
    todosUrl = "https://dummyjson.com/todos";
var _apiService = new DummyAPIService(usersUrl, postsUrl, todosUrl);
var _context = new DummyContext();
var _dbService = new DummyDBService(_context);

var users = await _apiService.GetUsers();
foreach(User user in users)
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

