using Microsoft.AspNetCore.Mvc;
using socialpsql.DTOS;
using socialpsql.Models;
using socialpsql.Repositories;

namespace socialpsql.Controllers;


[ApiController]
[Route("api/hashtags")]


public class HashtagController : ControllerBase 
{

    private readonly ILogger<HashtagController> _logger;
    private readonly IHashtagRepository _hashtag;
    private readonly IPostRepository _post;

    private readonly ILikesRepository _likes;

    public HashtagController(ILogger<HashtagController> logger,
    IHashtagRepository hashtags,IPostRepository post,ILikesRepository likes)
    {
        _logger = logger;
        _hashtag = hashtags;
        _post = post;
        _likes = likes;
    }

     [HttpGet]

    public async Task<ActionResult<List<HashtagDTO>>> GetAllusers()
    {
        var usersList = await _hashtag.GetList();

        var dtoList = usersList.Select(x => x.asDto);

        return Ok(dtoList);
    }


   [HttpPost]
    public async Task<ActionResult<HashtagDTO>> CreateUser([FromBody] HashtagCreateDTO Data)
    {

          var post = await _hashtag.GetById(Data.HashTagId);
        if (post is null)
            return NotFound("No post found with given post id");


        var toCreateHashtag = new Hashtag
        {
            HashTagName = Data.HashTagName,
            HashTagId = Data.HashTagId,
           
        };

        var createdHashtag = await _hashtag.Create(toCreateHashtag);

        return StatusCode(StatusCodes.Status201Created, createdHashtag.asDto);
    }


    
    [HttpPut("{hash_id}")]
    public async Task<ActionResult> UpdateHashtag([FromRoute] long id,
    [FromBody] HashtagUpdateDTO Data)
    {
        var existing = await _hashtag.GetById(id);
        if (existing is null)
            return NotFound("No user found with given id");

        var toUpdateHashtag = existing with
        {
          
          HashTagName = Data.HashTagName

           
            
        };

        var didUpdate = await _hashtag.Update(toUpdateHashtag);

        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not update name");

        return NoContent();
    }


     [HttpDelete("{hash_id}")]
    public async Task<ActionResult> DeleteHashtag([FromRoute] long id)
    {
        var existing = await _hashtag.GetById(id);
        if (existing is null)
            return NotFound("No hashtag found with given  name");

        var didDelete = await _hashtag.Delete(id);

        return NoContent();
    }

    
  [HttpGet("{HashTagId}")]
    public async Task<ActionResult<HashtagDTO>> GetHashtagById([FromRoute] long HashTagId)
    {
        var hashtag = await _hashtag.GetById(HashTagId);

        if (hashtag is null)
            return NotFound("No hashtag found with given hashtag_id");

        var dto = hashtag.asDto;
        dto.Post = (await _post.GetAllForHashtag(HashTagId)).Select(x=>x.asDto).ToList();;

        dto.Likes = (await _likes.GetAllForHashtag(HashTagId)).Select(x=>x.asDto).ToList();;


        return Ok(dto);
    }



}