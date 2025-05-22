namespace TSM.Core.DTOs
{
    public class TechnologyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string? Description { get; set; }
        public string? Version { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }

    public class TechnologyCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string? Description { get; set; }
        public string? Version { get; set; }
    }

    public class TechnologyUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string? Description { get; set; }
        public string? Version { get; set; }
    }
} 