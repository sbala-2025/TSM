using System.ComponentModel.DataAnnotations;

namespace TSM.Core.DTOs
{
    public class ChangePasswordDto
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; }
    }
} 