using Microsoft.AspNetCore.Mvc;
using socialpsql.DTOs;
using socialpsql.Models;
using socialpsql.Repositories;

namespace Instagram.Controllers;


[ApiController]
[Route("api/likes")]
public class LikesController : ControllerBase
{
    private readonly ILogger<LikesController> _logger;
    private readonly IPostRepository _post;
    private readonly ILikesRepository _likes;

    public LikesController(ILogger<LikesController> logger,
    ILikesRepository likes, IPostRepository post)
    {
        _logger = logger;
        _post = post;
        _likes = likes;
    }

    [HttpGet]

    public async Task<ActionResult<List<LikesDTO>>> GetAllUser()
    {
        var usersList = await _likes.GetList();

        
        var dtoList = usersList.Select(x => x.asDto);

        return Ok(dtoList);
    }




    [HttpPost]
    public async Task<ActionResult<LikesCreateDTO>> CreatePost([FromBody] LikesCreateDTO Data)
    {
       

        var toCreateLikes = new Likes
        {

            PostId = Data.PostId,
            UserId = Data.UserId,
            CreatedAt = Data.CreatedAt.UtcDateTime,


        };


        var createdLikes = await _likes.Create(toCreateLikes);

        return StatusCode(StatusCodes.Status201Created, createdLikes);
    }


    [HttpGet("{id}")]

    public async Task<ActionResult<LikesDTO>> GetUserById([FromRoute] int id)
    {
        var user = await _likes.GetById(id);

        if (user is null)
            return NotFound("No user found with id");

        return Ok(user.asDto);
    }



}