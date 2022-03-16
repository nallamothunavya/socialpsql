using socialpsql.DTOS;

namespace socialpsql.Models;

public record Hashtag
{
    public long HashTagId { get; set; }
    public string HashTagName { get; set; }



    public HashtagDTO asDto => new HashtagDTO
    {
        HashTagId = HashTagId,
        HashTagName = HashTagName,


    };
}