using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XONT.Ventura.VRENQ04
{
    [Serializable()]
    public class ProductClassification
    {
        public string MasterGroup { get; set; }
        public string MasterGroupDescription { get; set; }
        public string MasterGroupValue { get; set; }
        public string MasterGroupValueDescription { get; set; }
    }
}
