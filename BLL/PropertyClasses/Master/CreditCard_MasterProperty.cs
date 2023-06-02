using System;

namespace BLL.PropertyClasses.Master
{
    public class CreditCard_MasterProperty
    {
        public string bank_name { get; set; }
        public Int64 credit_card_id { get; set; }
        public string credit_card_name { get; set; }
        public Int32 credit_card_no { get; set; }
        public Int32 credit_card_pin { get; set; }
        public string adhar_card { get; set; }
        public string pancard { get; set; }
        public Int64 customer_id { get; set; }
    }
}
