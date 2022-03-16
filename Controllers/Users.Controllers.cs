using socialpsql.DTOs;
using socialpsql.Models;
using socialpsql.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace socialpsql.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _user;
    private readonly IPostRepository _post;

    private readonly ILikesRepository _likes;

    public UserController(ILogger<UserController> logger, IUserRepository user, IPostRepository post , ILikesRepository likes)
    {
        _logger = logger;
        _user = user;
        _post = post;
        _likes = likes;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetAllUsers()
    {
        var usersList = await _user.GetList();

    
        var dtoList = usersList.Select(x => x.asDto);

        return Ok(dtoList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUserById([FromRoute] long id)
    {
        var user = await _user.GetById(id);

        if (user is null)
            return NotFound("No user found with given id");

        var dto = user.asDto;
        dto.Post = (await _post.GetAllForUser(user.UserId)).Select(x=>x.asDto).ToList();
        dto.Likes = (await _likes.GetAllForUser(user.UserId)).Select(x=>x.asDto).ToList();
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserCreateDTO Data)
    {


        var toCreateUser = new User
        {
            
            FirstName = Data.FirstName.Trim(),
            LastName = Data.LastName.Trim(),
            Mobile = Data.Mobile,
            Email = Data.Email.Trim().ToLower(),


        };

        var createdUser = await _user.Create(toCreateUser);

        return StatusCode(StatusCodes.Status201Created, createdUser.asDto);
    }

    [HttpPut("{user_id}")]
    public async Task<ActionResult> UpdateUser([FromRoute] long user_id,
    [FromBody] UserUpdateDTO Data)
    {
        var existing = await _user.GetById(user_id);
        if (existing is null)
            return NotFound("No user found with given id");

        var toUpdateUser = existing with
        {
            Email = Data.Email?.Trim()?.ToLower() ?? existing.Email,
            LastName = Data.LastName?.Trim() ?? existing.LastName,
          

        };

        var didUpdate = await _user.Update(toUpdateUser);

        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not update user");

        return NoContent();
    }

    [HttpDelete("{user_id}")]
    public async Task<ActionResult> DeleteUser([FromRoute] long user_id)
    {
        var existing = await _user.GetById(user_id);
        if (existing is null)
            return NotFound("No user found with given id");

        var didDelete = await _user.Delete(user_id);

        return NoContent();
    }
}
