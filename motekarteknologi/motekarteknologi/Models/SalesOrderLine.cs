using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace motekarteknologi.Models
{
    public class SalesOrderLine
    {
        public Guid ID { get; set; }
        
        public Guid SalesOrderID { get; set; }
        public SalesOrder SalesOrder { get; set; }

        public Guid ProductID { get; set; }
        public Product Product { get; set; }
        [StringLength(200)]
        [Required]
        public string Description { get; set; }
        [Required]
        public int Qty { get; set; }
    }
}
