using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InterestRateCalc.Models;
using System.Threading.Tasks;
using InterestRateCalc.VilibidVilibor;

namespace InterestRateCalc.ViewModels
{
    public class CalculationReport
    {
        public string CustomerAgreementDescription { get; set; }
        public string CurrentBaseRate { get; set; }
        public decimal CurrentInterestRate { get; set; }
        public string NewBaseRate { get; set; }
        public decimal NewInterestRate { get; set; }

        public decimal Difference => CurrentInterestRate - NewInterestRate;

        public async Task<bool> Calculate(decimal margin)
        {
            VilibidViliborSoap svc = null;

            try
            {
                svc = new VilibidViliborSoapClient("VilibidViliborSoap");

                CurrentInterestRate = margin + await svc.getLatestVilibRateAsync(CurrentBaseRate);
                NewInterestRate = margin + await svc.getLatestVilibRateAsync(NewBaseRate);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}