using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceTestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new InterestRateCalcService.InterestRateCalcServiceClient("BasicHttpBinding_IInterestRateCalcService");

            // Get and list customers
            var customers = client.GetCustomers();

            while (true)
            {
                foreach (var c in customers)
                {
                    Console.WriteLine($"{c.FirstName} {c.LastName} / {c.PersonalId} / id: {c.Id}");
                }

                var id = Int32.Parse(Console.ReadLine());

                var customer = customers.FirstOrDefault(c => c.Id == id);

                if (customer == null) return;

                // Get and list agreements of selected customer
                var agreements = client.GetAgreements(id);

                foreach (var a in agreements)
                {
                    Console.WriteLine($"{a.Amount} / {a.BaseRateCode} / {a.Margin} / {a.Duration} / id: {a.Id}");
                }

                id = Int32.Parse(Console.ReadLine());

                var agreement = agreements.FirstOrDefault(a => a.Id == id);

                if (agreement == null) return;

                var values = new[] { "VILIBOR1m", "VILIBOR3m", "VILIBOR6m", "VILIBOR1y" };

                var currentValue = agreement.BaseRateCode;
                agreement.BaseRateCode = values.SkipWhile(v => v != currentValue).Skip(1).FirstOrDefault() ?? values[0];

                var report = client.UpdateAgreement(agreement);

                Console.WriteLine($"CustomerAgreementDescription: {report.CustomerAgreementDescription}");
                Console.WriteLine($"   CurrentBaseRateCode: {report.CurrentBaseRateCode}");
                Console.WriteLine($"   CurrentInterestRate: {report.CurrentInterestRate}");
                Console.WriteLine($"       NewBaseRateCode: {report.NewBaseRateCode}");
                Console.WriteLine($"       NewInterestRate: {report.NewInterestRate}");
                Console.WriteLine($"            Difference: {report.Difference}");
                Console.WriteLine($"");

                Console.Write("Continue? (y/n): ");
                if ('y' != Console.ReadKey().KeyChar) break;

                Console.WriteLine();
            }
        }
    }
}
