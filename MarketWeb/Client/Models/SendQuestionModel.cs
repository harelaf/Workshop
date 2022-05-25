using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class SendQuestionModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Title length is 5 to 50 characters.", MinimumLength = 1)]
        public string Title { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Question length is 5 to 500 characters." , MinimumLength = 1)]
        public string Question { get; set; }
    }
}
