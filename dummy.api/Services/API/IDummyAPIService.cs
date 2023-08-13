using dummy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dummy.api.Services.API
{
    public interface IDummyAPIService
    {
        Task<List<User>> GetUsers();
        Task<List<Post>> GetPosts();
        Task<List<TodoModel>> GetTodos();
    }
}
