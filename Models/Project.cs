using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace StudentScoreManagement.Models
{
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "專案名稱")]
        public string ProjectName { get; set; }

        [Required]
        [Range(0, 999999999)]
        [Display(Name = "預算")]
        public decimal Budget { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "開始日期")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        [Display(Name = "結束日期")]
        public DateTime? EndDate { get; set; }

        [Required]
        [Display(Name = "負責教師")]
        public int LeadInstructorID { get; set; }

        [ForeignKey("LeadInstructorID")]
        [Display(Name = "負責教師")]
        public Instructor? LeadInstructor { get; set; }

        // Navigation properties
        public ICollection<ProjectMember>? ProjectMembers { get; set; }
        public ICollection<ProjectTask>? ProjectTasks { get; set; }
        public ICollection<ProjectFunding>? ProjectFundings { get; set; }
    }
}
