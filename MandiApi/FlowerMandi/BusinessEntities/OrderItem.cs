using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlowerMandi.BusinessEntities
{
    public class OrderItem
    {
        public String SLPID { get; set; }
        public String SlNo { get; set; }
        public String ItemSlNo { get; set; }
        public float QTY { get; set; }
        public String CustID { get; set; }
        public float ItemRate { get; set; }
        public float ItemAmt { get; set; }
        public float BQTY { get; set; }
        public float CustInAmt { get; set; }
        public int ItemID { get; set; }
        public float CustRate { get; set; }
        public String FarmerID { get; set; }
    }
}