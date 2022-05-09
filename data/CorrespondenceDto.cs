namespace data;

/// <summary>
/// A rollup of recent communication with a contact.
/// Used for dashboard summaries.
/// </summary>
public record CorrespondenceDto
{
  public string ContactName { get; init; } = "";
  public string ContactDescription { get; init; } = "";
  public float DaysSinceWhen { get; init; } = -1;
  public string LastCommunication { get; init; } = "";
  public DateTime When { get; init; }

  public bool ExpectContactToFollowUp { get; init; }
  public bool ExpectMeToFollowUp { get; init; }


  public int LastCommunicationId { get; init; } = 0;
  public int ContactId { get; init; } = 0;

  public int NumCommunications { get; init; } = 0;

}