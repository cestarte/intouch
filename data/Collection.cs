namespace data;

/// <summary>
/// A collection holds contacts. One might have personal and
/// business collections to keep the contact lists separate.
/// </summary>
public class Collection
{
  public int Id { get; set; }
  public string Name { get; set; } = "";
  public string? Description { get; set; }
  public DateTime Created { get; set; }
  public DateTime Modified { get; set; }

  /// <summary>
  /// A flag representing a soft delete to pretend
  /// a record doesn't exist anymore. This is safer if your 
  /// target audience may make mistakes with their data.
  /// </summary>
  public bool Deleted { get; set; }
}
