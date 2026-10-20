using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentScoreManagement.Models
{
    public class ProjectMember
    {
        [Required]
        [Display(Name = "研究專案")]
        public int ProjectID { get; set; }

        [Required]
        [Display(Name = "參與學生")]
        public int StudentID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "專案角色")]
        public string Role { get; set; } = "Research Assistant";

        [Required]
        [Range(1, 168)]
        [Display(Name = "分配時數 (每週)")]
        public int HoursAllocated { get; set; } = 10;

        [ForeignKey("ProjectID")]
        [Display(Name = "專案")]
        public Project? Project { get; set; }

        [ForeignKey("StudentID")]
        [Display(Name = "學生")]
        public Student? Student { get; set; }
    }
}
