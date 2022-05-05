using System.Text;
namespace application.Dtos;

public class ContactDto {
  public int Id { get; set; }
  public string Name { get; set; } = "";
  public string? Description { get; set; }
  public DateTime Created { get; set; }
  public DateTime Modified { get; set; }
  public bool Deleted { get; set; }

  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.Append("ContactDto\r\n");
    sb.Append($"Id: {Id}");
    sb.Append($"Created: {Created}");
    sb.Append($"Modified: {Modified}");
    sb.Append($"Name: {Name}");
    sb.Append($"Description: {Description}");
    sb.Append($"Deleted: {Deleted}");
    return sb.ToString();
  }

  public void CopyValuesFrom(ContactDto source)
  {
    Id = source.Id;
    Created = source.Created;
    Modified = source.Modified;
    Name = source.Name;
    Description = source.Description;
    Deleted = source.Deleted;
  }
}

