using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanIGoOutside.Models
{
    public class Temperature
    {
        public Metric Metric { get; set; }
        public Imperial Imperial { get; set; }
    }
}