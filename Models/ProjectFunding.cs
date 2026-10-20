using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentScoreManagement.Models
{
    public class ProjectFunding
    {
        [Key]
        public int FundingID { get; set; }

        [Required]
        [Display(Name = "贊助商/客戶")]
        public int SponsorID { get; set; }

        [Required]
        [Display(Name = "研究專案")]
        public int ProjectID { get; set; }

        [Required]
        [Range(1, 999999999)]
        [Display(Name = "贊助金額")]
        public decimal FundingAmount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "贊助日期")]
        public DateTime FundingDate { get; set; } = DateTime.Today;

        [ForeignKey("SponsorID")]
        [Display(Name = "贊助商")]
        public Sponsor? Sponsor { get; set; }

        [ForeignKey("ProjectID")]
        [Display(Name = "專案")]
        public Project? Project { get; set; }
    }
}
