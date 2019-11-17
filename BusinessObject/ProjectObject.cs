using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessObject
{
    public class ProjectObject
    {
        public int ProjectId { get; set; }
        [Display(Name = "Description")]
        [Required(ErrorMessage = "Enter Description")]
        public string Description { get; set; }

        [Display(Name = "Impact")]
        [Required(ErrorMessage = "Enter Impact")]
        public string Impact { get; set; }

        [Display(Name = "Causes")]
        [Required(ErrorMessage = "Enter Causes")]
        public string Causes { get; set; }

        [Display(Name = "Centre for Pural Action")]
        [Required(ErrorMessage = "Enter Centre for Pural Action")]
        public string CentreforPuralAction { get; set; }

        [Display(Name = "Requirements")]
        [Required(ErrorMessage = "Enter Requirements")]
        public string Requirements { get; set; }

        [Display(Name = "Skills Needed")]
        [Required(ErrorMessage = "Enter Skills Needed")]
        public string SkillsNeeded { get; set; }

        [Display(Name = "Duration")]
        [Required(ErrorMessage = "Enter Duration")]
        public string Duration { get; set; }

        [Display(Name = "Time Commitment")]
        [Required(ErrorMessage = "Enter Time Commitment")]
        public string TimeCommitment { get; set; }

        [Display(Name = "Time Period")]
        [Required(ErrorMessage = "Enter Time Period")]
        public string TimePeriod { get; set; }

        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "Enter Category Name")]
        public string CategoryName { get; set; }
    }
}
