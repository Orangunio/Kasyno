using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasyno.Helpers
{
    public class StatsHelper
    {
        public int Amount { get; set; }
        public string Description { get; set; } = "";
        public string Outcome { get; set; } = "";
        public int ResultAmount { get; set; }
        public DateTime GameDate { get; set; }
    }
}
