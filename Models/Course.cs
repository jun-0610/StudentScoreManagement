using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace StudentScoreManagement.Models
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "課程名稱")]
        public string CourseName { get; set; }

        [Required]
        [Range(1, 10)]
        [Display(Name = "學分")]
        public int Credits { get; set; }

        [Required]
        [Display(Name = "開設科系")]
        public int DepartmentID { get; set; }

        [Required]
        [Display(Name = "授課教師")]
        public int InstructorID { get; set; }

        [ForeignKey("DepartmentID")]
        [Display(Name = "科系")]
        public Department? Department { get; set; }

        [ForeignKey("InstructorID")]
        [Display(Name = "教師")]
        public Instructor? Instructor { get; set; }

        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}
