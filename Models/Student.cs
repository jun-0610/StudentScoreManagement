using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace StudentScoreManagement.Models
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "名字")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "姓氏")]
        public string LastName { get; set; }

        [Display(Name = "姓名")]
        public string FullName => LastName + FirstName;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "電子郵件")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "入學日期")]
        public DateTime EnrollmentDate { get; set; } = DateTime.Today;

        [Required]
        [Display(Name = "主修科系")]
        public int DepartmentID { get; set; }

        [ForeignKey("DepartmentID")]
        [Display(Name = "科系")]
        public Department? Department { get; set; }

        // Navigation properties
        public ICollection<Enrollment>? Enrollments { get; set; }
        public ICollection<ProjectMember>? ProjectMembers { get; set; }
        public ICollection<ProjectTask>? ProjectTasks { get; set; }
    }
}
