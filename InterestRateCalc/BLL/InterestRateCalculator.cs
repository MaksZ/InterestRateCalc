using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InterestRateCalc.BLL
{
    public interface IBaseRateSource: IDisposable
    {
        Task<decimal> GetRate(string code);
    }

    public class InterestRateCalculator: IDisposable
    {
        private readonly IBaseRateSource source;

        public InterestRateCalculator(IBaseRateSource source)
        {
            this.source = source;
        }

        public async Task<decimal> Calculate(decimal margin, string baseRateCode)
        {
            return margin + await source.GetRate(baseRateCode).ConfigureAwait(false);
        }

        public void Dispose()
        {
            source.Dispose();
        }
    }
}