using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vehicle_Loan_Quote.BLL
{
    public class VehicleDetailsBLL
    {
        public decimal vehiclePrice { get; set; }
        public decimal depositAmount { get; set; }
        public DateTime deliveryDate { get; set; }
        public int financeOptions { get; set; }
    }
}