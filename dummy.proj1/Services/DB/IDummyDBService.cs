using dummy.Data;
using dummy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dummy.proj1.Services.DB
{
    public interface IDummyDBService
    {
        Task AddUser(User user);
        Task AddPost(Post post);
        Task AddTodo(TodoModel todo);
    }
}
