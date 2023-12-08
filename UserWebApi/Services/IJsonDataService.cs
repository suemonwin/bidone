using UserWebApi.Models;

namespace UserWebApi.Services
{
    public interface IJsonDataService
    {
        public  Task<IEnumerable<User>> GetDataFromJasonFile();
        public  Task WriteToFile(IEnumerable<User> dataList);
    }
}
