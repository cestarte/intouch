using System.Text;
namespace application.Dtos;

public class CommunicationDto {
  public int Id { get; set; }
  public DateTime Created { get; set; }
  public DateTime Modified { get; set; }
  public DateTime When { get; set; } = DateTime.Now;

  public string Description { get; set; } = "";
  public bool ExpectMeToFollowUp { get; set; }
  public bool ExpectContactToFollowUp { get; set; }
  public bool Deleted { get; set; }

  public int ContactId { get; set; }

  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.Append("CommunicationDto\r\n");
    sb.Append($"Id: {Id}");
    sb.Append($"Created: {Created}");
    sb.Append($"Modified: {Modified}");
    sb.Append($"When: {When.ToShortDateString()}");
    sb.Append($"Description: {Description}");
    sb.Append($"ExpectMeToFollowUp: {ExpectMeToFollowUp}");
    sb.Append($"ExpectContactToFollowUp: {ExpectContactToFollowUp}");
    sb.Append($"ContactId: {ContactId}");
    sb.Append($"Deleted: {Deleted}");
    return sb.ToString();
  }
}
