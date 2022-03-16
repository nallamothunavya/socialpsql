using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using socialpsql.DTOs;

namespace socialpsql.DTOS;

public record HashtagDTO
{
    [JsonPropertyName("hash_id")]
    public long HashTagId { get; set; }

    [JsonPropertyName("hash_name")]
    public string HashTagName { get; set; }


    [JsonPropertyName("post")]
    public List<PostDTO> Post { get; set; }

    [JsonPropertyName("Likes")]

    public List<LikesDTO> Likes {get;set;}
}

public record HashtagCreateDTO
{
    [JsonPropertyName("hash_id")]
    [Required]
    public int HashTagId { get; set; }

    [JsonPropertyName("hash_name")]
    [Required]
    [MaxLength(50)]

    public string HashTagName { get; set; }


}

public record HashtagUpdateDTO
{


    [JsonPropertyName("hash_name")]
    [Required]
    [MaxLength(50)]
    public string HashTagName { get; set; }

}