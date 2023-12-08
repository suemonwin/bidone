using UserWebApi.Models;
namespace UserWebApi.Services;

public interface IValidator {

 public abstract bool SpecialCharacterValidator(string? val);
 
}