using System.Collections.Generic;

namespace TSM.Core.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation properties
        public ICollection<Technology> Technologies { get; set; } = new List<Technology>();
    }
} 