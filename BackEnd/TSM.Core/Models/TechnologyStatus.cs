using System;

namespace TSM.Core.Models
{
    public class TechnologyStatus
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int TechnologyId { get; set; }
        public int StatusId { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public string? Comments { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UserId { get; set; }

        // Navigation properties
        public Team? Team { get; set; }
        public Technology? Technology { get; set; }
        public StatusType? Status { get; set; }
        public ApplicationUser? User { get; set; }
    }
} 