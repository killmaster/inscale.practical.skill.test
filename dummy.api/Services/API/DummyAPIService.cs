using dummy.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace dummy.api.Services.API
{
    public class DummyAPIService : IDummyAPIService
    {
        private string _usersUrl;
        private string _postsUrl;
        private string _todosUrl;
        public DummyAPIService(string apiUrl, string postsUrl, string todosUrl)
        {
            _usersUrl = apiUrl;
            _postsUrl = postsUrl;
            _todosUrl = todosUrl;
        }

        public async Task<List<Post>> GetPosts()
        {
            using (var client = new HttpClient())
            {
                Task<string> response = client.GetStringAsync(_postsUrl);
                PostsResponse? postsRequest = JsonConvert.DeserializeObject<PostsResponse>(await response);
                List<Post> postsResult = postsRequest.Posts;
                int total = postsRequest.Total;
                int limit = postsRequest.Limit;
                for (int i = limit; i < total; i += limit)
                {
                    Task<string> response2 = client.GetStringAsync(_postsUrl + "?skip=" + i + "&limit=" + limit);
                    PostsResponse? postsRequest2 = JsonConvert.DeserializeObject<PostsResponse>(await response2);
                    postsResult = postsResult.Concat(postsRequest2.Posts).ToList();
                }
                return postsResult;
            }
        }

        public async Task<List<TodoModel>> GetTodos()
        {
            using (var client = new HttpClient())
            {
                Task<string> response = client.GetStringAsync(_todosUrl);
                TodosResponse? todos = JsonConvert.DeserializeObject<TodosResponse>(await response);
                List<TodoModel> todosResult = todos.Todos;
                int total = todos.Total;
                int limit = todos.Limit;
                for (int i = limit; i < total; i += limit)
                {
                    Task<string> response2 = client.GetStringAsync(_todosUrl + "?skip=" + i + "&limit=" + limit);
                    TodosResponse? todosRequest2 = JsonConvert.DeserializeObject<TodosResponse>(await response2);
                    todosResult = todosResult.Concat(todosRequest2.Todos).ToList();
                }
                return todosResult;
            }
        }

        public async Task<List<User>> GetUsers()
        {
            using (var client = new HttpClient())
            {
                Task<string> response = client.GetStringAsync(_usersUrl);
                UsersResponse? usersRequest = JsonConvert.DeserializeObject<UsersResponse>(await response);
                List<User> usersResult = usersRequest.Users;
                int total = usersRequest.Total;
                int limit = usersRequest.Limit;
                for (int i = limit; i < total; i += limit)
                {
                    Task<string> response2 = client.GetStringAsync(_usersUrl + "?skip=" + i + "&limit=" + limit);
                    UsersResponse? usersRequest2 = JsonConvert.DeserializeObject<UsersResponse>(await response2);
                    usersResult = usersResult.Concat(usersRequest2.Users).ToList();
                }
                return usersResult;
            }
        }

        private class UsersResponse
        {
            public List<User> Users { get; set; } = new();
            public int Total { get; set; }
            public int Skip { get; set; }
            public int Limit { get; set; }
        }

        private class PostsResponse
        {
            public List<Post> Posts { get; set; } = new();
            public int Total { get; set; }
            public int Skip { get; set; }
            public int Limit { get; set; }
        }

        private class TodosResponse
        {
            public List<TodoModel> Todos { get; set; } = new();
            public int Total { get; set; }
            public int Skip { get; set; }
            public int Limit { get; set; }
        }
    }
}
