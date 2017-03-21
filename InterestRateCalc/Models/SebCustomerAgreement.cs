using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterestRateCalc.Models
{
    public class SebCustomerAgreement
    {
        public int Id { get; set; }

        [Required]
        [StringLength(9)]
        [Display(Name = "Base rate code")]
        public string BaseRateCode { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public int Amount { get; set; }

        [Required]
        [Display(Name = "Margin")]
        public float Margin { get; set; }

        [Required]
        [Display(Name = "Agreement duration")]
        public int Duration { get; set; }

        public SebCustomer Customer { get; set; }
    }
}