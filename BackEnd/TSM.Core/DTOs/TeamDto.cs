namespace TSM.Core.DTOs
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Department { get; set; }
    }

    public class TeamCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Department { get; set; }
    }

    public class TeamUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Department { get; set; }
    }
} 