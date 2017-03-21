using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace InterestRateCalc.Models
{
    public class SebCustomer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(11)]
        [Display(Name = "Personal Id")]
        public string PersonalId { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public virtual ICollection<SebCustomerAgreement> Agreements { get; set; }
    }
}