namespace data;

public class Communication {
  public int Id { get; set; }
  public DateTime Created { get; set; }
  public DateTime Modified { get; set; }
  public DateTime When { get; set; }

  public string Description { get; set; } = "";
  public bool ExpectMeToFollowUp { get; set; }
  public bool ExpectContactToFollowUp { get; set; }

  public int ContactId { get; set; }

  /// <summary>
  /// A flag representing a soft delete to pretend
  /// a record doesn't exist anymore. This is safer if your 
  /// target audience may make mistakes with their data.
  /// </summary>
  public bool Deleted { get; set; }
}
