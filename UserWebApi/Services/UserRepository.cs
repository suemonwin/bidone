using UserWebApi.Models;
using System.Diagnostics.CodeAnalysis;


namespace UserWebApi.Services;
public class UserRepository : IDataRepository<User>
{
    private readonly IValidator _userVaildator;
    private readonly IJsonDataService _jsonDataService;
    private readonly ILogger<UserRepository> _logger;
    public UserRepository(ILogger<UserRepository> logger, IJsonDataService jsonDataService, IValidator validator)
    {
        _userVaildator = validator;
        _jsonDataService = jsonDataService;
        _logger = logger;
    }

    [return: MaybeNull]
    public async Task<IEnumerable<User>>? GetAll()
    {
        try
        {
            var datalist = await _jsonDataService.GetDataFromJasonFile() ?? throw new Exception("No Data");
            return datalist;
        }
        catch (Exception e)
        {
            _logger.LogError(message: e.Message);
            return new List<User>();
        }
    }

    [return: MaybeNull]
    public async Task<User>? GetById(int id)
    {
        var dataList = await GetAll();
        var user = dataList.FirstOrDefault(item => item.Id == id);
        if (user is null)
        {
            _logger.LogDebug(message: "User is null", user);
            return new User();
        }
        return user;
    }


    public async Task<User> Create(User data)
    {
        if (data is not null &&
            _userVaildator.SpecialCharacterValidator(data.FirstName)
         && _userVaildator.SpecialCharacterValidator(data.LastName))
        {
            var dataList = await _jsonDataService.GetDataFromJasonFile();
            List<User> users = new List<User>();
            if (dataList is not null)
            {
                User? item = dataList.MaxBy(x => x.Id);
                data.Id = item is not null ? item.Id + 1 : 1;
                users = dataList.ToList();
            }
            else data.Id = 1;

            users.Add(data);
            await _jsonDataService.WriteToFile(users);
            return data;

        }
        return new User();

    }



    public async Task<bool> Update(User data)
    {
        var dataList = await GetAll() ?? new List<User>();
        var existingEntity = dataList.FirstOrDefault(item => item.Id == data.Id);
        if (existingEntity is not null)
        {
            if (_userVaildator.SpecialCharacterValidator(data.FirstName)
            && _userVaildator.SpecialCharacterValidator(data.LastName))
            {
                existingEntity.FirstName = data.FirstName;
                existingEntity.LastName = data.LastName;
                await _jsonDataService.WriteToFile(dataList);
                return true;
            }
            else
            {
                _logger.LogInformation(message: "Special characters are not allowed except for the hyphen (-).", data);
                return false;

            }
        }
        else return false;
    }

    public async Task<bool> Delete(int id)
    {
        var dataList = await GetAll() ?? throw new Exception("No data");
        var userToDelete = dataList.FirstOrDefault(item => item.Id == id);
        if (userToDelete is null)
        {
            _logger.LogDebug(message: "Id not found", id);
            return false;
        }
        else
        {
            List<User> users = dataList.ToList();
            users.Remove(userToDelete);
            await _jsonDataService.WriteToFile(users);
            return true;
        }
    }

}

