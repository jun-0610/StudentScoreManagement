using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace StudentScoreManagement.Models
{
    public class Instructor
    {
        [Key]
        public int InstructorID { get; set; }

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
        [Range(0, 99999999)]
        [Display(Name = "薪資")]
        public decimal Salary { get; set; }

        [Required]
        [Display(Name = "所屬科系/部門")]
        public int DepartmentID { get; set; }

        [Display(Name = "直屬主管")]
        public int? ManagerID { get; set; }

        [ForeignKey("DepartmentID")]
        [Display(Name = "所屬科系")]
        public Department? Department { get; set; }

        [ForeignKey("ManagerID")]
        [Display(Name = "直屬主管")]
        public Instructor? Manager { get; set; }

        // Navigation properties
        public ICollection<Instructor>? Subordinates { get; set; }
        public ICollection<Course>? Courses { get; set; }
        public ICollection<Project>? ProjectsLed { get; set; }
    }
}
