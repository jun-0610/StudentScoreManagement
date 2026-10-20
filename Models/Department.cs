using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace StudentScoreManagement.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "科系/部門名稱")]
        public string DepartmentName { get; set; }

        [StringLength(100)]
        [Display(Name = "辦公室位置")]
        public string? OfficeLocation { get; set; }

        [Required]
        [Range(0, 999999999)]
        [Display(Name = "預算")]
        public decimal Budget { get; set; }

        // Navigation properties
        public ICollection<Instructor>? Instructors { get; set; }
        public ICollection<Student>? Students { get; set; }
        public ICollection<Course>? Courses { get; set; }
    }
}
