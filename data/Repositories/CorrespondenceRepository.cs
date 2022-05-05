using Dapper;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options; // IOptions

namespace data.Repositories;

public interface ICorrespondenceRepository {
  Task<IEnumerable<CorrespondenceDto?>> Everything();

  Task<IEnumerable<CorrespondenceDto?>> WhoIsWaitingOnMe();
  Task<IEnumerable<CorrespondenceDto?>> WhoAmIWaitingOn();
  Task<IEnumerable<CorrespondenceDto?>> NoExpectedAction();
}

public class CorrespondenceRepository : ICorrespondenceRepository {
  private string _connectionString { get; set; } = "";

  public CorrespondenceRepository(IOptions<ConnectionStrings> connectionStrings)
  {
    var strings = connectionStrings.Value;
    _connectionString = strings.InTouch;
  }

  public async Task<IEnumerable<CorrespondenceDto?>> Everything()
  {
    var sql = @"
    SELECT 
      contact.Id AS ContactId,
      contact.Name AS ContactName,
      contact.Description AS ContactDescription,
      comm.Id AS LastCommunicationId,
      comm.[When],
	    (JULIANDAY('now') - JULIANDAY(comm.[When])) AS DaysSinceWhen,
      comm.Description AS LastCommunication,
      comm.ExpectMeToFollowUp,
      comm.ExpectContactToFollowUp,
	  comm.NumCommunications
    FROM [Contact] AS contact
    LEFT JOIN (
      SELECT Id, [When], Description, 
        ExpectMeToFollowUp, ExpectContactToFollowUp, 
        ContactId, MAX([When]), COUNT(*) AS NumCommunications
      FROM [Communication]
      WHERE Deleted = 0
      GROUP BY ContactId
      ORDER BY [When] DESC 
    ) AS comm ON comm.ContactId = contact.Id
    WHERE
      contact.Deleted = 0
    ORDER BY [When] DESC, [Name] ASC;
    ";
    using (var context = new SqliteConnection(_connectionString)) {
      return await context.QueryAsync<CorrespondenceDto?>(sql);
    }
  }
  public async Task<IEnumerable<CorrespondenceDto?>> WhoIsWaitingOnMe()
  {
    var sql = @"
    SELECT 
      contact.Id AS ContactId,
      contact.Name AS ContactName,
      contact.Description AS ContactDescription,
      comm.Id AS LastCommunicationId,
      comm.[When],
	    (JULIANDAY('now') - JULIANDAY(comm.[When])) AS DaysSinceWhen,
      comm.Description AS LastCommunication,
      comm.ExpectMeToFollowUp,
      comm.ExpectContactToFollowUp,
	  comm.NumCommunications
    FROM [Contact] AS contact
    LEFT JOIN (
      SELECT Id, [When], Description, 
        ExpectMeToFollowUp, ExpectContactToFollowUp, 
        ContactId, MAX([When]), COUNT(*) AS NumCommunications
      FROM [Communication]
      WHERE Deleted = 0
      GROUP BY ContactId
      ORDER BY [When] DESC 
    ) AS comm ON comm.ContactId = contact.Id
    WHERE
      contact.Deleted = 0 AND ExpectMeToFollowUp = 1
    ORDER BY [When] DESC, [Name] ASC
    ";
    using (var context = new SqliteConnection(_connectionString)) {
      return await context.QueryAsync<CorrespondenceDto?>(sql);
    }
  }

  public async Task<IEnumerable<CorrespondenceDto?>> WhoAmIWaitingOn()
  {
    var sql = @"
    SELECT 
      contact.Id AS ContactId,
      contact.Name AS ContactName,
      contact.Description AS ContactDescription,
      comm.Id AS LastCommunicationId,
      comm.[When],
	    (JULIANDAY('now') - JULIANDAY(comm.[When])) AS DaysSinceWhen,
      comm.Description AS LastCommunication,
      comm.ExpectMeToFollowUp,
      comm.ExpectContactToFollowUp,
	  comm.NumCommunications
    FROM [Contact] AS contact
    LEFT JOIN (
      SELECT Id, [When], Description, 
        ExpectMeToFollowUp, ExpectContactToFollowUp, 
        ContactId, MAX([When]), COUNT(*) AS NumCommunications
      FROM [Communication]
      WHERE Deleted = 0
      GROUP BY ContactId
      ORDER BY [When] DESC 
    ) AS comm ON comm.ContactId = contact.Id
    WHERE
      contact.Deleted = 0 AND ExpectContactToFollowUp = 1
    ORDER BY [When] DESC, [Name] ASC
    ";
    using (var context = new SqliteConnection(_connectionString)) {
      return await context.QueryAsync<CorrespondenceDto?>(sql);
    }
  }

  public async Task<IEnumerable<CorrespondenceDto?>> NoExpectedAction()
  {
    var sql = @"
    SELECT 
      contact.Id AS ContactId,
      contact.Name AS ContactName,
      contact.Description AS ContactDescription,
      comm.Id AS LastCommunicationId,
      comm.[When],
	    (JULIANDAY('now') - JULIANDAY(comm.[When])) AS DaysSinceWhen,
      comm.Description AS LastCommunication,
      comm.ExpectMeToFollowUp,
      comm.ExpectContactToFollowUp,
	  comm.NumCommunications
    FROM [Contact] AS contact
    LEFT JOIN (
      SELECT Id, [When], Description, 
        ExpectMeToFollowUp, ExpectContactToFollowUp, 
        ContactId, MAX([When]), COUNT(*) AS NumCommunications
      FROM [Communication]
      WHERE Deleted = 0
      GROUP BY ContactId
      ORDER BY [When] DESC 
    ) AS comm ON comm.ContactId = contact.Id
    WHERE
      contact.Deleted = 0 AND ExpectMeToFollowUp = 0 AND ExpectContactToFollowUp = 0
    ORDER BY [When] DESC, [Name] ASC
    ";
    using (var context = new SqliteConnection(_connectionString)) {
      return await context.QueryAsync<CorrespondenceDto?>(sql);
    }
  }
}