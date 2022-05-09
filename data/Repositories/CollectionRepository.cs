using Dapper;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options; // IOptions

namespace data.Repositories;

public interface ICollectionRepository
{
  /// <returns>
  /// A list of Collections which have not been soft deleted.
  /// Can be null.
  /// </returns>
  Task<IEnumerable<Collection>> GetAll();

  /// <returns>
  /// A single Collection. Can be null.
  /// </returns>
  Task<Collection?> GetById(int id);

  /// <returns>
  /// Id of new record
  /// </returns>
  Task<int> Create(Collection entity);

  /// <returns>
  /// true if record(s) altered, false if not
  /// </returns>
  Task<bool> Update(Collection entity);

  /// <summary>
  /// A soft delete just sets a flag in the database to pretend
  /// a record doesn't exist anymore. This is safer if your 
  /// target audience may make mistakes with their data.
  /// </summary>
  /// <returns>
  /// true if record(s) altered, false if not
  /// </returns>
  Task<bool> SoftDelete(int id);

  /// <summary>
  /// Perform a hard delete. There's no turning back!
  /// </summary>
  /// <returns>
  /// true if record(s) altered, false if not
  /// </returns>
  Task<bool> Delete(int id);

}

public class CollectionRepository : ICollectionRepository
{
  private string _connectionString { get; set; } = "";

  public CollectionRepository(IOptions<ConnectionStrings> connectionStrings)
  {
    var strings = connectionStrings.Value;
    _connectionString = strings.InTouch;
  }

  public async Task<IEnumerable<Collection>> GetAll()
  {
    var sql = @"SELECT * FROM [Collection] 
      WHERE Deleted = @Deleted
      ORDER BY [Name] ASC, [Created] DESC;";
    using (var context = new SqliteConnection(_connectionString))
    {
      return await context.QueryAsync<Collection>(sql, new { Deleted = false });
    }
  }

  public async Task<Collection?> GetById(int id)
  {
    var sql = @"SELECT * FROM [Collection] 
      WHERE Deleted = @Deleted AND Id = @Id;";
    using (var context = new SqliteConnection(_connectionString))
    {
      return await context.QuerySingleOrDefaultAsync<Collection?>(sql, new { Deleted = false, Id = id });
    }
  }

  public async Task<int> Create(Collection entity)
  {
    var sql = @"INSERT INTO [Collection] 
    (Name, Description, Created, Modified) 
    VALUES (@Name, @Description, @Created, @Modified)
    RETURNING Id";

    using (var context = new SqliteConnection(_connectionString))
    {
      var results = await context.QueryAsync<int>(sql, entity);
      return results.First();
    }
  }

  public async Task<bool> Update(Collection entity)
  {
    using (var context = new SqliteConnection(_connectionString))
    {
      var sql = @"UPDATE [Collection] 
      SET Name = @Name, Description = @Description, Modified = @Modified 
      WHERE Id = @Id";

      var numRowsAffected = await context.ExecuteAsync(sql, entity);
      return numRowsAffected > 0 ? true : false;
    }
  }

  public async Task<bool> Delete(int id)
  {
    var sql = @"DELETE FROM [Collection] WHERE Id = @Id;";
    using (var context = new SqliteConnection(_connectionString))
    {
      var numRowsAffected = await context.ExecuteAsync(sql, new { Id = id });
      return numRowsAffected > 0 ? true : false;
    }
  }

  public async Task<bool> SoftDelete(int id)
  {
    var sql = @"UPDATE [Collection]
      SET Deleted = @Deleted,
      Modified = @Modified 
      WHERE Id = @Id";

    using (var context = new SqliteConnection(_connectionString))
    {
      var numRowsAffected = await context.ExecuteAsync(sql,
        new { Deleted = true, Modified = DateTime.Now, Id = id });
      return numRowsAffected > 0 ? true : false;
    }
  }
}