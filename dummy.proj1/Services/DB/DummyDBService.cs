using dummy.Data;
using dummy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dummy.proj1.Services.DB
{
    public class DummyDBService : IDummyDBService
    {
        private readonly DummyContext _context;

        public DummyDBService(DummyContext context)
        {
            _context = context;
        }

        public async Task AddPost(Post post)
        {
            var existingPost = _context.Posts.FirstOrDefault(p => p.Id == post.Id);
            if (existingPost != null)
            {
                existingPost.Tags = post.Tags;
                existingPost.Reactions = post.Reactions;
            }
            else
            {
                User user = _context.Users.FirstOrDefault(u => u.Id == post.UserId);
                if (user != null)
                    post.User = user;
                var newPost = _context.Posts.Add(post);
            }
            await _context.SaveChangesAsync();
        }

        public async Task AddTodo(TodoModel todo)
        {
            var existingTodo = _context.TodoModels.FirstOrDefault(t => t.Id == todo.Id);
            if (existingTodo != null)
            {
                existingTodo.Completed = todo.Completed;
                existingTodo.Todo = todo.Todo;
            }
            else
            {
                User user = _context.Users.FirstOrDefault(u => u.Id == todo.UserId);
                if (user != null)
                    todo.User = user;
                var newTodo = _context.TodoModels.Add(todo);
            }
            await _context.SaveChangesAsync();
        }

        public async Task AddUser(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Bank = user.Bank;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Username = user.Username;
                user.Bank.UserId = user.Id;
                user.Bank.User = user;
                await AddBank(user.Bank);
            }
            else
            {
                _context.Users.Add(user);
                user.Bank.UserId = user.Id;
                user.Bank.User = user;
                await AddBank(user.Bank);
            }
            await _context.SaveChangesAsync();
        }

        public async Task AddBank(Bank bank)
        {
            var existingBank = _context.Bank.FirstOrDefault(b => b.UserId == bank.UserId);
            if (existingBank != null)
            {
                existingBank.CardExpire = bank.CardExpire;
                existingBank.CardNumber = bank.CardNumber;
                existingBank.CardType = bank.CardType;
                existingBank.Currency = bank.Currency;
                existingBank.Iban = bank.Iban;
            }
            else
            {
                _context.Bank.Add(bank);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetUsersWithNReactionsAndXTag(int reactions, string tag)
        {
            var users = _context.Users
            .Where(u => u.Posts.Count(p => p.Reactions > 0) >= reactions && u.Posts.Any(p => p.Tags.Contains(tag)))
            .ToList();
            return users;
        }

        public async Task<List<TodoModel>> TodosOfUsersWithMoreThanNPosts(int v)
        {
            var users = await _context.Users
                .Where(u => u.Posts.Count > v)
                .ToListAsync();

            List<TodoModel> todos = new();
            foreach (var user in users)
            {
                var userTodos = _context.TodoModels
                    .Where(todo => todo.UserId == user.Id)
                    .ToList();
                todos.AddRange(userTodos);
            }
            return todos;
        }

        public async Task<List<Post>> PostsOfUsersWithXCardType(string cardType)
        {
            var users = await _context.Users
                .Where(u => u.Bank.CardType == "mastercard")
                .ToListAsync();
            List<Post> posts = new();
            foreach (var user in users)
            {
                var userPosts = _context.Posts
                    .Where(post => post.UserId == user.Id)
                    .ToList();
                posts.AddRange(userPosts);
            }
            return posts;
        }

        public async Task AddCustomPost(CustomPost customPost)
        {
            var existingPost = _context.CustomPosts.FirstOrDefault(p => p.PostId == customPost.PostId);
            if (existingPost != null)
            {
                existingPost.UserName = customPost.UserName;
                existingPost.HasFrenchTag = customPost.HasFrenchTag;
                existingPost.HasFictionTag = customPost.HasFictionTag;
                existingPost.HasMoreThanTwoReactions = customPost.HasMoreThanTwoReactions;
            }
            else
            {
                _context.CustomPosts.Add(customPost);
            }
            await _context.SaveChangesAsync();
        }
    }
}
