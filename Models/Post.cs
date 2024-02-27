using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vFlow.Models
{
    public class Post
    {
        public int Id { get; set; }
        [ValidateNever]
        public string AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public DateTime TimeStamp { get; set; }
        [Required]
        public string Title { get; set; }
        public List<string>? Tags { get; set; }
        [Required]
        public string Body { get; set; }

    }
}
