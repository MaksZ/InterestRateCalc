using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using InterestRateCalc.VilibidVilibor;
using System.Threading.Tasks;

namespace InterestRateCalc.BLL
{
    public class ViliborSource: IBaseRateSource
    {
        private VilibidViliborSoap svc;

        public ViliborSource()
        {
            svc = new VilibidViliborSoapClient("VilibidViliborSoap");
        }

        public Task<decimal> GetRate(string code)
        {
            if (svc == null) throw new ObjectDisposedException(nameof(ViliborSource));

            return svc.getLatestVilibRateAsync(code);
        }

        public void Dispose()
        {
            svc = null;
        }
    }
}