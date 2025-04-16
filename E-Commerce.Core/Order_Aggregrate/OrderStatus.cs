using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Order_Aggregrate
{
    public enum OrderStatus
    {
        [EnumMember(Value ="pending")]
        pending,
        [EnumMember(Value = "PaymentSucceeded")]
        PaymentSucceeded,
        [EnumMember(Value = "PaymentFailed")]
        PaymentFailed
    }
}
