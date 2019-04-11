using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlowerMandi.BusinessEntities
{
    public class OrderFields
    {
        public String SLPID { get; set; }
        public String SLPDate { get; set; }
        public float TOTQTY { get; set; }
        public float TOTAMT { get; set; }
        public float GRANDTOT { get; set; }
        public String FarmerId { get; set; }
        public String RecordAddedBy { get; set; }
        public OrderItem [] items { get; set; }
    }
}