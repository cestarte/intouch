namespace data;

/// <summary>
/// There may be multiple ways to get in touch with 
/// a given contact. Ex: phone, email, physical address.
/// </summary>
public class ContactMethod
{
  public int Id { get; set; }
  public string Type { get; set; } = "";
  public string Value { get; set; } = "";
  public string? Note { get; set; }
  public DateTime Created { get; set; }
  public DateTime Modified { get; set; }

  public int ContactId { get; set; }

  /// <summary>
  /// A flag representing a soft delete to pretend
  /// a record doesn't exist anymore. This is safer if your 
  /// target audience may make mistakes with their data.
  /// </summary>
  public bool Deleted { get; set; }
}
