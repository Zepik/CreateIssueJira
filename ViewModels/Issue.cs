using System.ComponentModel.DataAnnotations;

namespace CreateIssueJira.ViewModels
{
    public class Issue
    {  

        [Required]
        [StringLengthAttribute(100, MinimumLength = 5)]
        public string Summary { get; set; }
        [Required]
        [StringLengthAttribute(200, MinimumLength = 5)]
        public string Description { get; set; }
        [Required]
        public string Reporter { get; set; }

    }
    
}