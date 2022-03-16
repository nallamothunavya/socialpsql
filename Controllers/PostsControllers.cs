using socialpsql.DTOs;
using socialpsql.Models;
using socialpsql.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Controllers;

[ApiController]
[Route("api/post")]
public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly IPostRepository _post;
    private readonly IUserRepository _user;
    private readonly ILikesRepository _likes;
   
    public PostController(ILogger<PostController> logger,
    IPostRepository post, IUserRepository user,ILikesRepository likes)
    {
        _logger = logger;
        _post = post;
        _user = user;
        _likes = likes;

    }


    [HttpGet]
    public async Task<ActionResult<List<PostDTO>>> GetAllUsers()
    {
        var usersList = await _post.GetList();

        var dtoList = usersList.Select(x => x.asDto);

        return Ok(dtoList); 
    }

    [HttpPost]
    public async Task<ActionResult<PostCreateDTO>> CreatePost([FromBody] PostCreateDTO Data)
    {
        

        var toCreatePost = new Post
        {

            
            PostDate = Data.PostDate.UtcDateTime,
            NoOfImages = Data.NoOfImages,
           
            



        };


        var createdPost = await _post.Create(toCreatePost);

        return StatusCode(StatusCodes.Status201Created, createdPost);
    }



    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost([FromRoute] int id)
    {
        var existing = await _post.GetById(id);
        if (existing is null)
            return NotFound("No post found with given id");

        await _post.Delete(id);

        return NoContent();
    }



  [HttpGet("{id}")]
    public async Task<ActionResult<PostDTO>> GetPostById([FromRoute] int id)
    {
        var post = await _post.GetById(id);

        if (post is null)
            return NotFound("No post found with given id");

        var dto = post.asDto;
        dto.Likes = (await _likes.GetAllForPost(post.PostId)).Select(x=>x.asDto).ToList();


        return Ok(dto);
    }
}