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
            var client = new InterestRateCalcService.InterestRateCalcServiceClient("IRateCalcSoap");
            Console.WriteLine(client.ItWorks(3));

        }
    }
}
