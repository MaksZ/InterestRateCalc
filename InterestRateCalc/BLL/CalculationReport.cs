using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using InterestRateCalc.Models;
using System.Threading.Tasks;

namespace InterestRateCalc.BLL
{
    public class CalculationReport
    {
        public string CustomerAgreementDescription { get; private set; }
        public string CurrentBaseRateCode { get; private set; }
        public decimal CurrentInterestRate { get; private set; }
        public string NewBaseRateCode { get; private set; }
        public decimal NewInterestRate { get; private set; }
        public decimal Difference => CurrentInterestRate - NewInterestRate;

        public static async Task<CalculationReport> Build(SebCustomerAgreement agreement, string oldBaseRateCode)
        {
            using (var calculator = new InterestRateCalculator(new ViliborSource()))
            {
                var a = agreement;
                var c = a.Customer;

                var margin = agreement.Margin;
                var currentRate = await calculator.Calculate((decimal)margin, oldBaseRateCode).ConfigureAwait(false);

                var newBaseRateCode = a.BaseRateCode;
                var newRate = await calculator.Calculate((decimal)margin, newBaseRateCode).ConfigureAwait(false);

                var report = new CalculationReport
                {
                    CustomerAgreementDescription = $"{c.FirstName} {c.LastName} / {a.Amount} / {a.Margin} / {a.Duration}",
                    CurrentBaseRateCode = oldBaseRateCode,
                    CurrentInterestRate = currentRate,
                    NewBaseRateCode = newBaseRateCode,
                    NewInterestRate = newRate
                };

                return report;
            }
        }

        private CalculationReport() { }
    }
}