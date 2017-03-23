using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterestRateCalc.ViewModels
{
    public class CalculationReport
    {
        public string CustomerAgreementDescription { get; set; }
        public string CurrentBaseRate { get; set; }
        public decimal CurrentInterestRate { get; set; }
        public string NewBaseRate { get; set; }
        public decimal NewInterestRate { get; set; }
        public decimal Difference { get; set; }
    }
}