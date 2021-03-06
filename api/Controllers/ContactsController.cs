using Microsoft.AspNetCore.Mvc;
using data;
using data.Repositories;
using application.Dtos;
using application.Extensions;
namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactsController : ControllerBase
{
  private IContactRepository _contactRepo;

  public ContactsController(IContactRepository repo)
  {
    _contactRepo = repo;
  }

  // https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-6.0
  [HttpGet]
  public async Task<IActionResult> Get()
  {
    var contacts = await _contactRepo.GetAll();
    return Ok(contacts.Select(x => x.ToDto()));
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> Get(int id)
  {
    var contact = await _contactRepo.GetById(id);
    return Ok(contact?.ToDto());
  }

  [HttpPost]
  public async Task<IActionResult> Post(ContactDto dto)
  {
    if (String.IsNullOrWhiteSpace(dto.Name))
    {
      return BadRequest("Name is required.");
    }
    if (dto.CollectionId == null)
    {
      return BadRequest("Name is required.");
    }

    var entity = dto.ToEntity();
    entity.Created = DateTime.Now;
    entity.Modified = entity.Created;

    try
    {
      var id = await _contactRepo.Create(entity);
      entity.Id = id;
      return Ok(entity.ToDto());
    }
    catch (Exception ex)
    {
      Log.Error($"Error during POST a contact. {dto.ToString()} {ex.FriendlyMessage()}");
      return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Put(int id, ContactDto dto)
  {
    Contact? entity = null;
    try
    {
      entity = await _contactRepo.GetById(id);
      if (entity == null)
      {
        return NoContent();
      }
    }
    catch (Exception ex)
    {
      var message = $"Error reading contact {id} for a PUT.";
      Log.Error($"{message} {dto.ToString()} {ex.FriendlyMessage()}");
      return Problem(message);
    }

    entity.Name = dto.Name;
    entity.Description = dto.Description;
    entity.CollectionId = dto.CollectionId;
    entity.Modified = DateTime.Now;

    try
    {
      if (await _contactRepo.Update(entity))
        return Ok(_contactRepo.GetById(id));
      else
        return Problem("Failed to update the entity.");
    }
    catch (Exception ex)
    {
      var message = $"Error saving PUT contact {id}.";
      Log.Error($"{message} {dto.ToString()} {ex.FriendlyMessage()}");
      return Problem(message);
    }
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    try
    {
      if (await _contactRepo.SoftDelete(id))
      {
        return Ok();
      }
      else
      {
        return NoContent();
      }
    }
    catch (Exception ex)
    {
      var message = $"Error attempting to delete a contact. Id: {id}";
      Log.Error($"{message} {ex.FriendlyMessage()}");
      return Problem(ex.FriendlyMessage());
    }
  }
}
