using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace socialpsql.DTOs;

public record UserDTO
{
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }


    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string LastName { get; set; }


    [JsonPropertyName("mobile")]
    public long Mobile { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("post")]
    public List<PostDTO> Post { get;  set;}

    [JsonPropertyName("Likes")]

    public List<LikesDTO> Likes {get;set;}
}

public record UserCreateDTO
{
    public string UserName { get; set; }
    [JsonPropertyName("first_name")]
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    [MaxLength(50)]
    [Required]
    public string LastName { get; set; }

    [JsonPropertyName("mobile")]
    [Required]
    public long Mobile { get; set; }

    [JsonPropertyName("email")]
    [MaxLength(255)]
    public string Email { get; set; }


}

public record UserUpdateDTO
{
    [JsonPropertyName("last_name")]
    [MaxLength(50)]
    public string LastName { get; set; }

    [JsonPropertyName("mobile")]
    public long? Mobile { get; set; }

    [JsonPropertyName("email")]
    [MaxLength(255)]
    public string Email { get; set; }
}