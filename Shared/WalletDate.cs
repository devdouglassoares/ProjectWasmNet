using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBlazor.Shared
{
    public class WalletDate
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public decimal Unit { get; set; }
        public decimal Wallet { get; set; }
    }
}