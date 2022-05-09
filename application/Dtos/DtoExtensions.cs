using data;

namespace application.Dtos;
public static class DtoExtensions
{
  public static ContactDto ToDto(this Contact self)
  {
    var target = new ContactDto();
    target.Id = self.Id;
    target.Created = self.Created;
    target.Modified = self.Modified;
    target.Name = self.Name;
    target.Description = self.Description;
    target.Deleted = self.Deleted;
    target.CollectionId = self.CollectionId;
    return target;
  }

  public static Contact ToEntity(this ContactDto self)
  {
    var target = new Contact();
    target.Id = self.Id;
    target.Created = self.Created;
    target.Modified = self.Modified;
    target.Name = self.Name;
    target.Description = self.Description;
    target.Deleted = self.Deleted;
    target.CollectionId = self.CollectionId;
    return target;
  }

  public static CollectionDto ToDto(this Collection self)
  {
    var target = new CollectionDto();
    target.Id = self.Id;
    target.Created = self.Created;
    target.Modified = self.Modified;
    target.Name = self.Name;
    target.Description = self.Description;
    target.Deleted = self.Deleted;
    return target;
  }

  public static Collection ToEntity(this CollectionDto self)
  {
    var target = new Collection();
    target.Id = self.Id;
    target.Created = self.Created;
    target.Modified = self.Modified;
    target.Name = self.Name;
    target.Description = self.Description;
    target.Deleted = self.Deleted;
    return target;
  }

  public static ContactMethodDto ToDto(this ContactMethod self)
  {
    var target = new ContactMethodDto();
    target.Id = self.Id;
    target.Created = self.Created;
    target.Modified = self.Modified;
    target.Note = self.Note;
    target.Type = self.Type;
    target.Value = self.Value;
    target.ContactId = self.ContactId;
    target.Deleted = self.Deleted;
    return target;
  }

  public static ContactMethod ToEntity(this ContactMethodDto self)
  {
    var target = new ContactMethod();
    target.Id = self.Id;
    target.Created = self.Created;
    target.Modified = self.Modified;
    target.Note = self.Note;
    target.Type = self.Type;
    target.Value = self.Value;
    target.ContactId = self.ContactId;
    target.Deleted = self.Deleted;
    return target;
  }

  public static CommunicationDto ToDto(this Communication self)
  {
    var target = new CommunicationDto();
    target.Id = self.Id;
    target.Created = self.Created;
    target.Modified = self.Modified;
    target.When = self.When;
    target.Description = self.Description;
    target.ExpectMeToFollowUp = self.ExpectMeToFollowUp;
    target.ExpectContactToFollowUp = self.ExpectContactToFollowUp;
    target.ContactId = self.ContactId;
    target.Deleted = self.Deleted;
    return target;
  }

  public static Communication ToEntity(this CommunicationDto self)
  {
    var target = new Communication();
    target.Id = self.Id;
    target.Created = self.Created;
    target.Modified = self.Modified;
    target.When = self.When;
    target.Description = self.Description;
    target.ExpectMeToFollowUp = self.ExpectMeToFollowUp;
    target.ExpectContactToFollowUp = self.ExpectContactToFollowUp;
    target.ContactId = self.ContactId;
    target.Deleted = self.Deleted;
    return target;
  }
}
