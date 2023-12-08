

using System.ComponentModel.DataAnnotations;

namespace UserWebApi.Models;
 
 public class User
 { 
  [Required]
   public int Id {get; set;} 
  [Required]
   public string ? FirstName { get; set; }
    [Required]
   public string ? LastName { get; set; }
 }
