using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace vFlow.Models
{
    public class AppUser : IdentityUser
    {
        //[Required]
        //public int Id { get; set; }

        public string? Name { get; set; }

        [Required]
        public string? Role { get; set; }
    }
}
