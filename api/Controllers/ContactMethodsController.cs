using Microsoft.AspNetCore.Mvc;
using data;
using data.Repositories;
using application.Dtos;
namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactMethodsController : ControllerBase {
  private IContactMethodRepository _repo;

  public ContactMethodsController(IContactMethodRepository contactMethodRepo)
  {
    _repo = contactMethodRepo;
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> Get(int id)
  {
    var entity = await _repo.GetById(id);
    if (entity == null) {
      return NotFound();
    } else {
      return Ok(entity.ToDto());
    }
  }

  [HttpGet("contacts/{contactId}")]
  public async Task<IActionResult> GetMethodsForContact(int contactId)
  {
    var entities = await _repo.GetByContactId(contactId);
    return Ok(entities?.Select(x => x?.ToDto()));
  }

  [HttpPost]
  public async Task<IActionResult> Post(ContactMethodDto dto)
  {
    if (String.IsNullOrWhiteSpace(dto.Type)) {
      return BadRequest("Type is required.");
    }
    if (dto.ContactId < 1) {
      return BadRequest("ContactId is required.");
    }

    var entity = dto.ToEntity();
    entity.Created = DateTime.Now;
    entity.Modified = entity.Created;

    try {
      var id = await _repo.Create(entity);
      entity.Id = id;
      return Ok(entity.ToDto());
    } catch (Exception ex) {
      return Problem(ex.ToString(), null, StatusCodes.Status500InternalServerError);
    }
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Put(int id, ContactMethodDto dto)
  {
    var entity = await _repo.GetById(id);
    if (entity == null) {
      return NotFound();
    }

    entity.Type = dto.Type;
    entity.Note = dto.Note;
    entity.Value = dto.Value;
    entity.Modified = DateTime.Now;

    if (await _repo.Update(entity)) {
      return Ok();
    } else {
      return Problem($"Failed to update the ContactMethod. Id {id}");
    }
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var entity = await _repo.GetById(id);
    if (entity == null) {
      return NotFound();
    }

    if (await _repo.SoftDelete(id)) {
      return Ok();
    } else {
      return Problem($"Failed to delete the ContactMethod. Id {id}");
    }
  }
}
