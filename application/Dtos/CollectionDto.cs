namespace application.Dtos;

public class CollectionDto
{
  public int Id { get; set; }
  public string Name { get; set; } = "";
  public string? Description { get; set; }
  public DateTime Created { get; set; }
  public DateTime Modified { get; set; }

  public bool Deleted { get; set; }
}
