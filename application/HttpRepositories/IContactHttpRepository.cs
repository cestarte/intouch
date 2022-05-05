using data;
using application.Dtos;
using System.Net.Http.Json;
namespace application.HttpRepositories;
public interface IContactHttpRepository {
  Task<IEnumerable<ContactDto>> GetContactsAsync();
}

public class ContactHttpRepository {
  private readonly HttpClient _client;

  public ContactHttpRepository(HttpClient client)
  {
    _client = client;
  }

  public async Task<IEnumerable<ContactDto>> GetContactsAsync()
  {
    var response = await _client.GetAsync("contacts");
    if (!response.IsSuccessStatusCode) {
      throw new ApplicationException(response.StatusCode.ToString());
    }
    return await response.Content.ReadFromJsonAsync<IEnumerable<ContactDto>>() ?? new List<ContactDto>();

  }
}