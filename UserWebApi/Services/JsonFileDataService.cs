using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using UserWebApi.Models;


namespace UserWebApi.Services;
public class JsonFileDataService : IJsonDataService
{
    private static string _filePath = string.Empty;
    private readonly IJsonDataService _jsonDataService;
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _configuration;
    private readonly ILogger<JsonFileDataService> _logger;
    public JsonFileDataService(IWebHostEnvironment env, IConfiguration configuration, ILogger<JsonFileDataService> logger)
    {
        _env = env;
        _configuration = configuration;
        _logger = logger;
        _filePath = _env.ContentRootPath + _configuration["FileSettings:FilePath"];
    }

    public async Task<IEnumerable<User>> GetDataFromJasonFile()
    {
        try
        {
            using FileStream fileStream = File.OpenRead(_filePath);
            fileStream.Position = 0;
            var json = File.ReadAllText(_filePath);
            var datalist = JsonConvert.DeserializeObject<IEnumerable<User>>(json);
            return datalist;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return new List<User>();
        }

    }

    public async Task WriteToFile(IEnumerable<User> dataList)
    {
        try
        {
            using FileStream fileStream = File.Create(_filePath);
            await System.Text.Json.JsonSerializer.SerializeAsync(fileStream, dataList);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

        }
    }


}