using System;

namespace TSM.Core.DTOs
{
    public class TechnologyStatusDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int TechnologyId { get; set; }
        public int StatusId { get; set; }
        public DateTime LastUpdated { get; set; }
        public string? Comments { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        
        // Additional properties for UI display
        public string TeamName { get; set; } = string.Empty;
        public string TechnologyName { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
    }

    public class TechnologyStatusCreateDto
    {
        public int TeamId { get; set; }
        public int TechnologyId { get; set; }
        public int StatusId { get; set; }
        public string? Comments { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UserId { get; set; }
    }

    public class TechnologyStatusUpdateDto
    {
        public int StatusId { get; set; }
        public string? Comments { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UserId { get; set; }
    }

    public class TechnologyStatusMatrixDto
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public int TechnologyId { get; set; }
        public string TechnologyName { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public string? Comments { get; set; }
        public DateTime LastUpdated { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
    }
} 