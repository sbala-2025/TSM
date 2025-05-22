using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSM.Core.Models
{
    [Table("TechnologySkills")]
    public class TechnologySkill
    {
        [Key]
        [Column("SkillId")]
        public int Id { get; set; }
        
        public int MemberId { get; set; }
        
        public int TechnologyId { get; set; }
        
        public int ProficiencyLevel { get; set; } // 1-5 scale
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? YearsOfExperience { get; set; }
        
        public DateTime? LastUsed { get; set; }

        // Navigation properties
        public TeamMember? TeamMember { get; set; }
        public Technology? Technology { get; set; }
    }
} 