using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.DTOs
{
    public class WeeklyTransactionStatDTO
    {
        public string Day { get; set; }    
        public int Count { get; set; }
    }
}
