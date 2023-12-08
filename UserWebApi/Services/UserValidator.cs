using System.Text.RegularExpressions;
using UserWebApi.Models;

namespace UserWebApi.Services;

public class UserValidator:IValidator{
      public bool SpecialCharacterValidator(string? val)
    {
        if(string.IsNullOrEmpty(val)) return false;
        return Regex.IsMatch(val, @"^[a-zA-Z0-9-]*$");
    }
}