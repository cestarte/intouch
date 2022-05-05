using Dapper;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options; // IOptions

namespace data.Repositories;

public interface ICommunicationRepository {


  /// <returns>
  /// A list of Communications which have not been soft deleted.
  /// Can be null.
  /// </returns>
  Task<IEnumerable<Communication?>> GetAll();


  /// <returns>
  /// A single Communication, can be null.
  /// </returns>
  Task<Communication?> GetById(int id);

  /// <returns>
  /// A list of Communications for a given ContactId,
  /// which have not been soft deleted.
  /// Can be null.
  /// </returns>
  Task<IEnumerable<Communication?>> GetByContactId(int contactId);

  /// <returns>
  /// Id of new record
  /// </returns>
  Task<int> Create(Communication entity);


  /// <returns>
  /// true if record(s) altered, false if not
  /// </returns>
  Task<bool> Update(Communication entity);

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

public class CommunicationRepository : ICommunicationRepository {
  private string _connectionString { get; set; } = "";

  public CommunicationRepository(IOptions<ConnectionStrings> connectionStrings)
  {
    var strings = connectionStrings.Value;
    _connectionString = strings.InTouch;
  }

  public async Task<IEnumerable<Communication?>> GetAll()
  {
    var sql = @"SELECT * FROM [Communication] 
      WHERE Deleted = @Deleted
      ORDER BY [When] DESC;";
    using (var context = new SqliteConnection(_connectionString)) {
      return await context.QueryAsync<Communication?>(sql, new { Deleted = false });
    }
  }

  public async Task<Communication?> GetById(int id)
  {
    var sql = @"SELECT * FROM [Communication] 
      WHERE Deleted = @Deleted AND Id = @Id;";
    using (var context = new SqliteConnection(_connectionString)) {
      return await context.QuerySingleOrDefaultAsync<Communication?>(sql, new { Deleted = false, Id = id });
    }
  }

  public async Task<IEnumerable<Communication?>> GetByContactId(int contactId)
  {
    var sql = @"SELECT * FROM [Communication] 
      WHERE Deleted = @Deleted AND ContactId = @ContactId
      ORDER BY [When] DESC;";
    using (var context = new SqliteConnection(_connectionString)) {
      return await context.QueryAsync<Communication?>(sql, new { Deleted = false, ContactId = contactId });
    }
  }

  public async Task<int> Create(Communication entity)
  {
    var sql = @"INSERT INTO [Communication] 
    (ContactId, [When], Description, ExpectMeToFollowUp, 
      ExpectContactToFollowUp, Created, Modified) 
    VALUES (@ContactId, @When, @Description, @ExpectMeToFollowUp, 
      @ExpectContactToFollowUp, @Created, @Modified)
    RETURNING Id";

    entity.Created = DateTime.Now;
    entity.Modified = entity.Created;

    using (var context = new SqliteConnection(_connectionString)) {
      var results = await context.QueryAsync<int>(sql, entity);
      return results.First();
    }
  }

  public async Task<bool> Update(Communication entity)
  {
    var sql = @"UPDATE [Communication] 
      SET Description = @Description, [When] = @When,
      ExpectMeToFollowUp = @ExpectMeToFollowUp, 
      ExpectContactToFollowUp = @ExpectContactToFollowUp, 
      Modified = @Modified, Deleted = @Deleted 
      WHERE Id = @Id";

    entity.Modified = DateTime.Now;

    using (var context = new SqliteConnection(_connectionString)) {
      var numRowsAffected = await context.ExecuteAsync(sql, entity);
      return numRowsAffected > 0 ? true : false;
    }
  }

  public async Task<bool> Delete(int id)
  {
    var sql = @"DELETE FROM [Communication] WHERE Id = @Id;";
    using (var context = new SqliteConnection(_connectionString)) {
      var numRowsAffected = await context.ExecuteAsync(sql, new { Id = id });
      return numRowsAffected > 0 ? true : false;
    }
  }

  public async Task<bool> SoftDelete(int id)
  {
    var sql = @"UPDATE [Communication]
      SET Deleted = @Deleted,
      Modified = @Modified 
      WHERE Id = @Id";

    using (var context = new SqliteConnection(_connectionString)) {
      var numRowsAffected = await context.ExecuteAsync(sql,
        new { Deleted = true, Modified = DateTime.Now, Id = id });
      return numRowsAffected > 0 ? true : false;
    }
  }
}