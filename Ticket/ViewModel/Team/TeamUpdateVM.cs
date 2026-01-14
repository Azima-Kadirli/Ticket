using System.ComponentModel.DataAnnotations;

namespace Ticket.ViewModel.Team
{
    public class TeamUpdateVM
    {
        public int Id  { get; set; }
        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(255)]
        public string Position { get; set; }
        public IFormFile Image { get; set; }
        [Required]
        public string SocialMedia { get; set; }
    }
}
