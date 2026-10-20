using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace StudentScoreManagement.Models
{
    public class Sponsor
    {
        [Key]
        public int SponsorID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "贊助商/客戶名稱")]
        public string SponsorName { get; set; }

        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "聯絡信箱")]
        public string? ContactEmail { get; set; }

        public ICollection<ProjectFunding>? ProjectFundings { get; set; }
    }
}
