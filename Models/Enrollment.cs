using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentScoreManagement.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentID { get; set; }

        [Required]
        [Display(Name = "學生")]
        public int StudentID { get; set; }

        [Required]
        [Display(Name = "課程")]
        public int CourseID { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "學期")]
        public string Semester { get; set; }

        [Range(0.00, 100.00)]
        [Display(Name = "成績")]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Grade { get; set; }

        [ForeignKey("StudentID")]
        [Display(Name = "學生")]
        public Student? Student { get; set; }

        [ForeignKey("CourseID")]
        [Display(Name = "課程")]
        public Course? Course { get; set; }
    }
}
