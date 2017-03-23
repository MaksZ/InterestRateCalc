using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace InterestRateCalcService
{
    [ServiceContract]
    public interface IInterestRateCalcService
    {
        [OperationContract]
        List<Customer> GetCustomers();

        [OperationContract]
        List<Agreement> GetAgreements(int customerId);

        [OperationContract]
        UpdateReport UpdateAgreement(Agreement agreement);
    }

    [DataContract]
    public class Customer
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string PersonalId { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }
    }

    [DataContract]
    public class Agreement
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string BaseRateCode { get; set; }

        [DataMember]
        public int Amount { get; set; }

        [DataMember]
        public float Margin { get; set; }

        [DataMember]
        public int Duration { get; set; }
    }

    [DataContract]
    public class UpdateReport
    {
        [DataMember]
        public string CustomerAgreementDescription { get; set; }

        [DataMember]
        public string CurrentBaseRateCode { get; set; }

        [DataMember]
        public decimal CurrentInterestRate { get; set; }

        [DataMember]
        public string NewBaseRateCode { get; set; }

        [DataMember]
        public decimal NewInterestRate { get; set; }

        [DataMember]
        public decimal Difference { get; set; }

        internal static UpdateReport Failed => new UpdateReport { CustomerAgreementDescription = "Error" };
    }
}
