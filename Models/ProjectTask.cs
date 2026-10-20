using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentScoreManagement.Models
{
    public class ProjectTask
    {
        [Key]
        public int TaskID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "任務名稱")]
        public string TaskName { get; set; }

        [Required]
        [Display(Name = "研究專案")]
        public int ProjectID { get; set; }

        [Required]
        [Display(Name = "分配給學生")]
        public int AssignedStudentID { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "狀態")]
        public string Status { get; set; } = "To Do"; // To Do, In Progress, Completed

        [DataType(DataType.Date)]
        [Display(Name = "截止日期")]
        public DateTime? DueDate { get; set; }

        [ForeignKey("ProjectID")]
        [Display(Name = "專案")]
        public Project? Project { get; set; }

        [ForeignKey("AssignedStudentID")]
        [Display(Name = "指派學生")]
        public Student? AssignedStudent { get; set; }
    }
}
