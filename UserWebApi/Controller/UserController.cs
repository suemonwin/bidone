using Microsoft.AspNetCore.Mvc;
using UserWebApi.Models;
using UserWebApi.Services;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IDataRepository<User> _userRepository;
    private List<User> _users;
    private readonly ILogger<UserController> _logger;
    public UserController(IDataRepository<User> userRepository, ILogger<UserController> logger)
    {
        _userRepository = userRepository;
        _users = new List<User>();
        _logger = logger;
    }

    // GET all action
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepository.GetAll();
        if (users == null)
        {
            _logger.LogInformation(message: "No Data");
            return NoContent();
        }
        else return Ok(users);
    }
    // GET by Id action
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userRepository.GetById(id);
        if (user == null)
        {
            _logger.LogInformation(message: "User is Null", id);
            return NotFound();
        }
        return Ok(user);
    }
    // POST action
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        _logger.LogInformation(message: "Create User");
        await _userRepository.Create(user);
        return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
    }
    // PUT action
    [HttpPut]
    public async Task<IActionResult> Update(User user)
    {
        _logger.LogInformation(message: "Update User", user.Id);
        return await _userRepository.Update(user) ? new OkObjectResult(user.Id) : new BadRequestObjectResult(user.Id);
    }
    // DELETE action
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation(message: "Delete User", id);
        return await _userRepository.Delete(id) ? new OkObjectResult(id) : new BadRequestObjectResult(id);
    }

}