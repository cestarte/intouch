using Microsoft.AspNetCore.Mvc;
using data;
using data.Repositories;
using application.Dtos;
using application.Extensions;
namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class CollectionsController : ControllerBase
{
  private ICollectionRepository _collectionRepo;

  public CollectionsController(ICollectionRepository repo)
  {
    _collectionRepo = repo;
  }

  // https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-6.0
  [HttpGet]
  public async Task<IActionResult> Get()
  {
    var entities = await _collectionRepo.GetAll();
    return Ok(entities?.Select(x => x.ToDto()));
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> Get(int id)
  {
    var collection = await _collectionRepo.GetById(id);
    return Ok(collection?.ToDto());
  }

  [HttpPost]
  public async Task<IActionResult> Post(CollectionDto dto)
  {
    if (String.IsNullOrWhiteSpace(dto.Name))
    {
      return BadRequest("Name is required.");
    }

    var entity = dto.ToEntity();
    entity.Created = DateTime.Now;
    entity.Modified = entity.Created;

    try
    {
      var id = await _collectionRepo.Create(entity);
      entity.Id = id;
      return Ok(entity.ToDto());
    }
    catch (Exception ex)
    {
      Log.Error($"Error during POST a collection. {dto.ToString()} {ex.FriendlyMessage()}");
      return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Put(int id, CollectionDto dto)
  {
    Collection? entity = null;
    try
    {
      entity = await _collectionRepo.GetById(id);
      if (entity == null)
      {
        return NoContent();
      }
    }
    catch (Exception ex)
    {
      var message = $"Error reading collection {id} for a PUT.";
      Log.Error($"{message} {dto.ToString()} {ex.FriendlyMessage()}");
      return Problem(message);
    }

    entity.Name = dto.Name;
    entity.Description = dto.Description;
    entity.Modified = DateTime.Now;

    try
    {
      if (await _collectionRepo.Update(entity))
        return Ok(_collectionRepo.GetById(id));
      else
        return Problem("Failed to update the entity.");
    }
    catch (Exception ex)
    {
      var message = $"Error saving PUT collection {id}.";
      Log.Error($"{message} {dto.ToString()} {ex.FriendlyMessage()}");
      return Problem(message);
    }
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    try
    {
      if (await _collectionRepo.SoftDelete(id))
      {
        return Ok();
      }
      else
      {
        return NotFound();
      }
    }
    catch (Exception ex)
    {
      var message = $"Error attempting to delete a collection. Id: {id}";
      Log.Error($"{message} {ex.FriendlyMessage()}");
      return Problem(ex.FriendlyMessage());
    }
  }
}
