using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ware.Data
{
    public class tmpOrder
    {
        public int IdOrder { get; set; }
        public int IdProduct { get; set; }
        public int Amount { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime FulfilledAt { get; set; }



    }
}
