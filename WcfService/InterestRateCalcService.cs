using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class InterestRateCalcService : IInterestRateCalcService
    {
        public SebCustomer GetCustomer(string personelId)
        {
            using (var context = new InterestRateCalc.DAL.SebContext())
            {
                var customer = context.Customers.FirstOrDefault(x => x.PersonalId == personelId);

                if (customer == null) return null;

                var result = new SebCustomer
                {
                    PersonalId = customer.PersonalId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName
                };

                return result;
            }
        }
    }
}
