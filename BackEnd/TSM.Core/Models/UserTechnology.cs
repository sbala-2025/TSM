using System.ComponentModel.DataAnnotations.Schema;

namespace TSM.Core.Models
{
    public class UserTechnology
    {
        public string UserId { get; set; } = string.Empty;
        public int TechnologyId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;
        public Technology Technology { get; set; } = null!;
    }
} 