using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ware.Data
{
    public class Prod
    {
    
            [Required(ErrorMessage = "Wymagane ID")]
            public int IdProduct { get; set; }


            [Required(ErrorMessage = "Wymagane ID")]
            public int IdWarehouse { get; set; }



            [Required(ErrorMessage = "Wymagane ID")]
            [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
            public int Amount { get; set; }



            [Required(ErrorMessage = "Wymagane ID")]
            public DateTime CreatedAt { get; set; }
        


    }
}
