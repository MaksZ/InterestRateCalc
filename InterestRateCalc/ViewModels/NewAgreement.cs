using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InterestRateCalc.ViewModels
{
    public class NewAgreement
    {
        public int CustomerId { get; set; }

        [Required]
        [StringLength(9)]
        [Display(Name = "Base rate code")]
        public string BaseRateCode { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public int Amount { get; set; }

        [Required]
        [Range(0.01, 100.0)]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Margin")]
        public float Margin { get; set; }

        [Required]
        [Range(1, 120)]
        [Display(Name = "Agreement duration")]
        public int Duration { get; set; }
    }
}