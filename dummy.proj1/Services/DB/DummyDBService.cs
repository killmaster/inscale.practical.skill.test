﻿using dummy.Data;
using dummy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dummy.proj1.Services.DB
{
    internal class DummyDBService : IDummyDBService
    {
        private readonly DummyContext _context;

        public DummyDBService(DummyContext context)
        {
            _context = context;
        }

        public async Task AddPost(Post post)
        {
            var checkPost = await _context.Posts.FindAsync(post.Id);
            if (checkPost != null)
                return;
            User user = _context.Users.FirstOrDefault(u => u.Id == post.UserId);
            if (user != null)
                post.User = user;
            var newPost = _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task AddTodo(TodoModel todo)
        {
            var checkTodo = await _context.TodoModels.FindAsync(todo.Id);
            if (checkTodo != null)
                return;
            User user = _context.Users.FirstOrDefault(u => u.Id == todo.UserId);
            if (user != null)
                todo.User = user;
            var newTodo = _context.TodoModels.Add(todo);
            await _context.SaveChangesAsync();
        }

        public async Task AddUser(User user)
        {
            var checkUser = await _context.Users.FindAsync(user.Id);
            if (checkUser != null)
                return;
            var newUser = _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
