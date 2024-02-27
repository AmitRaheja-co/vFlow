using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vFlow.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [ValidateNever]
        [Required]
        public int AnsId { get; set; }
        [ForeignKey("AnswerId")]
        public DateTime TimeStamp { get; set; }
        [ValidateNever]
        public string AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        [Required]
        public string Body {  get; set; }
    }
}
