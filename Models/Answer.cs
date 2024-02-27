using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vFlow.Models
{
    public class Answer
    {
        public int Id { get; set; }
        [Required]
        [ValidateNever]
        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public DateTime TimeStamp { get; set; }
        public List<string>? UpVote { get; set; }
        [Required]
        [ValidateNever]
        public string AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        [Required]
        public string Body { get; set; }

    }
}
