using System.Collections.Generic;

namespace TSM.Core.Models
{
    public class StatusType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<TechnologyStatus> TechnologyStatuses { get; set; } = new List<TechnologyStatus>();
    }
} 