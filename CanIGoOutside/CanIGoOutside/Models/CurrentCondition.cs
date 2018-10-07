using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanIGoOutside.Models
{
    public class CurrentCondition
    {
        public Temperature Temperature { get; set; }
        public Wind Wind { get; set; }
        public decimal RelativeHumidity { get; set; }
    }
}