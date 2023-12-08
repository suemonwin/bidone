namespace UserWebApi.Services;
public interface IDataRepository<T>
{
   public Task<IEnumerable<T>>? GetAll();
   public Task<T>? GetById(int id);
   public Task<T> Create(T obj);
   public Task<bool> Update(T obj);
   public Task<bool> Delete(int id);

   
}