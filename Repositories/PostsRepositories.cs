using Dapper;
using socialpsql.DTOs;
using socialpsql.Models;
using socialpsql.Utilities;
using socialpsql.Repositories;

public interface IPostRepository
{
    Task<Post> Create(Post Item);

    Task Delete(long Id);
    Task<List<Post>> GetAllForUser(long userId);
    Task<List<Post>> GetList();
    Task<List<Post>> GetAllForHashtag(long HashTagId);

    Task<Post> GetById(int Id);

}

public class PostRepository : BaseRepository, IPostRepository
{
    public PostRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Post> Create(Post Item)
    {
        var query = $@"INSERT INTO {TableNames.posts} (post_id,user_id,no_of_images) VALUES (@PostId, @UserId, @NoOfImages) 
        RETURNING *";

        using (var con = NewConnection)
            return await con.QuerySingleAsync<Post>(query, Item);
    }



    public async Task Delete(long Id)
    {
        var query = $@"DELETE FROM {TableNames.posts} WHERE id = @Id";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, new { Id });
    }



    public async Task<List<Post>> GetAllForUser(long UserId)
    {
        var query = $@"SELECT * FROM {TableNames.posts} WHERE user_id = @UserId";

        using (var con = NewConnection)
            return (await con.QueryAsync<Post>(query, new { UserId })).AsList();
    }

    public async Task<Post> GetById(int PostId)
    {
        var query = $@"SELECT * FROM {TableNames.posts} 
        WHERE post_id = @PostId";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Post>(query, new { PostId });
    }

    public async Task<List<Post>> GetList()
    {
        var query = $@"SELECT * FROM ""{TableNames.posts}""";
        List<Post> res;

        using (var con = NewConnection)
        {
            res = (await con.QueryAsync<Post>(query)).AsList();
        }

        return res;
    }

    public async Task<List<Post>> GetAllForHashtag(long HashTagId)
    {
        var query = $@"SELECT p.* FROM {TableNames.post_hash} hp
        LEFT JOIN {TableNames.posts} p ON p.post_id = hp.post_id
        WHERE hashtag_id = @HashTagId";

        using (var con = NewConnection)
            return (await con.QueryAsync<Post>(query, new { HashTagId })).AsList();
    }
}
