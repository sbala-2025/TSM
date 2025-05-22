using System.Collections.Generic;

namespace TSM.Core.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class CategoryCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class CategoryUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
} 