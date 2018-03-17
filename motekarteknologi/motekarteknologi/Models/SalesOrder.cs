using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace motekarteknologi.Models
{
    public class SalesOrder
    {
        public Guid ID { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Description { get; set; }

        public ICollection<SalesOrderLine> Lines { get; set; } = new List<SalesOrderLine>();
    }
}
