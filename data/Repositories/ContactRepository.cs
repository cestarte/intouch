using Dapper;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options; // IOptions

namespace data.Repositories;

public interface IContactRepository {
  /// <returns>
  /// A list of Contacts which have not been soft deleted.
  /// Can be null.
  /// </returns>
  Task<IEnumerable<Contact?>> GetAll();

  /// <returns>
  /// A single Contact. Can be null.
  /// </returns>
  Task<Contact?> GetById(int id);

  /// <returns>
  /// Id of new record
  /// </returns>
  Task<int> Create(Contact entity);

  /// <returns>
  /// true if record(s) altered, false if not
  /// </returns>
  Task<bool> Update(Contact entity);

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

public class ContactRepository : IContactRepository {
  private string _connectionString { get; set; } = "";

  public ContactRepository(IOptions<ConnectionStrings> connectionStrings)
  {
    var strings = connectionStrings.Value;
    _connectionString = strings.InTouch;
  }

  public async Task<IEnumerable<Contact?>> GetAll()
  {
    var sql = @"SELECT * FROM [Contact] 
      WHERE Deleted = @Deleted
      ORDER BY [Name] ASC, [Created] DESC;";
    using (var context = new SqliteConnection(_connectionString)) {
      return await context.QueryAsync<Contact?>(sql, new { Deleted = false });
    }
  }

  public async Task<Contact?> GetById(int id)
  {
    var sql = @"SELECT * FROM [Contact] 
      WHERE Deleted = @Deleted AND Id = @Id;";
    using (var context = new SqliteConnection(_connectionString)) {
      return await context.QuerySingleOrDefaultAsync<Contact?>(sql, new { Deleted = false, Id = id });
    }
  }

  public async Task<int> Create(Contact entity)
  {
    var sql = @"INSERT INTO [Contact] 
    (Name, Description, Created, Modified) 
    VALUES (@Name, @Description, @Created, @Modified)
    RETURNING Id";

    using (var context = new SqliteConnection(_connectionString)) {
      var results = await context.QueryAsync<int>(sql, entity);
      return results.First();
    }
  }

  public async Task<bool> Update(Contact entity)
  {
    using (var context = new SqliteConnection(_connectionString)) {
      var sql = @"UPDATE [Contact] 
      SET Name = @Name, Description = @Description, Modified = @Modified 
      WHERE Id = @Id";

      var numRowsAffected = await context.ExecuteAsync(sql, entity);
      return numRowsAffected > 0 ? true : false;
    }
  }

  public async Task<bool> Delete(int id)
  {
    var sql = @"DELETE FROM [Contact] WHERE Id = @Id;";
    using (var context = new SqliteConnection(_connectionString)) {
      var numRowsAffected = await context.ExecuteAsync(sql, new { Id = id });
      return numRowsAffected > 0 ? true : false;
    }
  }

  public async Task<bool> SoftDelete(int id)
  {
    var sql = @"UPDATE [Contact]
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