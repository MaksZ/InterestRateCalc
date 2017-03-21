using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using InterestRateCalc.Models;

namespace InterestRateCalc.DAL
{
    public class SebDbInitializer : DropCreateDatabaseIfModelChanges<SebContext>
    {
        protected override void Seed(SebContext context)
        {
            var customers = new List<SebCustomer>
            {
                new SebCustomer {
                    PersonalId = "67812203006", FirstName = "Goras", LastName = "Trusevičius",
                    Agreements = new List<SebCustomerAgreement>
                    {
                        new SebCustomerAgreement
                        {
                            Amount = 12000,
                            BaseRateCode = "VILIBOR3m",
                            Margin = 1.6f,
                            Duration = 60
                        }
                    }
                },
                new SebCustomer { PersonalId = "78706151287", FirstName = "Dange", LastName = "Kulkavičiutė",
                                    Agreements = new List<SebCustomerAgreement>
                    {
                        new SebCustomerAgreement
                        {
                            Amount = 8000,
                            BaseRateCode = "VILIBOR1y",
                            Margin = 2.2f,
                            Duration = 36
                        },
                        new SebCustomerAgreement
                        {
                            Amount = 1000,
                            BaseRateCode = "VILIBOR6m",
                            Margin = 1.85f,
                            Duration = 24
                        }
                    }
                }
            };

            customers.ForEach(x => context.Customers.Add(x));

            context.SaveChanges();
        }
    }
}