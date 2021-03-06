namespace data;

/// <summary>
/// A contact is an entity (person or business) with
/// which there is a desire to document correspondence.
/// </summary>
public class Contact
{
  public int Id { get; set; }
  public int CollectionId { get; set; }
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
