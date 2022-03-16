using Dapper;
using socialpsql.DTOS;
using socialpsql.Models;
using socialpsql.Utilities;

namespace socialpsql.Repositories;

public interface IHashtagRepository
{
    Task<Hashtag> Create(Hashtag Item);
    Task<bool> Update(Hashtag Item);
    Task<bool> Delete(long Id);
    Task<Hashtag> GetById(long Id);
    Task<List<Hashtag>> GetAllForPost(long PostId);
    
    Task<List<Hashtag>> GetList();
}

public class HashtagRepository : BaseRepository, IHashtagRepository
{
    public HashtagRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Hashtag> Create(Hashtag Item)
    {
        var query = $@"INSERT INTO ""{TableNames.hashtags}"" 
        (hashtag_name) VALUES (@HashTagName) RETURNING *";

        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<Hashtag>(query, Item);
            return res;
        }
    }

    public async Task<bool> Delete(long Id)
    {
        var query = $@"DELETE FROM ""{TableNames.hashtags}"" 
        WHERE hashtag_id = Hash@Id";

        using (var con = NewConnection)
        {
            var res = await con.ExecuteAsync(query, new { Id });
            return res > 0;
        }
    }

   

    public async Task<List<Hashtag>> GetAllForPost(long PostId)
    {
        var query = $@"SELECT * FROM {TableNames.hashtags} 
        WHERE post_id = @PostId";

        using (var con = NewConnection)
            return (await con.QueryAsync<Hashtag>(query, new {PostId})).AsList();
    }

    

    public async Task<Hashtag> GetById(long Id)
    {
        var query = $@"SELECT * FROM ""{TableNames.hashtags}"" 
        WHERE hashtag_id = @Id";
        
        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Hashtag>(query, new {Id });
    }

    public async Task<List<Hashtag>> GetList()
    {
        var query = $@"SELECT * FROM ""{TableNames.hashtags}""";

        List<Hashtag> res;
        using (var con = NewConnection) 
            res = (await con.QueryAsync<Hashtag>(query)).AsList(); 
        
        return res;
    }

    public async Task<bool> Update(Hashtag Item)
     {
         var query = $@"UPDATE ""{TableNames.hashtags}"" SET  name = @Name WHERE hashtag_id = @Id";
         

         using (var con = NewConnection)
         {
             var rowCount = await con.ExecuteAsync(query, Item);
             return rowCount == 1;
         }
     }

   
   
}

    