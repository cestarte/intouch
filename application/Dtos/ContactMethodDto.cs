using System.Text;
namespace application.Dtos;

public class ContactMethodDto {
  public int Id { get; set; }
  public string Type { get; set; } = "";
  public string Value { get; set; } = "";
  public string? Note { get; set; }
  public DateTime Created { get; set; }
  public DateTime Modified { get; set; }
  public bool Deleted { get; set; }

  public int ContactId { get; set; }

  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.Append("ContactMethodDto\r\n");
    sb.Append($"Id: {Id}");
    sb.Append($"Created: {Created}");
    sb.Append($"Modified: {Modified}");
    sb.Append($"Type: {Type}");
    sb.Append($"Note: {Note}");
    sb.Append($"Value: {Value}");
    sb.Append($"ContactId: {ContactId}");
    sb.Append($"Deleted: {Deleted}");
    return sb.ToString();
  }
}