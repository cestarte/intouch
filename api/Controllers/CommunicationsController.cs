using Microsoft.AspNetCore.Mvc;
using data;
using data.Repositories;
using application.Dtos;
namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class CommunicationsController : ControllerBase {
  private ICommunicationRepository _repo;

  public CommunicationsController(ICommunicationRepository communicationRepo)
  {
    _repo = communicationRepo;
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
  public async Task<IActionResult> Post(CommunicationDto dto)
  {
    if (dto.ContactId < 1) {
      return BadRequest("ContactId is required.");
    }

    var entity = dto.ToEntity();
    entity.ContactId = dto.ContactId;
    entity.Description = dto.Description;
    entity.ExpectMeToFollowUp = dto.ExpectMeToFollowUp;
    entity.ExpectContactToFollowUp = dto.ExpectContactToFollowUp;
    entity.When = dto.When;
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
  public async Task<IActionResult> Put(int id, CommunicationDto dto)
  {
    var entity = await _repo.GetById(id);
    if (entity == null) {
      return NotFound();
    }

    entity.When = dto.When;
    entity.Description = dto.Description;
    entity.ExpectMeToFollowUp = dto.ExpectMeToFollowUp;
    entity.ExpectContactToFollowUp = dto.ExpectContactToFollowUp;
    entity.Modified = DateTime.Now;

    if (await _repo.Update(entity)) {
      return Ok();
    } else {
      return Problem($"Failed to update the Communication. Id {id}");
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
      return Problem($"Failed to delete the Communication. Id {id}");
    }
  }
}
