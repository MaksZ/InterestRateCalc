using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using InterestRateCalc.DAL;
using InterestRateCalc.BLL;
using InterestRateCalc.Models;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;


namespace InterestRateCalcService
{
    /// <remarks>
    /// Actually we shouldn't access SebContext here, 
    /// instead we need to put all logic into separate layer
    /// and use it as from here and as from controller
    /// </remarks>
    public class InterestRateCalcService : IInterestRateCalcService
    {
        public List<Customer> GetCustomers()
        {
            using (var context = new SebContext())
            {
                return context.Customers.Select(ToCustomer).ToList();
            }
        }

        public List<Agreement> GetAgreements(int customerId)
        {
            using (var context = new SebContext())
            {
                return context.Agreements.Where(x => x.Customer.Id == customerId).Select(ToAgreement).ToList();
            }
        }

        public UpdateReport UpdateAgreement(Agreement agreement)
        {
            SebContext context = null;
            try
            {
                context = new SebContext();

                var id = agreement.Id;
                var current = context.Agreements.Include(x => x.Customer).First(x => x.Id == id);

                var oldBaseRateCode = current.BaseRateCode;
                current.BaseRateCode = agreement.BaseRateCode;

                var report = CalculationReport.Build(current, oldBaseRateCode).Result;

                // if we are here let save new value
                context.SaveChanges();

                return ToUpdateReport(report);

            }
            catch
            {
                return UpdateReport.Failed;
            }
            finally
            {
                context?.Dispose();
            }
        }

        private static Customer ToCustomer(SebCustomer entry)
            =>
            new Customer
            {
                Id = entry.Id,
                PersonalId = entry.PersonalId,
                FirstName = entry.FirstName,
                LastName = entry.LastName
            };

        private static Agreement ToAgreement(SebCustomerAgreement entry)
            =>
            new Agreement
            {
                Id = entry.Id,
                Amount = entry.Amount,
                BaseRateCode = entry.BaseRateCode,
                Duration = entry.Duration,
                Margin = entry.Margin
            };

        private static UpdateReport ToUpdateReport(CalculationReport report)
            =>
            new UpdateReport
            {
                CustomerAgreementDescription = report.CustomerAgreementDescription,
                CurrentBaseRateCode = report.CurrentBaseRateCode,
                CurrentInterestRate = report.CurrentInterestRate,
                NewBaseRateCode = report.NewBaseRateCode,
                NewInterestRate = report.NewInterestRate,
                Difference = report.Difference
            };
    }
}
