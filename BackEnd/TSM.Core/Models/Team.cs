using System.Collections.Generic;

namespace TSM.Core.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Department { get; set; }

        // Navigation properties
        public ICollection<TechnologyStatus> TechnologyStatuses { get; set; } = new List<TechnologyStatus>();
        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
} 