using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consimple.Model
{
    public class PurchasesProductsDto
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int PurchaseID { get; set; }
        public decimal Count { get; set; }
    }
}
