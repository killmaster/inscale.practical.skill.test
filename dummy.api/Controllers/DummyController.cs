using dummy.contracts;
using dummy.api.Services.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dummy.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DummyController : ControllerBase
    {
        private readonly IDummyDBService _dbService;

        public DummyController(IDummyDBService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("Todos/")]
        [ProducesResponseType(typeof(TodosOfUsersWithMoreThanNPostsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TodosOfUsersWithMoreThanNPosts([FromQuery] TodosOfUsersWithMoreThanNPostsRequest request)
        {
            int posts = 0;
            if (request.NumberOfPosts != null)
                posts = (int)request.NumberOfPosts;
            else return BadRequest();
            var todos = await _dbService.TodosOfUsersWithMoreThanNPosts(posts);
            return Ok(new TodosOfUsersWithMoreThanNPostsResponse()
            {
                Todos = todos.Cast<TodoModel>().ToList()
            });
        }

        [HttpGet("Users/")]
        [ProducesResponseType(typeof(AtLeastNReactionsAndSpecificTagResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UsersWithNReactionsAndXTag([FromQuery] AtLeastNReactionsAndSpecificTagRequest request)
        {
            int reactions = 0;
            if (request.Reactions != null)
                reactions = (int)request.Reactions;
            if (request.Tag == null)
                return BadRequest();

            var users = await _dbService.GetUsersWithNReactionsAndXTag(reactions, request.Tag);
            return Ok(new AtLeastNReactionsAndSpecificTagResponse()
            {
                Users = users.Cast<User>().ToList()
            });
        }

        [HttpGet("Posts/")]
        [ProducesResponseType(typeof(PostsOfUsersWithXCardtypeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostsOfUsersWithXCardType([FromQuery] PostsOfUsersWithXCardtypeRequest request)
        {
            if (string.IsNullOrEmpty(request.Cardtype))
                return BadRequest();
            var posts = await _dbService.PostsOfUsersWithXCardType(request.Cardtype);
            return Ok(new PostsOfUsersWithXCardtypeResponse()
            {
                Posts = posts.Cast<Post>().ToList()
            });
        }
    }
}
