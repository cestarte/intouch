using Dapper;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options; // IOptions

namespace data.Repositories;

public interface IContactMethodRepository {


  /// <returns>
  /// A list of ContactMethods which have not been soft deleted.
  /// Can be null.
  /// </returns>
  Task<IEnumerable<ContactMethod?>> GetAll();


  /// <returns>
  /// A single ContactMethod, can be null.
  /// </returns>
  Task<ContactMethod?> GetById(int id);

  /// <returns>
  /// A list of ContactMethods for a given ContactId,
  /// which have not been soft deleted.
  /// Can be null.
  /// </returns>
  Task<IEnumerable<ContactMethod?>> GetByContactId(int contactId);

  /// <returns>
  /// Id of new record
  /// </returns>
  Task<int> Create(ContactMethod entity);


  /// <returns>
  /// true if record(s) altered, false if not
  /// </returns>
  Task<bool> Update(ContactMethod entity);

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

public class ContactMethodRepository : IContactMethodRepository {
  private string _connectionString { get; set; } = "";

  public ContactMethodRepository(IOptions<ConnectionStrings> connectionStrings)
  {
    var strings = connectionStrings.Value;
    _connectionString = strings.InTouch;
  }

  public async Task<IEnumerable<ContactMethod?>> GetAll()
  {
    var sql = @"SELECT * FROM [ContactMethod] 
      WHERE Deleted = @Deleted
      ORDER BY [Type] ASC, [Created] DESC;";
    using (var context = new SqliteConnection(_connectionString)) {
      return await context.QueryAsync<ContactMethod?>(sql, new { Deleted = false });
    }
  }

  public async Task<ContactMethod?> GetById(int id)
  {
    var sql = @"SELECT * FROM [ContactMethod] 
      WHERE Deleted = @Deleted AND Id = @Id;";
    using (var context = new SqliteConnection(_connectionString)) {
      return await context.QuerySingleOrDefaultAsync<ContactMethod?>(sql, new { Deleted = false, Id = id });
    }
  }

  public async Task<IEnumerable<ContactMethod?>> GetByContactId(int contactId)
  {
    var sql = @"SELECT * FROM [ContactMethod] 
      WHERE Deleted = @Deleted AND ContactId = @ContactId
      ORDER BY [Type] ASC, [Created] DESC;";
    using (var context = new SqliteConnection(_connectionString)) {
      return await context.QueryAsync<ContactMethod?>(sql, new { Deleted = false, ContactId = contactId });
    }
  }

  public async Task<int> Create(ContactMethod entity)
  {
    var sql = @"INSERT INTO [ContactMethod] 
    (Note, Type, Value, Created, Modified, ContactId) 
    VALUES (@Note, @Type, @Value, @Created, @Modified, @ContactId)
    RETURNING Id";

    using (var context = new SqliteConnection(_connectionString)) {
      var results = await context.QueryAsync<int>(sql, entity);
      return results.First();
    }
  }

  public async Task<bool> Update(ContactMethod entity)
  {
    using (var context = new SqliteConnection(_connectionString)) {
      var sql = @"UPDATE [ContactMethod] 
      SET Note = @Note, Type = @Type, Value = @Value, Modified = @Modified, Deleted = @Deleted 
      WHERE Id = @Id";

      var numRowsAffected = await context.ExecuteAsync(sql, entity);
      return numRowsAffected > 0 ? true : false;
    }
  }

  public async Task<bool> Delete(int id)
  {
    var sql = @"DELETE FROM [ContactMethod] WHERE Id = @Id;";
    using (var context = new SqliteConnection(_connectionString)) {
      var numRowsAffected = await context.ExecuteAsync(sql, new { Id = id });
      return numRowsAffected > 0 ? true : false;
    }
  }

  public async Task<bool> SoftDelete(int id)
  {
    var sql = @"UPDATE [ContactMethod]
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