using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSM.Core.Models
{
    [Table("TeamMembers")]
    public class TeamMember
    {
        [Key]
        [Column("MemberId")]
        public int Id { get; set; }
        
        public string? UserId { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string? Position { get; set; }
        
        public string? Email { get; set; }
        
        public bool IsExternal { get; set; }

        // Navigation properties
        public ApplicationUser? User { get; set; }
        public ICollection<ProjectTeamMember> ProjectTeamMembers { get; set; } = new List<ProjectTeamMember>();
        public ICollection<TechnologySkill> TechnologySkills { get; set; } = new List<TechnologySkill>();
    }
} 