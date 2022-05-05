using Microsoft.AspNetCore.Mvc;
using data;
using data.Repositories;
using application.Dtos;
namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class CorrespondenceController : ControllerBase {
  private ICorrespondenceRepository _repo;

  public CorrespondenceController(ICorrespondenceRepository correspondenceRepo)
  {
    _repo = correspondenceRepo;
  }

  [HttpGet]
  public async Task<IActionResult> Get()
  {
    return Ok(await _repo.Everything());
  }

  [HttpGet("waitingonme")]
  public async Task<IActionResult> WhoIsWaitingOnMe()
  {
    return Ok(await _repo.WhoIsWaitingOnMe());
  }

  [HttpGet("waitingforcontact")]
  public async Task<IActionResult> WhoAmIWaitingOn()
  {
    return Ok(await _repo.WhoAmIWaitingOn());
  }

  [HttpGet("noaction")]
  public async Task<IActionResult> NoAction()
  {
    return Ok(await _repo.NoExpectedAction());
  }
}
