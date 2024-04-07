using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYHF.Utility
{
    public class SD
    {
        public const string Role_Customer = "Customer";
        //public const string Role_User_Comp = "Company";
        public const string Role_Admin = "Admin";
        public const string Role_Shipper = "Shipper";

        public const string StatusPending = "Pending";
		public const string StatusApproved = "Approved";
		public const string StatusPackaging = "Packaging";
        public const string StatusShipping = "Shipping";
        public const string StatusPacked = "Packed";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusRejected = "Rejected";
	}
}
