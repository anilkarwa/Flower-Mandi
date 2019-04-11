using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlowerMandi.BusinessEntities
{
    public class Customers
    {
        public int CustId { get; set; }
        public String CustCode { get; set; }
        public String CustTabCode { get; set; }
        public String CustomerName { get; set; }
        public String InActive { get; set; }
    }
}