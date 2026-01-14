using Ticket.Models.Common;

namespace Ticket.Models
{
    public class Team:BaseEntity
    {
        public string FullName { get;set; }=string.Empty;
        public string Position { get;set; }=string.Empty;
        public string Image { get; set; }
        public string SocialMedia { get; set; }
    }
}
